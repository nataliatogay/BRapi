namespace BR.Utils
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
        public ServerResponse(T data) : base(StatusCode.Ok)
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
        UserRegistered,
        EmailUsed,
        TokenError,
        SendOnConfirmation,
        NotAvailable,
        SendingNotificationError,
        ClientIsBlocked,
        ClientNotFound,
        WaiterNotFound
    }
}