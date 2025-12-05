using LD_PaymentEngine.APIServices;
using Microsoft.Testing.Platform.Extensions.Messages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static LD_AutomationFramework.APIConstants;
using System.Collections;
using LD_AutomationFramework;
using static log4net.Appender.RollingFileAppender;
using Newtonsoft.Json.Linq;
using AventStack.ExtentReports.Model;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System.Runtime.Remoting.Contexts;
using System.ComponentModel.Design.Serialization;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net.Repository.Hierarchy;
using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace LD_PaymentEngine.APIServices
{
    public class LoanPaymentIntegrationAPIServices
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        RestManager restManager;
        WebElementExtensionsPage webElementExtensions = null;
        public LoanPaymentIntegrationAPIServices(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
        }
        public string PrepareJsonInputForPost(string loanNumber, LoanPaymentTypeEnum paymentFrequency, string method,
        string payloadName, List<Hashtable> loanLevelData = null)
        {
            string filepath = string.Empty;
            string inputJson = string.Empty;
            string strjson = string.Empty;
            string workingDirectory = Environment.CurrentDirectory;
            AddPaymentsToLoanApiRequest _paymentData = new AddPaymentsToLoanApiRequest();
            AddHelocDrawToLoanApiRequest _drawReqData = new AddHelocDrawToLoanApiRequest();
            AddKyribaResponseApiRequest _kyribaResponse = new AddKyribaResponseApiRequest();
            AddPaymentsProfileToBorrowerApiRequest _profileData = new AddPaymentsProfileToBorrowerApiRequest();
            AddSendLetterToLoanApiRequest _letterData = new AddSendLetterToLoanApiRequest();
            AddSentEmailToLoanApiRequest _emailData = new AddSentEmailToLoanApiRequest();
            AddTran73RecordApiRequest _tran73Record = new AddTran73RecordApiRequest();
            try
            {
                filepath = Directory.GetParent(workingDirectory).ToString().Substring(0, workingDirectory.IndexOf("\\bin")) + @"\TestData\PaymentInput.json";
                inputJson = JsonUtils.GetJsonStringFromFile(filepath);
                var resultJsonDataValue = JArray.Parse(inputJson)
                    .DescendantsAndSelf()
                    .OfType<JProperty>()
                    .Single(x => x.Name.Equals(payloadName))
                    .Value;
                inputJson = resultJsonDataValue.ToString();
                switch (payloadName)
                {
                    case ("payment"):
                        _paymentData = AddPaymentsRequest(inputJson, paymentFrequency, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_paymentData);
                        break;
                    case ("paymentprofile"):
                        _profileData = AddPaymentProfile(inputJson);
                        strjson = JsonUtils.GetJsonStringFromObject(_profileData);
                        break;
                    case ("helocdraw"):
                        _drawReqData = AddHelocDraw(inputJson);
                        strjson = JsonUtils.GetJsonStringFromObject(_drawReqData);
                        break;
                    case ("kyribaresponse"):
                        _kyribaResponse = JsonUtils.GetObjectFromJsonString<AddKyribaResponseApiRequest>(inputJson);
                        strjson = JsonUtils.GetJsonStringFromObject(_kyribaResponse);
                        break;
                    case ("sendletter"):
                        _letterData = AddSendLetter(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_letterData);
                        break;
                    case ("sendEmail"):
                        _emailData = AddSentEmail(inputJson, paymentFrequency, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_emailData);
                        break;
                    case ("tran73record"):
                        _tran73Record = AddTran73Record(inputJson, paymentFrequency, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_tran73Record);
                        break;
                    case ("get"):
                        break;
                    case ("delete"):
                        break;
                    default:
                        break;
                }
                strjson = strjson.Replace("+00:00", "");

                return strjson;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while Preparing Json input" + ex.Message);
            }
            return strjson;
        }

        public string PrepareJsonInputForPut(string loanNumber, string payloadName, List<Hashtable> loanLevelData = null)
        {
            string filepath = string.Empty;
            string inputJson = string.Empty;
            string strjson = string.Empty;
            string workingDirectory = Environment.CurrentDirectory;
            var _helocData = new UpdateHelocAutopayPaymentApiRequest();
            var _setupData = new UpdatePaymentSetupApiRequest();
            var _drawData = new UpdateLoanInDrawApiRequest();
            var _recurringData = new UpdateLoanRecurringPendingPaymentStatusInLoanApiRequest();
            var _pstatusData = new UpdateLoanPaymentStatusInLoanApiRequest();
            var _profileData = new UpdatePaymentsProfileFromBorrowerApiRequest();
            var _loanData = new UpdateLoanInPaymentApiRequest();
            var _paymentData = new UpdatePaymentInLoanApiRequest();
            var _deletedData = new DeletePaymentsFromLoanApiRequest();
            var _deleteProfile = new DeletePaymentsProfileFromBorrowerApiRequest();
            var _paymentsetup = new UpdatePaymentSetupApiRequest();
            try
            {
                filepath = Directory.GetParent(workingDirectory).ToString().Substring(0, workingDirectory.IndexOf("\\bin")) + @"\TestData\PaymentInput.json";
                inputJson = JsonUtils.GetJsonStringFromFile(filepath);
                var resultJsonDataValue = JArray.Parse(inputJson)
                    .DescendantsAndSelf()
                    .OfType<JProperty>()
                    .Single(x => x.Name.Equals(payloadName))
                    .Value;
                inputJson = resultJsonDataValue.ToString();
                switch (payloadName)
                {
                    case ("putpaymentprofile"):
                        _profileData = AssignDataForPaymentProfile(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_profileData);
                        break;
                    case ("helocautopay"):
                        _helocData = AssignDataForUpdateHelocAutopay(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_helocData);
                        break;
                    case ("puthelocdraw"):
                        _drawData = AssignDataForUpdateHelocDraw(inputJson);
                        strjson = JsonUtils.GetJsonStringFromObject(_drawData);
                        break;
                    case ("putupdateloan"):
                        _loanData = AssignDataForUpdatePaymentSpecificDetails(inputJson);
                        strjson = JsonUtils.GetJsonStringFromObject(_loanData);
                        break;
                    case ("putloanpaymentstatus"):
                        _pstatusData = AssignDataForLoanPaymentStatus(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_paymentData);
                        break;
                    case ("recurringpendingstatus"):
                        _recurringData = AssignDataForLoanRecurringPendingPaymentStatus(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_recurringData);
                        break;
                    case ("putupdatepayment"):
                        _paymentData = AssignDataForUpdatePayment(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_paymentData);
                        break;
                    case ("paymentsetup"):
                        _setupData = AssignDataForPaymentSetup(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_setupData);
                        break;
                    case ("deletepayment"):
                        _deletedData = AssignDataForDeletePayment(inputJson, loanLevelData);
                        strjson = JsonUtils.GetJsonStringFromObject(_deletedData);
                        break;
                    case ("deleteprofile"):
                        _deleteProfile = AssignDataForDeleteProfile(inputJson);
                        strjson = JsonUtils.GetJsonStringFromObject(_deleteProfile);
                        break;
                    default:
                        break;
                }
                strjson = strjson.Replace("+00:00", "");

                return strjson;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while Preparing Json input" + ex.Message);
            }
            return strjson;
        }


        #region"       POST METHODS     "

        public AddPaymentsToLoanApiRequest AddPaymentsRequest(string inputJson, LoanPaymentTypeEnum paymentFrequency, List<Hashtable> loanLevelData = null)
        {
            AddPaymentsToLoanApiRequest paymentData = new AddPaymentsToLoanApiRequest();
            try
            {
                paymentData = JsonUtils.GetObjectFromJsonString<AddPaymentsToLoanApiRequest>(inputJson);

                TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(DateTime.Now.ToShortDateString()), pacificZone);

                paymentData.PaymentDate = dt;
                paymentData.TotalDraftAmount = Convert.ToDecimal(loanLevelData[0][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString());
                paymentData.ChannelId = ChannelEnum._6;
                paymentData.MonthlyPaymentAmount = Convert.ToDecimal(loanLevelData[0][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString());
                paymentData.AuthorizedBy = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerFullName].ToString();
                paymentData.PaymentFrequency = paymentFrequency;
                paymentData.IsPaymentInSuspense = false;

                pacificZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                dt = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(dt.Year, dt.Month, 1), pacificZone);

                PaymentDue due = new PaymentDue();
                due.DueDate = dt;
                due.TotalMonthlyPayment = Convert.ToDecimal(loanLevelData[0][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString());
                due.PrincipalAndInterest = Convert.ToDecimal(loanLevelData[0][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString());
                due.TaxAndInsurance = Convert.ToDecimal(loanLevelData[0][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString());
                List<PaymentDue> due1 = new List<PaymentDue>();
                due1.Add(due);
                paymentData.PaymentDue = due1;
                Default df = new Default();
                df.AdditionalRepaymentPlanAmount = 0;
                df.DefaultProgram = DefaultProgram._1;
                paymentData.DefaultPayment = df;
                paymentData.BankName = Constants.BankAccountData.BankName; ;
                paymentData.MaskedBankAccountNumber = Constants.BankAccountData.MaskedBankAccountNumber;
                paymentData.RoutingNumber = Constants.BankAccountData.RoutingNumber;
                paymentData.LoanType = LoanType._0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return paymentData;

        }

        public AddHelocDrawToLoanApiRequest AddHelocDraw(string inputJson)
        {
            string barrowerId = string.Empty;
            string profileId = string.Empty;
            barrowerId = PaymentEngineConstants.BorrowerId;
            AddHelocDrawToLoanApiRequest _drawReqData = new AddHelocDrawToLoanApiRequest();
            try
            {
                _drawReqData = JsonUtils.GetObjectFromJsonString<AddHelocDrawToLoanApiRequest>(inputJson);
                profileId = GetPaymentProfile(barrowerId);
                Guid prId = new Guid(profileId);
                _drawReqData.PaymentProfileId = prId;

                TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(DateTime.Now.ToShortDateString()), pacificZone);
                _drawReqData.TransactionStartDate = dt;
                _drawReqData.TransactionEndDate = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _drawReqData;
        }

        public AddPaymentsProfileToBorrowerApiRequest AddPaymentProfile(string inputJson)
        {
            AddPaymentsProfileToBorrowerApiRequest _profileData = new AddPaymentsProfileToBorrowerApiRequest();
            try
            {
                _profileData = JsonUtils.GetObjectFromJsonString<AddPaymentsProfileToBorrowerApiRequest>(inputJson);
                _profileData.ProfileName = "TestAutoProfile" + DateTime.Now.ToString("MMddyyHHmm");
                _profileData.ProfileType = PaymentProfileTypeEnum._0;
                _profileData.DefaultPaymentProfile = true;
                PaymentProfileDetails details = new PaymentProfileDetails();
                details.FirstName = PaymentEngineConstants.FirstName;
                details.LastName = PaymentEngineConstants.LastName;
                details.AccountProfile = AccountProfileEnum._0;
                details.AccountType = AccountTypeEnum._1;
                details.RoutingNumber = PaymentEngineConstants.RoutingNumber;
                webElementExtensions = new WebElementExtensionsPage(_driver, test);
                details.AccountNumber = PaymentEngineConstants.AccountNumnber + webElementExtensions.RandomNumberGenerator(_driver, 0000, 9999);
                _profileData.PaymentProfileDetails = details;
                _profileData.ChannelId = ChannelEnum._1;
                _profileData.SaveAccountInformation = true;
                _profileData.UserId = PaymentEngineConstants.BorrowerId;
                _profileData.ValidateAccountInformation = true;
                _profileData.LoanType = LoanType._0;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding payment profile input" + ex.Message);
            }
            return _profileData;
        }

        public AddSendLetterToLoanApiRequest AddSendLetter(string inputJson, List<Hashtable> loanLevelData = null)
        {
            string loanNumber = string.Empty;
            AddSendLetterToLoanApiRequest _letterData = new AddSendLetterToLoanApiRequest();
            try
            {
                _letterData = JsonUtils.GetObjectFromJsonString<AddSendLetterToLoanApiRequest>(inputJson);
                _letterData.LetterID = LetterIDEnum._13;
                _letterData.BorrowerFullName = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerFullName].ToString();
                _letterData.CoBorrowerFullName = loanLevelData[0][Constants.LoanLevelDataColumns.CoBorrowerFullName].ToString();
                _letterData.BillingAddress = loanLevelData[0][Constants.LoanLevelDataColumns.BillingAddress].ToString();
                _letterData.BillingCity = loanLevelData[0][Constants.LoanLevelDataColumns.BillingCity].ToString();
                _letterData.BillingState = loanLevelData[0][Constants.LoanLevelDataColumns.BillingState].ToString();
                _letterData.BillingZip = loanLevelData[0][Constants.LoanLevelDataColumns.BillingZip].ToString();
                _letterData.PropertyAddress = loanLevelData[0][Constants.LoanLevelDataColumns.PropertyAddress].ToString();
                _letterData.PropertyCity = loanLevelData[0][Constants.LoanLevelDataColumns.PropertyCity].ToString();
                _letterData.PropertyState = loanLevelData[0][Constants.LoanLevelDataColumns.PropertyState].ToString();
                _letterData.PropertyZip = loanLevelData[0][Constants.LoanLevelDataColumns.PropertyZip].ToString();
                _letterData.PmtDate = Convert.ToDateTime(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate]);
                _letterData.PmtAmount = Convert.ToDecimal(loanLevelData[0][Constants.PaymentSetupDataColumns.TotalDraftAmount].ToString());
                _letterData.PmtTrackingNumber = loanLevelData[0][Constants.PaymentSetupDataColumns.ConfirmationNumber].ToString();
                _letterData.PmtScheduledAt = Convert.ToDateTime(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate].ToString());
                _letterData.AuthorizationMethod = "Scheduled with an agent";
                _letterData.DayBeforeProcessingDate = Convert.ToDateTime(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate]).AddDays(-1);
                _letterData.Description = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerFullName].ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding payment profile input" + ex.Message);
            }
            return _letterData;
        }

        public AddSentEmailToLoanApiRequest AddSentEmail(string inputJson, LoanPaymentTypeEnum paymentFrequency, List<Hashtable> loanLevelData = null)
        {
            string loanNumber = string.Empty;
            AddSentEmailToLoanApiRequest _emailData = new AddSentEmailToLoanApiRequest();
            try
            {
                _emailData = JsonUtils.GetObjectFromJsonString<AddSentEmailToLoanApiRequest>(inputJson);
                _emailData.LetterID = LetterIDEnum._13;
                _emailData.PaymentFrequency = paymentFrequency;
                if (loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate] != null)
                {
                    _emailData.DateSend = Convert.ToDateTime(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate]);
                }
                else
                {
                    _emailData.DateSend = Convert.ToDateTime(DateTime.Now.ToString());
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding send email input" + ex.Message);
            }
            return _emailData;
        }

        public AddTran73RecordApiRequest AddTran73Record(string inputJson, LoanPaymentTypeEnum paymentFrequency, List<Hashtable> loanLevelData = null)
        {
            string loanNumber = string.Empty;
            AddTran73RecordApiRequest _tran73Data = new AddTran73RecordApiRequest();
            try
            {
                _tran73Data = JsonUtils.GetObjectFromJsonString<AddTran73RecordApiRequest>(inputJson);
                _tran73Data.TransactionDate = Convert.ToDateTime(DateTime.Now.ToString());
                _tran73Data.PaymentAmount = Convert.ToDecimal(loanLevelData[0][Constants.LoanLevelDataColumns.TotalMonthlyPayment]);
                _tran73Data.SuspenseAmount = Convert.ToDecimal(loanLevelData[0][Constants.LoanLevelDataColumns.SuspenseBalance]);
                _tran73Data.NextPaymentDueDate = Convert.ToDateTime(loanLevelData[0][Constants.LoanLevelDataColumns.NextPaymentDueDate]);
                _tran73Data.ProcessStopCode = loanLevelData[0][Constants.LoanLevelDataColumns.ProcessStopCode].ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding tran73 record " + ex.Message);
            }
            return _tran73Data;
        }


        #endregion

        #region "       PUT METHODS      "

        public UpdatePaymentsProfileFromBorrowerApiRequest AssignDataForPaymentProfile(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdatePaymentsProfileFromBorrowerApiRequest _profileData = new UpdatePaymentsProfileFromBorrowerApiRequest();
            try
            {

                _profileData = JsonUtils.GetObjectFromJsonString<UpdatePaymentsProfileFromBorrowerApiRequest>(inputJson);
                _profileData.UserId = loanLevelData[0][Constants.PaymentSetupDataColumns.UserId].ToString();
                _profileData.coBorrowerId = loanLevelData[0][Constants.PaymentSetupDataColumns.UserId].ToString();
                _profileData.ChannelId = Convert.ToInt32(ChannelEnum._1);
                _profileData.AccountType = AccountTypeEnum._0;
                _profileData.RoutingNumber = Constants.BankAccountData.RoutingNumber;
                _profileData.AccountNumber = Constants.BankAccountData.AccountNumber;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _profileData;



        }

        public UpdateHelocAutopayPaymentApiRequest AssignDataForUpdateHelocAutopay(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdateHelocAutopayPaymentApiRequest _helocData = new UpdateHelocAutopayPaymentApiRequest();
            try
            {

                _helocData = JsonUtils.GetObjectFromJsonString<UpdateHelocAutopayPaymentApiRequest>(inputJson);
                _helocData.LoanNumber = Convert.ToInt64(loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber]);
                _helocData.TotalDraftAmount = Convert.ToDecimal(loanLevelData[0][Constants.PaymentSetupDataColumns.TotalDraftAmount]) + 100;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _helocData;



        }

        public UpdateLoanInDrawApiRequest AssignDataForUpdateHelocDraw(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdateLoanInDrawApiRequest _drawData = new UpdateLoanInDrawApiRequest();
            try
            {

                _drawData = JsonUtils.GetObjectFromJsonString<UpdateLoanInDrawApiRequest>(inputJson);
                _drawData.TransactionEndDate = DateTime.Now;
                _drawData.LastUpdatedBy = "PE Automation";
                _drawData.DrawStatusId = DrawStatusEnum._0;
                _drawData.FileProcessingStatusId = FileProcessingStatus._0;
                _drawData.KyribaFileName = "TestFile_" + DateTime.Now;
                _drawData.KyribaTransactionNumber = "123456789";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _drawData;



        }


        public UpdateLoanInPaymentApiRequest AssignDataForUpdatePaymentSpecificDetails(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdateLoanInPaymentApiRequest _paymentData = new UpdateLoanInPaymentApiRequest();
            try
            {
                _paymentData = JsonUtils.GetObjectFromJsonString<UpdateLoanInPaymentApiRequest>(inputJson);
                _paymentData.LoanNumber = Convert.ToInt64(loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber]);
                _paymentData.PaymentId = Guid.Parse(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString());
                _paymentData.PaymentDate = Convert.ToDateTime(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate]);
                _paymentData.PaymentStatus = (LoanPaymentStatusEnum)Enum.Parse(typeof(LoanPaymentStatusEnum), loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentStatus].ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _paymentData;
        }

        public UpdatePaymentSetupApiRequest AssignDataForPaymentSetup(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdatePaymentSetupApiRequest _setupData = new UpdatePaymentSetupApiRequest();
            try
            {
                _setupData = JsonUtils.GetObjectFromJsonString<UpdatePaymentSetupApiRequest>(inputJson);
                _setupData.LoanNumber = Convert.ToInt64(loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber]);
                _setupData.PaymentId = Guid.Parse(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString());
                _setupData.SendToMSP = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _setupData;
        }

        public UpdateLoanRecurringPendingPaymentStatusInLoanApiRequest AssignDataForLoanRecurringPendingPaymentStatus(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdateLoanRecurringPendingPaymentStatusInLoanApiRequest _recurringData = new UpdateLoanRecurringPendingPaymentStatusInLoanApiRequest();
            try
            {
                _recurringData = JsonUtils.GetObjectFromJsonString<UpdateLoanRecurringPendingPaymentStatusInLoanApiRequest>(inputJson);
                _recurringData.PaymentId = Guid.Parse(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString());
                _recurringData.PaymentStatus = LoanPaymentStatusEnum._3;
                _recurringData.IsPaymentInSuspense = false;
                _recurringData.ProcessStopCode = "0";
                _recurringData.PifStopCode = "0";
                _recurringData.ForeclosureStopCode = "0";
                _recurringData.BadCheckStopCode = "0";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _recurringData;



        }

        public UpdateLoanPaymentStatusInLoanApiRequest AssignDataForLoanPaymentStatus(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdateLoanPaymentStatusInLoanApiRequest _paymentData = new UpdateLoanPaymentStatusInLoanApiRequest();
            try
            {
                _paymentData = JsonUtils.GetObjectFromJsonString<UpdateLoanPaymentStatusInLoanApiRequest>(inputJson);
                _paymentData.PaymentId = Guid.Parse(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString());
                _paymentData.PaymentStatus = LoanPaymentStatusEnum._3;
                _paymentData.IsPaymentInSuspense = false;
                _paymentData.ProcessStopCode = "0";
                _paymentData.PifStopCode = "0";
                _paymentData.ForeclosureStopCode = "0";
                _paymentData.BadCheckStopCode = "0";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _paymentData;



        }

        public UpdatePaymentInLoanApiRequest AssignDataForUpdatePayment(string inputJson, List<Hashtable> loanLevelData = null)
        {
            UpdatePaymentInLoanApiRequest _paymentData = new UpdatePaymentInLoanApiRequest();
            try
            {
                _paymentData = JsonUtils.GetObjectFromJsonString<UpdatePaymentInLoanApiRequest>(inputJson);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while preparing Json input" + ex.Message);
            }
            return _paymentData;
        }
        #endregion

        #region"        GET METHODS      "

        public string GetPaymentProfile(string barrowerId)
        {
            string urlEndPoint = string.Empty;
            string profileId = string.Empty;

            RestManager restManager;
            try
            {
                string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);
                urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetPaymentProfilesAssociatedToBarrower.Replace("{BarrowerId}", barrowerId);
                JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
                var response = JsonUtils.GetDynamicFromJsonString(json.ToString());

                foreach (var detail in response.details)
                {
                    string strtemp = detail.ToString();
                    if (strtemp.Contains("profileId") && strtemp.Contains("profileName"))
                    {
                        string[] results = strtemp.Split(',');
                        profileId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                    }
                    if (webElementExtensions.IsValidGuid(profileId)) break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting payment profile" + ex.Message);
            }
            return profileId;

        }

        public string GetPayments(string loanNumber)
        {
            string urlEndPoint = string.Empty;
            string paymentId = string.Empty;

            RestManager restManager;
            try
            {
                string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetPaymentsAssociatedToLoanNumber.Replace("{LoanNumber}", loanNumber);
                JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
                string response = json.ToString();

                if (response.Contains("paymentId") && response.Contains("loanNumber") && response.Contains("confirmationNumber"))
                {
                    string[] results = response.Split(',');
                    paymentId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                }
                webElementExtensions.IsValidGuid(paymentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting payment associated with loan number " + ex.Message);
            }
            return paymentId;

        }

        public string GetSpecPaymentDetails(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string paymentId = string.Empty;
            bool IsGuid = false;
            RestManager restManager;
            try
            {
                paymentId = loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString();

                string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetPaymentDetailsByPaymentId.Replace("{LoanNumber}", loanNumber).Replace("{PaymentId}", paymentId);
                JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
                string response = json.ToString();

                if (response.Contains("paymentId") && response.Contains("loanNumber") && response.Contains("confirmationNumber"))
                {
                    string[] results = response.Split(',');
                    paymentId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                }
                webElementExtensions.IsValidGuid(paymentId);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting payment associated with loan number " + ex.Message);
            }
            return paymentId;

        }

        public string GetDateWiseRecurringPayments(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string paymentDate = string.Empty;
            string paymentId = string.Empty;
            string itemSize = "10";
            RestManager restManager;
            try
            {
                DateTime pDate = Convert.ToDateTime(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate]);
                paymentDate = pDate.ToString("yyyy-MM-dd");

                string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetDateWiseRecurringPendingPayments.Replace("{PaymentDate}", paymentDate)
                    .Replace("{Itemsize}", itemSize);
                JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
                string response = json.ToString();

                if (response.Contains("paymentId") && response.Contains("loanNumber") && response.Contains("confirmationNumber"))
                {
                    string[] results = response.Split(',');
                    paymentId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                }
                webElementExtensions.IsValidGuid(paymentId);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting payment associated with loan number " + ex.Message);
            }
            return paymentId;

        }

        public string GetInactivePayments(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string paymentId = string.Empty;
            RestManager restManager;
            try
            {
                string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetInactivePaymentsAssociatedToLoanNumber.Replace("{LoanNumber}", loanNumber);
                JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
                string response = json.ToString();

                if (response.Contains("paymentId") && response.Contains("loanNumber") && response.Contains("confirmationNumber"))
                {
                    string[] results = response.Split(',');
                    paymentId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                }
                webElementExtensions.IsValidGuid(paymentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting payment associated with loan number " + ex.Message);
            }
            return paymentId;

        }

        public string GetPostedPayments(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string paymentId = string.Empty;
            string paymentDate = string.Empty;
            RestManager restManager;
            try
            {
                DateTime pDate = Convert.ToDateTime(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentDate]);
                paymentDate = pDate.ToString("yyyy-MM-dd");

                string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetMspPostedPaymentsForGivenDate.Replace("{PaymentDate}", paymentDate);
                JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
                string response = json.ToString();

                if (response.Contains("paymentId") && response.Contains("loanNumber") && response.Contains("confirmationNumber"))
                {
                    string[] results = response.Split(',');
                    paymentId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                }
                webElementExtensions.IsValidGuid(paymentId);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting payment associated with loan number " + ex.Message);
            }
            return paymentId;

        }

        public string GetStatusWisePayments(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string paymentId = string.Empty;
            string paymentStatus = LoanPaymentStatusEnum._3.ToString();
            RestManager restManager;
            try
            {
                string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetPaymentStatusWisePaymentDetails.Replace("{LoanNumber}", loanNumber)
                 .Replace("{paymentStatus}", paymentStatus);
                JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
                string response = json.ToString();

                if (response.Contains("paymentId") && response.Contains("loanNumber") && response.Contains("confirmationNumber"))
                {
                    string[] results = response.Split(',');
                    paymentId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                }

                webElementExtensions.IsValidGuid(paymentId);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting payment associated with loan number " + ex.Message);
            }
            return paymentId;

        }

        #endregion

        #region "       DELETE METHODS      "
        public DeletePaymentsFromLoanApiRequest AssignDataForDeletePayment(string inputJson, List<Hashtable> loanLevelData = null)
        {
            string loanNumber = string.Empty;
            DeletePaymentsFromLoanApiRequest _deleteData = new DeletePaymentsFromLoanApiRequest();
            try
            {
                _deleteData = JsonUtils.GetObjectFromJsonString<DeletePaymentsFromLoanApiRequest>(inputJson);
                _deleteData.ChannelId = ChannelEnum._1;
                _deleteData.UserId = loanLevelData[0][Constants.PaymentSetupDataColumns.UserId].ToString();
                _deleteData.AuthorizedBy = loanLevelData[0][Constants.PaymentSetupDataColumns.AuthorizedBy].ToString();
                _deleteData.ProcessStopCode = loanLevelData[0][Constants.LoanLevelDataColumns.ProcessStopCode].ToString();
                _deleteData.BadCheckStopCode = loanLevelData[0][Constants.LoanLevelDataColumns.BadCheckStopCode].ToString();
                _deleteData.PifStopCode = loanLevelData[0][Constants.LoanLevelDataColumns.PifStopCode].ToString();
                _deleteData.ForeclosureStopCode = loanLevelData[0][Constants.LoanLevelDataColumns.ForeclosureStopCode].ToString();
                _deleteData.DeletedReason = "Deleted by PE Automation";
                _deleteData.UserName = loanLevelData[0][Constants.PaymentSetupDataColumns.UserName].ToString();
                _deleteData.LoanPaymentStatus = LoanPaymentStatusEnum._2;
                _deleteData.LoanNumber = Convert.ToInt64(loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString());
                _deleteData.PaymentsId = Guid.Parse(loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString());


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding tran73 record " + ex.Message);
            }
            return _deleteData;
        }

        public DeletePaymentsProfileFromBorrowerApiRequest AssignDataForDeleteProfile(string inputJson)
        {
            string loanNumber = string.Empty;
            DeletePaymentsProfileFromBorrowerApiRequest _deleteData = new DeletePaymentsProfileFromBorrowerApiRequest();
            try
            {
                _deleteData = JsonUtils.GetObjectFromJsonString<DeletePaymentsProfileFromBorrowerApiRequest>(inputJson);
                _deleteData.ChannelId = ChannelEnum._1;
                _deleteData.UserId = PaymentEngineConstants.BorrowerId;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding tran73 record " + ex.Message);
            }
            return _deleteData;
        }
        #endregion

        #region"       Common Methods       "


        public string SubmitHelocDraw(string loanNumber)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PostHelocDrawAssociatedToLoanNumber.Replace("{LoanNumber}", loanNumber);
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                jsonInput = PrepareJsonInputForPost(loanNumber, LoanPaymentTypeEnum._0, APIMethods.POST, PaymentEngineConstants.HelocDraw);
                JObject json = JObject.Parse(restManager.PostMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting heloc draw  " + ex.Message);
            }
            return response;
        }

        public string SubmitKyribaResponse()
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string loanNumber = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                jsonInput = PrepareJsonInputForPost(loanNumber, LoanPaymentTypeEnum._0, APIMethods.POST, PaymentEngineConstants.KyribaResponse);
                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PostKyribaResponseRequest;
                JObject json = JObject.Parse(restManager.PostMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting heloc draw  " + ex.Message);
            }
            return response;
        }

        public string SubmitPaymentProfile(string barrowerId)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PostPaymentProfilesAssociatedToBarrower.Replace("{BarrowerId}", barrowerId);
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                jsonInput = PrepareJsonInputForPost(barrowerId, LoanPaymentTypeEnum._0, APIMethods.POST, PaymentEngineConstants.PaymentProfile);
                JObject json = JObject.Parse(restManager.PostMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting payment profile " + ex.Message);
            }
            return response;
        }

        public string SubmitSendLetter(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PostSendLetterRequest.Replace("{LoanNumber}", loanNumber);

                jsonInput = PrepareJsonInputForPost(loanNumber, LoanPaymentTypeEnum._0, APIMethods.POST, PaymentEngineConstants.SendLetter, loanLevelData);
                JObject json = JObject.Parse(restManager.PostMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string SubmitSendEmail(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PostSendEmailRequest.Replace("{LoanNumber}", loanNumber);

                jsonInput = PrepareJsonInputForPost(loanNumber, LoanPaymentTypeEnum._0, APIMethods.POST, PaymentEngineConstants.SendEmail, loanLevelData);
                JObject json = JObject.Parse(restManager.PostMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string SubmitTrans73Record(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PostTran73RecordRequest;

                jsonInput = PrepareJsonInputForPost(loanNumber, LoanPaymentTypeEnum._0, APIMethods.POST, PaymentEngineConstants.Tran73Record, loanLevelData);
                JObject json = JObject.Parse(restManager.PostMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string SubmitPayment(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PostPaymentsAssociatedToLoanNumber.Replace("{LoanNumber}", loanNumber);

                jsonInput = PrepareJsonInputForPost(loanNumber, LoanPaymentTypeEnum._0, APIMethods.POST, PaymentEngineConstants.Payment, loanLevelData);
                JObject json = JObject.Parse(restManager.PostMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }


        public string DeletePaymentProfile(string barrowerId)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            string profileId = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                profileId = GetPaymentProfile(barrowerId);
                urlEndPoint = APIConstants.PaymentEngineResourcePaths.DeletePaymentProfileByProfileId.Replace("{BarrowerId}", barrowerId)
                   .Replace("{PaymentProfileId}", profileId);

                jsonInput = PrepareJsonInputForPut(barrowerId, PaymentEngineConstants.DeleteProfile, null);
                JObject json = JObject.Parse(restManager.DeleteMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting payment profile " + ex.Message);
            }
            return response;
        }

        public string DeletePayment(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            string paymentId = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                paymentId = loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString();
                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.DeletePayment, loanLevelData);
                urlEndPoint = APIConstants.PaymentEngineResourcePaths.DeletePaymentByPaymentId.Replace("{LoanNumber}", loanNumber).Replace("{PaymentId}", paymentId);
                JObject json = JObject.Parse(restManager.DeleteMethod(urlEndPoint, null, null, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting payment profile " + ex.Message);
            }
            return response;
        }

        public string PutPaymentSetup(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutPaymentSetup;

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.PaymentSetup, loanLevelData);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string UpdateLoanRecurringPendingPaymentStatus(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutLoanRecurringPendingPaymentStatus.Replace("{LoanNumber}", loanNumber);

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.LoanRecurringStatus, loanLevelData);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string UpdateLoanPaymentStatus(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutLoanPaymentStatusForLoanNumber.Replace("{LoanNumber}", loanNumber);

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.LoanPaymentStatus, loanLevelData);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string UpdateHelocAutopayPayment(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutHelocAutopayPayment;

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.HelocAutopay, loanLevelData);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string UpdatePaymentProfile(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutPaymentProfile;

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.PutPaymentProfile, loanLevelData);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string UpdateHelocDrawRequest(string loanNumber, string drawRequestId)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            try
            {
                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutDrawRequestAssociatedToLoanNumber.Replace("{DrawRequestId}", drawRequestId).Replace("{LoanNumber}", loanNumber);

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.PutHelocDraw, null);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while submitting Letter  " + ex.Message);
            }
            return response;
        }

        public string UpdateLoanPayment(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            string paymentId = string.Empty;
            try
            {
                paymentId = loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString();

                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutPaymentDetailsToLoanNumber.Replace("{PaymentId}", paymentId).Replace("{LoanNumber}", loanNumber); ;

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.PutUpdateLoan, loanLevelData);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while update loan payment details  " + ex.Message);
            }
            return response;
        }

        public string UpdatePaymentDetails(string loanNumber, List<Hashtable> loanLevelData = null)
        {
            string urlEndPoint = string.Empty;
            string jsonInput = string.Empty;
            string paymentEngineBaseURI = string.Empty;
            string response = string.Empty;
            string paymentId = string.Empty;
            try
            {
                paymentId = loanLevelData[0][Constants.PaymentSetupDataColumns.PaymentId].ToString();

                paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                restManager = new RestManager(paymentEngineBaseURI, test);

                urlEndPoint = APIConstants.PaymentEngineResourcePaths.PutPaymentAssociatedToLoanNumber.Replace("{LoanNumber}", loanNumber).Replace("PaymentId", paymentId);

                jsonInput = PrepareJsonInputForPut(loanNumber, PaymentEngineConstants.PutUpdatePayment, loanLevelData);
                JObject json = JObject.Parse(restManager.PutMethod(urlEndPoint, null, jsonInput, false, "Basic"));
                response = json.ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while update loan payment details  " + ex.Message);
            }
            return response;
        }

        #endregion

        #region"        Validation Methods      "

        public bool ValidatePaymentProfileResponse(string response)
        {
            bool isProfileCreated = false;
            try
            {
                if (response.IndexOf("paymentProfileId") > 0)
                {
                    string[] results = response.Split(',');
                    string profileid = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "");
                    string status = results[1].Substring(results[1].ToString().LastIndexOf(':') + 1).Replace("\"", "");
                    if (profileid != null && profileid != string.Empty && status.Contains("VALID")) isProfileCreated = true;
                }
            }
            catch (Exception ex)
            {
                isProfileCreated = false;
                Console.WriteLine("Error while validating payment profile response " + ex.Message);
            }
            return isProfileCreated;
        }

        public bool ValidateHelocDrawResponse(string response)
        {
            string referenceNumber = string.Empty;
            bool isSuccess = false;
            try
            {
                var json = JsonUtils.GetDynamicFromJsonString(response);
                foreach (var detail in json.details)
                {
                    string strtemp = detail.ToString();
                    if (strtemp.Contains("referenceNumber"))
                    {
                        string[] results = strtemp.Split(',');
                        referenceNumber = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                        isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Console.WriteLine("Error while validating payment profile response " + ex.Message);
            }
            return isSuccess;
        }

        public bool ValidateDeletePaymentProfileResponse(string response)
        {
            string referenceNumber = string.Empty;
            bool isSuccess = false;
            try
            {
                var json = JsonUtils.GetDynamicFromJsonString(response);

                if (json.details != null)
                {
                    foreach (var detail in json.details)
                    {
                        string strtemp = Convert.ToString(detail);
                        if (strtemp.Contains("Deleted Bank Information Successfully"))
                            isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Console.WriteLine("Error while validating delete payment profile response " + ex.Message);
            }
            return isSuccess;
        }

        public bool ValidateDeletePaymentResponse(string response)
        {
            string referenceNumber = string.Empty;
            bool isSuccess = false;
            try
            {
                var json = JsonUtils.GetDynamicFromJsonString(response);

                if (json.details != null)
                {
                    foreach (var detail in json.details)
                    {
                        string strtemp = Convert.ToString(detail);
                        if (strtemp.Contains("Deleted Bank Information Successfully"))
                            isSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Console.WriteLine("Error while validating delete payment profile response " + ex.Message);
            }
            return isSuccess;
        }

        public bool ValidatePaymentSetupResponse(string response)
        {
            bool isProfileCreated = false;
            try
            {
                if (response.IndexOf("details") > 0)
                {
                    string[] results = response.Split(',');

                    //if (profileid != null && profileid != string.Empty && status.Contains("VALID")) isProfileCreated = true;
                }
            }
            catch (Exception ex)
            {
                isProfileCreated = false;
                Console.WriteLine("Error while validating payment profile response " + ex.Message);
            }
            return isProfileCreated;
        }

        public bool ValidatePaymentResponse(string response)
        {
            string paymentId = string.Empty;
            bool isSuccess = false;
            try
            {
                if (response.Contains("paymentId") && response.Contains("loanNumber") && response.Contains("confirmationNumber"))
                {
                    string[] results = response.Split(',');
                    paymentId = results[0].Substring(results[0].ToString().LastIndexOf(':') + 1).Replace("\"", "").Trim();
                    isSuccess = true;
                }
                webElementExtensions.IsValidGuid(paymentId);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Console.WriteLine("Error while validating payment profile response " + ex.Message);
            }
            return isSuccess;
        }

        #endregion
    }
}

