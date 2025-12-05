using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD_CustomerPortal.APIServices
{
    /// <summary>
    /// All Servis bot API methods
    /// </summary>
    internal class ServisBotApi : BasePage
    {

        string encryptionUri = APIConstants.CustomerPortalURIs.EncryptionURI.Replace("{ENVIRONMENT}", ConfigSettings.Environment.ToLower());
        string servisBotUri = APIConstants.CustomerPortalURIs.ServisBotURI.Replace("{ENVIRONMENT}", ConfigSettings.Environment.ToLower());
        Dictionary<string,string> headers = new Dictionary<string,string>();

        RestManager restManager;
        public ServisBotApi()
        {
            restManager = new RestManager(servisBotUri, test);
            headers.Add("Channel-Id", "2");
            headers.Add("Ocp-Apim-Subscription-Key", APIConstants.ServisBotSubscriptions.SubscriptionKey);
        }
        /// <summary>
        /// Generated encryption key for given loan number
        /// </summary>
        /// <param name="loanNumber">loan_number</param>
        /// <returns></returns>
        private string GetEncryptionKey(string loanNumber)
        {
            string secretKey = string.Empty;

            try
            {
                var restManager = new RestManager(encryptionUri, test);
                var urlEndPoint = APIConstants.CustomerPortalResourcePaths.GET_EncryptionKey.Replace("{loan_number}", loanNumber);
                var responsejson = JObject.Parse(restManager.GetMethod(urlEndPoint));
                var responseObject = JsonUtils.GetDynamicFromJsonString(responsejson.ToString());

                secretKey= responseObject.secretKey;

            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Error, $"Error while generating the encryption key for {loanNumber}" + ex.Message);
            }
            return secretKey;

        }
        /// <summary>
        /// Call GetLoan API and return the response
        /// </summary>
        /// <param name="loanNumber">loan_number</param>
        /// <param name="userid">borrower id</param>
        /// <returns></returns>

        public dynamic GetLoanDetails(string loanNumber,string userid)
        {
            try
            {
                var urlEndPoint = APIConstants.CustomerPortalResourcePaths.POST_GetLoan.Replace("{loan_number}", loanNumber);
                var encryptionKey = GetEncryptionKey(loanNumber);

                var payload = JsonUtils.GetJsonStringFromObject(
                    new GetLoansRequest
                    {
                        LoanNumber = loanNumber,
                        EncryptionValue = encryptionKey,
                        UserId = userid
                    });
                var response = restManager.PostMethod(resource: urlEndPoint, data: payload, headers: headers);

                JObject json = JObject.Parse(response);
                return JsonUtils.GetDynamicFromJsonString(json.ToString());
            }catch(Exception e)
            {
                test.Log(AventStack.ExtentReports.Status.Error, $"Error while getting loan details {e.Message}");
            }
            return null;

        }

    }
}
