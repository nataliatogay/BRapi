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
        Ok,                             // 0
        IncorrectVerificationCode,      // 1
        CodeHasAlreadyBeenSent,         // 2
        Expired,                        // 3
        SendingMessageError,            // 4
        LinkHasAlreadyBeenSent,         // 5
        SendingMailError,               // 6
        UserNotFound,                   // 7
        IncorrectLoginOrPassword,       // 8
        Error,                          // 9
        UserRegistered,                 // 10    
        EmailUsed,                      // 11
        TokenError,                     // 12
        SendOnConfirmation,             // 13
        NotAvailable,                   // 14
        SendingNotificationError,       // 15
        RoleNotFound,                   // 16
        InvalidRole,                    // 17
        UserBlocked,                    // 18
        UserUnblocked,                  // 19
        UserDeleted,                    // 20
        DbConnectionError,              // 21
        NotFound,                       // 22
        Duplicate,                      // 23    
        EmailNotConfirmed,              // 24
        BlobError                       // 25
    }
}