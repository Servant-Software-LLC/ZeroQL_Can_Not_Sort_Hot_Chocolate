using HotChocolate;
using HotChocolate.AspNetCore.Serialization;
using HotChocolate.Execution;
using System.Net;
using System.Text;

namespace Integration.Tests;

public class HotChocoClientHandler : HttpClientHandler
{
    private readonly IRequestExecutor executor;
    private readonly IHttpRequestParser httpRequestParser;

    public HotChocoClientHandler(IRequestExecutor executor, IHttpRequestParser httpRequestParser)
    {
        this.executor = executor ?? throw new ArgumentNullException(nameof(executor));
        this.httpRequestParser = httpRequestParser ?? throw new ArgumentNullException(nameof(httpRequestParser));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
    {
        var requestStream = await httpRequest.Content!.ReadAsStreamAsync(cancellationToken);
        var graphQLRequests = await httpRequestParser.ReadJsonRequestAsync(requestStream, cancellationToken);

        string json = string.Empty;
        foreach(HotChocolate.Language.GraphQLRequest graphQLRequest in graphQLRequests)
        {
            var query = graphQLRequest.Query!.ToString();
            var executionResult = graphQLRequest.Variables is null ? 
                await executor.ExecuteAsync(query, cancellationToken) : 
                await executor.ExecuteAsync(query, graphQLRequest.Variables, cancellationToken);
            json += await executionResult.ToJsonAsync();
        }

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ReadOnlyMemoryContent(Encoding.UTF8.GetBytes(json))
        };
    }
}