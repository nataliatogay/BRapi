using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Utils
{
    public interface ISMSConfiguration
    {
        string AccountSid { get; set; }
        string AuthToken { get; set; }
        string PhoneNumber { get; set; }
    }
    public class TwilioConfiguration : ISMSConfiguration
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string PhoneNumber { get; set; }
    }
}
