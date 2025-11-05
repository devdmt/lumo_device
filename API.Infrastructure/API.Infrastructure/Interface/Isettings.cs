

namespace API.Infrastructure.Interface
{
    public interface Isettings:ITransientService
    {
        void LogRequests(string errMsg, string module, RequestType requestType,string request="");
    }

    public enum RequestType
    {
        Incoming,
        Outgoing,
        Error,
        Info, Comparison, fortesting
    }
}
