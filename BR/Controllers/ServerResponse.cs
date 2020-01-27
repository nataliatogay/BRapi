namespace BR.Controllers
{

    public class ServerResponse
    {
        public ServerResponse(StatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public StatusCode StatusCode { get; set; }
        //public Response(StatusCode statusCode)
        //{
        //    StatusCode = statusCode;
        //}


    }

    public class ServerResponse<T> : ServerResponse
    {
        public ServerResponse(StatusCode statusCode, T data) : base(statusCode)
        {
            Data = data;
        }

        public T Data { get; set; }
    }

    public enum StatusCode
    {
        Ok,
        IncorrectVerificationCode,
        CodeHasAlreadyBeenSent,
        Expired,
        SendingMessageError,
        LinkHasAlreadyBeenSent,
        SendingMailError,
        UserNotFound,
        IncorrectLoginOrPassword,
        UserBlocked,
        Error,
        UserRegistered
    }
}