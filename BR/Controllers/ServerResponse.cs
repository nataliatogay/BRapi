namespace BR.Controllers
{
    public class ServerResponse<T>
    {
        public ServerResponse(StatusCode statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }
        public ServerResponse()
        {

        }

        public StatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }

    public enum StatusCode
    {
        Ok,
        IncorrectVerificationCode
    }
}