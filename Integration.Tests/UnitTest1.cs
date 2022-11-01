using HotChocoServer;
using HotChocoServer.Generated;
using Xunit;

namespace Integration.Tests;


public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var excutor = await ConfigureServices.GetGraphQLExecutorAsync();

        var httpClient = new HttpClient(new HotChocoClientHandler(excutor))
        {
            BaseAddress = new Uri("http://bogus:1")
        };

        var qlClient = new GeneratedClient(httpClient);

        //No sorting.
        var noSortResponse = await qlClient.Query(static query => query.Authors(null, o => o.Name));
        Assert.Null(noSortResponse.Errors);
        Assert.Equal(2, noSortResponse.Data.Length);
        Assert.Equal("Scott Hanselman", noSortResponse.Data[0]);
        Assert.Equal("Jon Skeet", noSortResponse.Data[1]);

        //var sortResponse = await qlClient.Query(static query => query.Authors(new AuthorSortInput[] { new AuthorSortInput { Name = SortEnumType.Asc } } , o => o.Name));
        //Assert.Null(sortResponse.Errors);
        //Assert.Equal(2, sortResponse.Data.Length);
        //Assert.Equal("Jon Skeet", sortResponse.Data[0]);
        //Assert.Equal("Scott Hanselman", sortResponse.Data[1]);

    }
}