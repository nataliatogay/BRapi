using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : ControllerBase
    {
        protected ServerResponse Response(StatusCode statusCode)
        {
            return new ServerResponse(statusCode);
        }

        protected ServerResponse<T> Response<T> (T data)
        {
            return new ServerResponse<T>(Controllers.StatusCode.Ok, data);
        }

        protected ServerResponse<T> Response<T>(StatusCode statusCode, T  data)
        {
            return new ServerResponse<T>(statusCode, data);
        }

    }
}

