using HotChocolate;
using HotChocolate.Execution;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ZeroQL.Internal;

namespace Integration.Tests;

public class HotChocoClientHandler : HttpClientHandler
{
    private readonly IRequestExecutor executor;

    public HotChocoClientHandler(IRequestExecutor executor)
    {
        this.executor = executor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
    {
        var requestJson = await httpRequest.Content!.ReadAsStringAsync(cancellationToken);
        var request = JsonConvert.DeserializeObject<GraphQLRequest>(requestJson);
        var executionResult = await executor.ExecuteAsync(request.Query, cancellationToken);
        var json = await executionResult.ToJsonAsync();

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ReadOnlyMemoryContent(Encoding.UTF8.GetBytes(json))
        };
    }
}