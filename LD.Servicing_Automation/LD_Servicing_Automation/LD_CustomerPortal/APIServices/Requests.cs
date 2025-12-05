using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD_CustomerPortal.APIServices
{

    public class GetLoansRequest
    {

        [JsonProperty("loanNumber", Required = Required.DisallowNull)]
        public string LoanNumber { get; set; }
        [JsonProperty("encryptionValue", Required = Required.DisallowNull)]
        public string EncryptionValue { get; set; }
        [JsonProperty("userId", Required = Required.DisallowNull)]
        public string UserId { get; set; }
    }

}


