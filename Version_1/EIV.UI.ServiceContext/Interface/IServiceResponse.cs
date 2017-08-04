
namespace EIV.UI.ServiceContext.Interface
{
    public interface IServiceResponse
    {
        int StatusCode { get; }
        string Message { get; }
        string Error { get; }
        string ExtraError { get; }
        string Content { get; }
        string QueryUri { get; }
        void SetContent(string content);
    }
}