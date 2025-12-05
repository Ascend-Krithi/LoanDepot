using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using LD_AutomationFramework.Config;
using System.Collections;
using System.Reflection;
using LD_AutomationFramework.Utilities;
using LD_PaymentEngine.APIServices;
using Newtonsoft.Json;
using AventStack.ExtentReports;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace LD_PaymentEngine.APITestScripts
{
    [TestClass]
    public class LoanPaymentIntegrationAPITests : BasePage
    {
        #region  Queries        

        public static string getLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetLoanHavingPayments"));
        public static string getHelocAutopayDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetHelocAutopayPayments"));
        public static string getHelocLoansDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetHelocLoans"));
        public static string getFMLoansDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetFMLoans"));
        public static string getPaymentsDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetPayments"));
        public static string getOneTimePaymentsDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetPayments_OTP"));
        public static string getRecurringPaymentsDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetRecurringPayments"));
        public static string getSuspenseBalanceDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetSuspenseBalanceLoans"));
        public static string getInactivePaymentsDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetInactivePayments"));
        public static string getPostedPaymentsDataQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, "xml/Query_GetPostedPayments"));


        #endregion

        #region TestClassInitialization    

        LoanPaymentIntegrationAPIServices loanPaymentIntegrationAPIServices;
        RestManager restManager;
        List<Hashtable> loanLevelData = null;
        CommonServicesPage commonServices = null;
        DBconnect dBconnect = null;
        static List<string> columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();

        #endregion TestClassInitialization

        public TestContext TestContext
        {
            get;
            set;
        }

        [TestInitialize]
        public void PrepareAPI_TestCases()
        {
            InitializeFramework(TestContext, true);
            restManager = new RestManager(string.Empty, string.Empty, test, APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower()));
            commonServices = new CommonServicesPage(_driver, test);
            dBconnect = new DBconnect(test, "MelloServETL");
            loanPaymentIntegrationAPIServices = new LoanPaymentIntegrationAPIServices(_driver, test);
        }

        #region Get Methods

        [TestMethod]
        [Description("<br> Verify get payments for loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetPaymentsAssociatedToLoanNumber()
        {
            string loanNumber = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getOneTimePaymentsDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();
            
            string paymentId = loanPaymentIntegrationAPIServices.GetPayments(loanNumber);
            ReportingMethods.LogAssertionTrue(test, !string.IsNullOrEmpty(paymentId), "Unable to get the payment Id from Get Payments method");
            

        }

        [TestMethod]
        [Description("<br> Verify get specific payment details for loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetSpecificPaymentDetailsAssociatedToLoanNumber()
        {
            string loanNumber = string.Empty;
            string paymentId = string.Empty;
            string urlEndPoint = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getFMLoansDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();
            
            paymentId = loanPaymentIntegrationAPIServices.GetSpecPaymentDetails(loanNumber, loanLevelData);
            ReportingMethods.LogAssertionTrue(test, !string.IsNullOrEmpty(paymentId), "Unable to get the payment Id from Get Specific Payment details");

        }

        [TestMethod]
        [Description("<br> Verify get payment profile for barrower Id <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetPaymentProfilesAssociatedToBarrower()
        {
            string barrowerId = string.Empty;
            barrowerId = APIConstants.PaymentEngineConstants.BorrowerId;
            
            string profileId = loanPaymentIntegrationAPIServices.GetPaymentProfile(barrowerId);
            ReportingMethods.LogAssertionTrue(test, !string.IsNullOrEmpty(profileId), "Unable to get the profie Id from Get Profile method");

        }

        [TestMethod]
        [Description("<br> Verify get date wise recurring pending payments for the given date <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetDateWiseRecurringPendingPaymentDetails()
        {
            string loanNumber = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentDate };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getRecurringPaymentsDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();
            
            string paymentId = loanPaymentIntegrationAPIServices.GetDateWiseRecurringPayments(loanNumber,loanLevelData);
            ReportingMethods.LogAssertionTrue(test, !string.IsNullOrEmpty(paymentId), "Unable to get the payment Id from Get Payments method");
        }

        [TestMethod]
        [Description("<br> Verify get inactive payment details associated to loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetInactivePaymentDetails()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getInactivePaymentsDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            string paymentId = loanPaymentIntegrationAPIServices.GetInactivePayments(loanNumber, loanLevelData);
            ReportingMethods.LogAssertionTrue(test, !string.IsNullOrEmpty(paymentId), "Unable to get the payment Id from Get Inactive Payments method");
        }

        [TestMethod]
        [Description("<br> Verify get msp posted payments for the given date <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetMspPostedPayments()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getInactivePaymentsDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();
            
            
            string paymentId = loanPaymentIntegrationAPIServices.GetPostedPayments(loanNumber, loanLevelData);
            ReportingMethods.LogAssertionTrue(test, !string.IsNullOrEmpty(paymentId), "Unable to get the payment details from Get MSP Payments method");
        }

        [TestMethod]
        [Description("<br> Verify get returned payments for the given date <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetReturnedPaymentsForGivenDate()
        {
            string urlEndPoint = string.Empty;
            string paymentDate = "2025-09-06";

            string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
            restManager = new RestManager(paymentEngineBaseURI, test);

            urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetRejectedPaymentsForGivenDate.Replace("{PaymentDate}", paymentDate);

            JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
        }

        [TestMethod]
        [Description("<br> Verify get rejected payments for the given date <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetRejectedPaymentsForGivenDate()
        {
            string urlEndPoint = string.Empty;
            string paymentDate = "2025-02-12";
            string LoanNumber = "1038488654";
            string isAutopay = "false";
            string isCancelled = "false";

            string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
            restManager = new RestManager(paymentEngineBaseURI, test);

            urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetRejectedPaymentsForGivenDate.Replace("{PaymentDate}", paymentDate)
                .Replace("{LoanNumber", LoanNumber).Replace("{isAutopay}", isAutopay).Replace("isCancelled", isCancelled);

            JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
        }

        [TestMethod]
        [Description("<br> Verify get pending payments count associated to loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetPendingPaymentsCountAssociatedToLoanNumber()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getLoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            string paymentEngineBaseURI = APIConstants.PaymentEngineURIs.PaymentEngineBaseURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
            restManager = new RestManager(paymentEngineBaseURI, test);

            urlEndPoint = APIConstants.PaymentEngineResourcePaths.GetPaymentCountAssociatedToLoanNumber.Replace("{LoanNumber}", loanNumber);
            JObject json = JObject.Parse(restManager.GetMethod(urlEndPoint, null, false, "Basic"));
        }

        [TestMethod]
        [Description("<br> Verify get pending payments count associated to loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void GetPaymentStatusWisePaymentDetails()
        {
            string loanNumber = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getPaymentsDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            string paymentId = loanPaymentIntegrationAPIServices.GetStatusWisePayments(loanNumber);
            ReportingMethods.LogAssertionTrue(test, !string.IsNullOrEmpty(paymentId), "Unable to get the payment details from Get MSP Payments method");
            
        }

        #endregion

        #region Post Methods 


        [TestMethod]
        [Description("<br> Verify submit heloc draw for loan number <br> ")]
        [TestCategory("PE_API_Regression"), TestCategory("PE_API_POST")]
        public void SubmitHelocDrawRequest()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getHelocLoansDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.SubmitHelocDraw(loanNumber);
            ReportingMethods.LogAssertionTrue(test, loanPaymentIntegrationAPIServices.ValidateHelocDrawResponse(response), "Validation failed in heloc draw");
        }

        [TestMethod]
        [Description("<br> Verify submit reqeust for saving kyriba response <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitKyribaResponse()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;

            
            response = loanPaymentIntegrationAPIServices.SubmitKyribaResponse();
            ReportingMethods.LogAssertionTrue(test, response.Contains(" \"details\": true"), "Validation failed in kyriba response");

        }

        [TestMethod]
        [Description("<br> Verify submit payment profile request for barrowerId <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitPaymentProfileRequest()
        {
            string response = string.Empty;
            string barrowerId = APIConstants.PaymentEngineConstants.BorrowerId.ToString();

            
            response = loanPaymentIntegrationAPIServices.SubmitPaymentProfile(barrowerId);
            ReportingMethods.LogAssertionTrue(test, loanPaymentIntegrationAPIServices.ValidatePaymentProfileResponse(response), "Validation failed in payment profile");
        }

        [TestMethod]
        [Description("<br> validate submit send letter <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitSendLetter()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.BorrowerFullName, Constants.LoanLevelDataColumns.CoBorrowerFullName,
            Constants.LoanLevelDataColumns.BillingAddress, Constants.LoanLevelDataColumns.BillingCity, Constants.LoanLevelDataColumns.BillingState,Constants.LoanLevelDataColumns.BillingZip,
            Constants.LoanLevelDataColumns.PropertyAddress,Constants.LoanLevelDataColumns.PropertyCity, Constants.LoanLevelDataColumns.PropertyState,Constants.LoanLevelDataColumns.PropertyZip,
            Constants.PaymentSetupDataColumns.PaymentDate,Constants.PaymentSetupDataColumns.TotalDraftAmount,Constants.PaymentSetupDataColumns.ConfirmationNumber,};
            loanLevelData = commonServices.GetLoanDataFromDatabase(getFMLoansDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.SubmitSendLetter(loanNumber, loanLevelData);
            ReportingMethods.LogAssertionTrue(test, response.Contains(" \"details\": true"), "Validation failed in submit sending letter");

        }


        [TestMethod]
        [Description("<br> validate submit send email <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitSendEmail()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getFMLoansDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.SubmitSendEmail(loanNumber, loanLevelData);
            ReportingMethods.LogAssertionTrue(test, response.Contains(" \"details\": true"), "Validation failed in Sending Email");

        }

        [TestMethod]
        [Description("<br> validate submit tran73 record <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitTrans73Record()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.ProcessStopCode,
            Constants.LoanLevelDataColumns.NextPaymentDueDate,Constants.PaymentSetupDataColumns.PaymentDate,
            Constants.LoanLevelDataColumns.TotalMonthlyPayment,Constants.LoanLevelDataColumns.SuspenseBalance};
            loanLevelData = commonServices.GetLoanDataFromDatabase(getSuspenseBalanceDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.SubmitTrans73Record(loanNumber, loanLevelData);

            ReportingMethods.LogAssertionTrue(test, response.Contains(" \"details\": true"), "Validation failed in Sending Email");
        }

        [TestMethod]
        [Description("<br> Verify submit one time payment for loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitOneTimePaymentToLoanNumber()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.TotalMonthlyPayment,
                Constants.LoanLevelDataColumns.PrincipalInterestAmount, Constants.LoanLevelDataColumns.TaxAndInsuranceAmount, Constants.LoanLevelDataColumns.BorrowerFullName, };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getLoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.SubmitPayment(loanNumber, loanLevelData);

            ReportingMethods.LogAssertionTrue(test, loanPaymentIntegrationAPIServices.ValidatePaymentResponse(response), "Validation failed in payment");

            


        }

        [TestMethod]
        [Description("<br> Verify submit monthly payment for loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitMonthlyPaymentToLoanNumber()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.TotalMonthlyPayment,
                Constants.LoanLevelDataColumns.PrincipalInterestAmount, Constants.LoanLevelDataColumns.TaxAndInsuranceAmount, Constants.LoanLevelDataColumns.BorrowerFullName };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getLoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.SubmitPayment(loanNumber, loanLevelData);

            ReportingMethods.LogAssertionTrue(test, loanPaymentIntegrationAPIServices.ValidatePaymentResponse(response), "Validation failed in payment");


        }

        [TestMethod]
        [Description("<br> Verify submit biweekly payment for loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void SubmitBiweeklyPaymentToLoanNumber()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.LoanLevelDataColumns.TotalMonthlyPayment,
                Constants.LoanLevelDataColumns.PrincipalInterestAmount, Constants.LoanLevelDataColumns.TaxAndInsuranceAmount, Constants.LoanLevelDataColumns.BorrowerFullName };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getLoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();


            
            response = loanPaymentIntegrationAPIServices.SubmitPayment(loanNumber, loanLevelData);

            ReportingMethods.LogAssertionTrue(test, loanPaymentIntegrationAPIServices.ValidatePaymentResponse(response), "Validation failed in payment");


        }

        #endregion

        #region Delete Methods

        [TestMethod]
        [Description("<br> Verify delete payment for loan number <br> ")]
        [TestCategory("PE_API_Regression")]
        public void DeletePaymentAssociatedToLoanNumberAndPaymentId()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            string urlEndPoint = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId,
                Constants.PaymentSetupDataColumns.UserId,Constants.PaymentSetupDataColumns.AuthorizedBy,Constants.LoanLevelDataColumns.ProcessStopCode,
                Constants.PaymentSetupDataColumns.UserName,Constants.LoanLevelDataColumns.PifStopCode,Constants.LoanLevelDataColumns.ForeclosureStopCode,
                Constants.LoanLevelDataColumns.BadCheckStopCode};
            loanLevelData = commonServices.GetLoanDataFromDatabase(getOneTimePaymentsDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.DeletePayment(loanNumber, loanLevelData);

            Assert.IsTrue(loanPaymentIntegrationAPIServices.ValidateDeletePaymentResponse(response), "Validation failed in delete payment response");


        }

        [TestMethod]
        [Description("<br> Verify delete payment profile associated with barrower <br> ")]
        [TestCategory("PE_API_Regression")]
        public void DeleteProfileAssociatedToBarrowerId()
        {
            string response = string.Empty;
            string barrowerId = APIConstants.PaymentEngineConstants.BorrowerId.ToString();

            
            response = loanPaymentIntegrationAPIServices.DeletePaymentProfile(barrowerId);

            Assert.IsTrue(loanPaymentIntegrationAPIServices.ValidateDeletePaymentProfileResponse(response), "Validation failed in delete payment profile");

        }

        #endregion

        #region Put Methods


        [TestMethod]
        [Description("<br> Put - Update Payment Profile <br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdatePaymentProfile()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId,
                                    Constants.PaymentSetupDataColumns.TotalDraftAmount, Constants.PaymentSetupDataColumns.MonthlyPaymentAmount };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getHelocAutopayDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.UpdatePaymentProfile(loanNumber, loanLevelData);

        }


        [TestMethod]
        [Description("<br> Put - Update Heloc Autopay Payment <br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdateHelocAutopayPayment()
        {
            string loanNumber = string.Empty;
            string urlEndPoint = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId,
                                    Constants.PaymentSetupDataColumns.TotalDraftAmount, Constants.PaymentSetupDataColumns.MonthlyPaymentAmount };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getHelocAutopayDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.UpdateHelocAutopayPayment(loanNumber, loanLevelData);

        }

        [TestMethod]
        [Description("<br> Put - Update Heloc Draw <br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdateHelocDrawRequestAssociatedToLoanNumber()
        {
            string loanNumber = string.Empty;
            string drawRequestId = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getHelocAutopayDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.UpdateHelocDrawRequest(loanNumber, drawRequestId);
        }

        [TestMethod]
        [Description("<br> Put - Update Loan Payment Details By PaymentId<br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdateLoanPaymentDetailsByPaymentId()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getLoanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.UpdateLoanPayment(loanNumber, loanLevelData);

        }


        [TestMethod]
        [Description("<br> Put - Update Loan Payment Status <br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdateLoanPaymentStatus()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId,
                                    Constants.PaymentSetupDataColumns.TotalDraftAmount, Constants.PaymentSetupDataColumns.MonthlyPaymentAmount };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getHelocAutopayDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.UpdateLoanPaymentStatus(loanNumber, loanLevelData);

        }


        [TestMethod]
        [Description("<br> Put - Update Loan Recurring Pending Payment Status <br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdateLoanRecurringPendingPaymentStatus()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getHelocAutopayDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.UpdateLoanRecurringPendingPaymentStatus(loanNumber, loanLevelData);

        }

        [TestMethod]
        [Description("<br> Put - Update Loan Recurring Pending Payment Status <br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdateLoanPaymentDetails()
        {
            string loanNumber = string.Empty;
            string paymentId = string.Empty;
            string urlEndPoint = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId,
                                    Constants.PaymentSetupDataColumns.TotalDraftAmount, Constants.PaymentSetupDataColumns.MonthlyPaymentAmount };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getHelocAutopayDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();



        }

        [TestMethod]
        [Description("<br> Put - Update Loan Recurring Pending Payment Status <br> ")]
        [TestCategory("PE_API_Regression")]
        public void UpdateLoanPaymentSetupParameters()
        {
            string loanNumber = string.Empty;
            string response = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber, Constants.PaymentSetupDataColumns.PaymentId };
            loanLevelData = commonServices.GetLoanDataFromDatabase(getPaymentsDataQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();

            
            response = loanPaymentIntegrationAPIServices.PutPaymentSetup(loanNumber, loanLevelData);

            Assert.IsTrue(loanPaymentIntegrationAPIServices.ValidatePaymentSetupResponse(response), "Validation failed in payment setup");

        }

        #endregion

    }
}
