using HotChocolate.AspNetCore.Serialization;
using HotChocoServer;
using HotChocoServer.Generated;
using Xunit;

namespace Integration.Tests;


public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var (executor, httpRequestParser) = await ConfigureServices.GetGraphQLExecutorAsync<IHttpRequestParser>();

        var httpClient = new HttpClient(new HotChocoClientHandler(executor, httpRequestParser))
        {
            BaseAddress = new Uri("http://bogus:1")
        };

        var qlClient = new GeneratedClient(httpClient);

        //No sorting.
        var noSortResponse = await qlClient.Query(static query => query.Authors(null, o => o.Name));
        Assert.Null(noSortResponse.Errors);
        Assert.Equal(2, noSortResponse.Data!.Length);
        Assert.Equal("Scott Hanselman", noSortResponse.Data[0]);
        Assert.Equal("Jon Skeet", noSortResponse.Data[1]);

        var variables = new { Sorting = new AuthorSortInput[] { new AuthorSortInput { Name = SortEnumType.Asc } } };
        var sortResponse = await qlClient.Query(variables, static (input, query) => query.Authors(input.Sorting, o => o.Name));
        Assert.Null(sortResponse.Errors);
        Assert.Equal(2, sortResponse.Data!.Length);
        Assert.Equal("Jon Skeet", sortResponse.Data[0]);
        Assert.Equal("Scott Hanselman", sortResponse.Data[1]);

    }
}