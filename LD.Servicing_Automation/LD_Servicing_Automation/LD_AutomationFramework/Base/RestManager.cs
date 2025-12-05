using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using LD_AutomationFramework.Utilities;
using LD_AutomationFramework.Config;
using log4net;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Net.Http.Headers;


namespace LD_AutomationFramework.Base
{
    public class RestManager
    {
        private ExtentTest test { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string m_BaseUrl {  get; set; }
        private string m_Password {  get; set; }
        private string m_Username {  get; set; }
        private HttpWebResponse response { get; set; }
        private EncryptionManager encryption {  get; set; }
        public SecurityProtocolType SecurityProtocolType { get; set; }

        public RestManager(string uriScheme, ExtentTest test)
        {
            InitializeBaseUri(uriScheme);
            this.test = test;
        }

        public RestManager(string uriScheme, string port, ExtentTest test)
        {
            InitializeBaseUri(uriScheme);
            m_BaseUrl += ":" + port;
            this.test = test;
        }
        public RestManager(string uriScheme, string username, string password, ExtentTest test)
        {
            InitializeBaseUri(uriScheme);
            m_Username = username;
            m_Password = password;
            this.test = test;
        }
        public RestManager(string uriScheme, string port, string username, string password, ExtentTest test)
        {
            InitializeBaseUri(uriScheme);
            m_BaseUrl += ":" + port;
            m_Username = username;
            m_Password = password;
            this.test = test;
        }
        public RestManager(string username, string password, ExtentTest test, string baseUrl)
        {
            m_Username = username;
            m_Password = password;
            this.test = test;
            m_BaseUrl = baseUrl;
        }
        private void InitializeBaseUri(string uriScheme = "")
        {
            Uri uri = new Uri(ConfigSettings.AppUrl);
            string uriLeftPart = string.Empty;
            if (!string.IsNullOrEmpty(uriScheme))
            {
                uriLeftPart = uriScheme;
                m_BaseUrl = uriLeftPart;
            }
            else
            {
                uriLeftPart = uri.GetLeftPart(UriPartial.Scheme);
                m_BaseUrl = uriLeftPart + uri.Host.Replace("web", "app");
            }

        }

        public HttpWebResponse GetResponse()
        {
            return response;
        }

        protected string RunQuery(string resource, string argument = null, string data = null, string method = "GET", bool isAuthRequired = false, string authType = "Basic", Dictionary<string, string> headers = null)
        {
            List<Header> headerList = new List<Header>();
            StreamWriter writer = null;
            StreamReader reader = null;
            string url = null;
            string result = string.Empty;
            Header headerRecord = null;
            try
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        headerRecord = new Header();
                        headerRecord.key = header.Key;
                        headerRecord.values = new string[] { header.Value };
                        headerList.Add(headerRecord);
                    }
                }

                url = new Uri(m_BaseUrl) + resource;
                if (argument != null)
                {
                    url = new Uri(url) + "/" + argument;
                    
                }
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Method = method;
                

                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType;

                if(isAuthRequired && authType.Equals("Basic"))
                {
                    string base64Credentials = GetEncodedCredentials();
                    request.Headers.Add("Authorization", "Basic " + base64Credentials);
                }
                if(headers!= null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                if (data != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;
                    using (writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(data);
                    }
                }

                response = request.GetResponse() as HttpWebResponse;
                using (reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch(WebException ex)
            {
                log.Error("Rest Exception occurred"+ ex.InnerException);
                return result;
            }
            finally
            {
                writer?.Close();
                reader?.Close();
            }
        }

        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", m_Username, m_Password);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        public string GetMethod(string resource, string argument = null, bool isAuthRequired = false, string authType = "Basic", Dictionary<string, string> headers = null)
        {
            string responseStream = RunQuery(resource, argument, null, "GET", isAuthRequired, authType, headers);
            return responseStream;
        }

        public string PostMethod(string resource, string argument = null, string data = null, bool isAuthRequired = false, string authType = "Basic", Dictionary<string, string> headers = null)
        {
            string responseStream = RunQuery(resource, argument, data, "POST", isAuthRequired, authType, headers);
            return responseStream;
        }
        public string DeleteMethod(string resource, string argument = null, string data = null, bool isAuthRequired = false, string authType = "Basic", Dictionary<string, string> headers = null)
        {
            string responseStream = RunQuery(resource, argument, data, "DELETE", isAuthRequired, authType, headers);
            return responseStream;
        }
        public string PutMethod(string resource, string argument = null, string data = null, bool isAuthRequired = false, string authType = "Basic", Dictionary<string, string> headers = null)
        {
            string responseStream = RunQuery(resource, argument, data, "PUT", isAuthRequired, authType, headers);
            return responseStream;
        }


    }

    public class Header
    {
        public string key { get; set; }
        public String[] values { get; set; }
    }
}
