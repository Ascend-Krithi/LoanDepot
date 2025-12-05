using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.APIServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LD_CustomerPortal.APITestScripts
{
    [TestClass]
    public class ServisBotApiTests : BasePage
    {
        #region  Queries        

        public static string getEligibleLoansQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.ServisBotEligibleLoans));


        #endregion Queries

        #region TestClassInitialization    

        ServisBotApi servisBotApi;
        List<Hashtable> loanLevelData = null;
        List<string> usedLoanTestData = new List<string>();
        DBconnect dBconnect = null;
        CommonServicesPage commonServices = null;
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
            dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
            commonServices = new CommonServicesPage(_driver, test);
            servisBotApi = new ServisBotApi();
        }


        [TestMethod]
        [Description("<br> Verify get Loan details  for loan number <br> " +
                     "TPR 3208 Verify POST GetLoan<LoanNumber> API")]
        [TestCategory("ServisBot_API_Regression")]
        public void TPR_3208_TPR_VerifyGetLoanDetails()
        {

            loanLevelData = commonServices.GetLoanDataFromDatabase(getEligibleLoansQuery, usedLoanTestData: usedLoanTestData);

            //Loan Details 
            var loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();
            var customerId = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerCustomerID].ToString();
            var escrowBalance = loanLevelData[0][Constants.LoanLevelDataColumns.EscrowBalance].ToString();
            var unpaidBalance = loanLevelData[0][Constants.LoanLevelDataColumns.FirstPrincipalBalance].ToString();
            var totalAmountDue = loanLevelData[0][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString();
            var otherFee = loanLevelData[0][Constants.LoanLevelDataColumns.OtherFees].ToString();
            var maturityDate = loanLevelData[0][Constants.LoanLevelDataColumns.MaturityDate].ToString();
            var pmiAmount = loanLevelData[0][Constants.LoanLevelDataColumns.MiMonthlyAmount].ToString();
            var investorName = loanLevelData[0][Constants.LoanLevelDataColumns.InvestorName].ToString();
            var escrowPaymentAmount = loanLevelData[0][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
            var isActive = loanLevelData[0][Constants.LoanLevelDataColumns.IsActive].ToString();
            var hasEscrowInsurance = loanLevelData[0][Constants.LoanLevelDataColumns.EscrowIndicator].ToString() == "Y" ? true : false;
            var closingDate = loanLevelData[0][Constants.LoanLevelDataColumns.OriginationDate].ToString();
            var loanTerm = loanLevelData[0][Constants.LoanLevelDataColumns.RemainingTerm].ToString();
            var escrowShortageAmount = loanLevelData[0][Constants.LoanLevelDataColumns.OverShortAmount].ToString();
            var originalAmount = loanLevelData[0][Constants.LoanLevelDataColumns.OriginalMortgageAmount].ToString();
            var PaymentDueDate = loanLevelData[0][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
            var interestRate = loanLevelData[0][Constants.LoanLevelDataColumns.InterestRates].ToString();
            var lateCharge = loanLevelData[0][Constants.LoanLevelDataColumns.AccruedLateChargeAmount].ToString();
            var principalIntAmount = loanLevelData[0][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString();
            var taxInsuranceAmount = loanLevelData[0][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();


            //Call the API
            var apiResponse = servisBotApi.GetLoanDetails(loanNumber, customerId);

            test.Log(Status.Info, $"API Respose :{JsonUtils.FormatJson(apiResponse)} ");

            test.Log(Status.Info, $"Validating API Response with Database Values");

            ReportingMethods.LogAssertionTrue(test, (bool)apiResponse.isActive == bool.Parse(isActive), $"Verify isActive, Expected  {isActive}, Actual {(bool)apiResponse.isActive}");
            ReportingMethods.LogAssertionTrue(test, (bool)apiResponse.hasEscrowInsurance == hasEscrowInsurance, $"Verify hasEscrowInsurance Expected {hasEscrowInsurance}, Actual {(bool)apiResponse.hasEscrowInsurance}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(escrowBalance) == (float)apiResponse.escrowBalance, $"Verify escrowBalance Expected {escrowBalance}, Actual {(float)apiResponse.escrowBalance}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(unpaidBalance) == float.Parse((string)apiResponse.unpaidBalance), $"Verify unpaidBalance Expected {unpaidBalance}, Actual {(string)apiResponse.unpaidBalance}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(totalAmountDue) + float.Parse(otherFee) == float.Parse((string)apiResponse.totalAmountDue), $"Verify amountDue Expected {float.Parse(totalAmountDue) + float.Parse(otherFee)}, Actual {(string)apiResponse.totalAmountDue}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(pmiAmount) == float.Parse((string)apiResponse.pmiAmount), $"Verify pmiAmount Expected {pmiAmount}, Actual {(string)apiResponse.pmiAmount}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(escrowPaymentAmount) == float.Parse((string)apiResponse.escrowPaymentAmount), $"Verify escrowPaymentAmount Expected {escrowPaymentAmount}, Actual {(string)apiResponse.escrowPaymentAmount}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(escrowShortageAmount) == float.Parse((string)apiResponse.escrowShortageAmount), $"Verify escrowShortageAmount Expected {escrowShortageAmount}, Actual {(string)apiResponse.escrowShortageAmount}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(originalAmount) == float.Parse((string)apiResponse.originalLoanAmount), $"Verify originalLoanAmount Expected {originalAmount}, Actual {(string)apiResponse.originalLoanAmount}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(interestRate) * 100 == float.Parse((string)apiResponse.interestRate), $"Verify interestRate Expected {float.Parse(interestRate) * 100}, Actual {(string)apiResponse.interestRate}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(lateCharge) == float.Parse((string)apiResponse.lateCharge), $"Verify lateCharge Expected {lateCharge}, Actual {(string)apiResponse.lateCharge}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(principalIntAmount) == float.Parse((string)apiResponse.principalInterestAmount), $"Verify principalIntAmount Expected {principalIntAmount}, Actual {(string)apiResponse.principalInterestAmount}");
            ReportingMethods.LogAssertionTrue(test, float.Parse(taxInsuranceAmount) == float.Parse((string)apiResponse.taxInsuranceAmount), $"Verify taxInsuranceAmount Expected {taxInsuranceAmount}, Actual {(string)apiResponse.taxInsuranceAmount}");

            ReportingMethods.LogAssertionEqual(test, loanNumber, (string)apiResponse.loanNumber, $"Verify loanNumber Expected {loanNumber}, Actual {(string)apiResponse.loanNumber}");
            ReportingMethods.LogAssertionEqual(test, investorName, (string)apiResponse.investorName, $"Verify investorName Expected {investorName}, Actual {(string)apiResponse.investorName}");

            ReportingMethods.LogAssertionTrue(test, DateTime.Parse(maturityDate).Date == DateTime.Parse((string)apiResponse.maturityDate).Date, $"Verify maturityDate Expected {maturityDate}, Actual {(string)apiResponse.maturityDate}");
            ReportingMethods.LogAssertionTrue(test, DateTime.Parse(closingDate).Date == DateTime.Parse((string)apiResponse.closingDate).Date, $"Verify closingDate Expected {closingDate}, Actual {(string)apiResponse.closingDate}");
            ReportingMethods.LogAssertionTrue(test, DateTime.Parse(PaymentDueDate).Date == DateTime.Parse((string)apiResponse.paymentDueDate).Date, $"Verify PaymentDueDate Expected {PaymentDueDate}, Actual {(string)apiResponse.paymentDueDate}");

            ReportingMethods.LogAssertionEqual(test, loanTerm, (string)apiResponse.loanTerm, "Verify loanTerm");

        }

    }
}
