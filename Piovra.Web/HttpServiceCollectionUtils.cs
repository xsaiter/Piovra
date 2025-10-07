using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Piovra.Web;

public static class HttpServiceCollectionUtils {
    public static IHttpClientBuilder AddNamedHttpClient<T, TImpl>(this IServiceCollection services, Action<HttpClient> configureClient)
        where T : class where TImpl : class, T, IStaticClientNameKnown<TImpl> {
        var clientName = TImpl.ClientName();
        return services.AddHttpClient<T, TImpl>(clientName, configureClient);
    }
}
