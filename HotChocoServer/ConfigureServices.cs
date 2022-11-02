using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using HotChocoServer;

namespace HotChocoServer;

public static class ConfigureServices
{
    public static void AddServices(IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .AddSorting()
            .AddQueryType<Query>();
    }

    public static IRequestExecutorResolver GetGraphQLRequestExecutorResolver()
    {
        var serviceCollection = CreateServiceCollection();

        AddServices(serviceCollection);

        //Now get the executor resolver
        return serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<IRequestExecutorResolver>();
    }

    public static (IRequestExecutorResolver, TExtraService) GetGraphQLRequestExecutorResolver<TExtraService>()
    {
        var serviceCollection = CreateServiceCollection();

        AddServices(serviceCollection);

        var serviceProvider = serviceCollection
                .BuildServiceProvider();
        var requestExecutorResolver = serviceProvider.GetRequiredService<IRequestExecutorResolver>();
        var extraService = serviceProvider.GetRequiredService<TExtraService>();

        return (requestExecutorResolver, extraService);
    }


    public static async Task<IRequestExecutor> GetGraphQLExecutorAsync()
    {
        return await GetGraphQLRequestExecutorResolver()
        .GetRequestExecutorAsync();
    }

    public static async Task<(IRequestExecutor, TExtraService)> GetGraphQLExecutorAsync<TExtraService>()
    {
        var (requestExecutorResolver, extraService) = GetGraphQLRequestExecutorResolver<TExtraService>();
        var requestExecutor = await requestExecutorResolver.GetRequestExecutorAsync();

        return (requestExecutor, extraService);
    }

    /// <summary>
    /// Define all singleton services here.
    /// </summary>
    /// <returns></returns>
    private static IServiceCollection CreateServiceCollection()
    {
        var serviceCollection = new ServiceCollection();
        return serviceCollection;
    }
}
