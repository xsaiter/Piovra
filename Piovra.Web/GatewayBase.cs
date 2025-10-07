using System.Net.Http;

namespace Piovra.Web;

public abstract class GatewayBase<T>(HttpClient client) where T : IStaticClientNameKnown<T>
{
    protected readonly HttpClient _client = client;

    protected string GetClientName()
    {
        return T.ClientName();
    }
}
