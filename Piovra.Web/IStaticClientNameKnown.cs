namespace Piovra.Web;

public interface IStaticClientNameKnown<T> where T : IStaticClientNameKnown<T> {
    public virtual static string ClientName() {
        return typeof(T).FullName ?? throw new Exception("No client name");
    }
}
