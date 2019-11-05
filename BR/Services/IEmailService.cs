using BR.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IEmailService
    {
        Task SendAsync(SendMailRequest sendMailInfo /*, string userId = null*/);
    }
}
