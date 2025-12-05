using System;
using System.Collections.Generic;
using System.Linq;

namespace LD_AutomationFramework
{
    public class APIConstants
    {

        #region AgentPortal
        public struct AgentPortalURIs
        {
            public const string ServicingLINCPortalURI = "https://app-servicinglincportalborrowers-azusw2-dvo-ENVIRONMENT1.azurewebsites.net/api/v1/ServicingLINCPortal/";

            public const string HelocURI = "https://s-az-mi-ENVIRONMENT1-01.ld.corp.local:8266/api/v1/MelloServPaymentIntegration/";
        }
        public struct AgentPortalResourcePaths
        {
            public const string BorrowerIVRDetailsPath = "Borrower/BorrowerIVRDetails";

            public const string HelocLoanInfoPath = "HelocLoanInfo";
        }

        public struct AgentPortalHeadersName
        {
            public const string loanNumber = "loanNumber";

            public const string encryptionValue = "encryptionValue";

            public const string ChannelId = "Channel-Id";

            public const string LoanConsumerType = "Loan-Consumer-Type";
        }

        public struct AgentPortalHeadersValue
        {
            public const string ChannelId = "1";

            public const string LoanConsumerType = "4";
        }

        public class HelocLoanInfo
        {
            public double? DelinquentPaymentBalance { get; set; }
            public double? TotalDue { get; set; }
            public List<Fees> Fees { get; set; }
            public List<FeesBreakdown> FeeBreakdown { get; set; }
            public List<PaymentsDue> PaymentsDue { get; set; }
            public double? NextPaymentDue { get; set; }
            public bool IsBillGenerated { get; set; }
            public double? AnnualInterestRate { get; set; }
            public double? AvailableHELOCAmount { get; set; }
            public double? CurrentBalance { get; set; }
            public double? HelocLimitAmount { get; set; }
            public double? YtdInterestPaid { get; set; }
            public double? LastPaymentAmount { get; set; }
            public double? NetDue { get; set; }
            public DateTime? lastPaymentDate { get; set; }
            public DateTime? AsOfDate { get; set; }
            public DateTime? LateChargeStartDate { get; set; }
            public List<EligibilityCheck> EligibilityCheck { get; set; }
            public bool? IsDrawEligible =>
                EligibilityCheck?
                    .FirstOrDefault(e => e.Type == "Draw")?
                    .Eligibility?
                    .IsEligible;
            public string LoanNumber { get; set; }
        }

        public class Fees
        {
            public int? Type { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public double? Amount { get; set; }
            public List<Fees> Breakdown { get; set; }
        }

        public class FeesBreakdown
        {
            public string PaymentDueDate { get; set; }
            public List<Fees> Fees { get; set; }
        }
        public class PaymentsDue
        {
            public DateTime? DueDate { get; set; }
            public double? TotalMonthlyPayment { get; set; }
            public double? PrincipalAndInterest { get; set; }
            public double? TaxAndInsurance { get; set; }
            public bool? IsSuspense { get; set; }
        }

        #endregion AgentPortal

        #region CustomerPortal
        public class EligibilityCheck
        {
            public string Type { get; set; }
            public Eligibility Eligibility { get; set; }
        }
        public class Eligibility
        {
            public bool IsEligible { get; set; }
            public List<EligibilityError> Errors { get; set; }
        }
        public class EligibilityError
        {
            public string ExceptionType { get; set; }
            public string ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
            public string AdditionalMessage { get; set; }
        }
        #endregion CustomerPortal

        #region PaymentEngine
        public struct PaymentEngineURIs
        {
            public const string PaymentEngineBaseURI = "https://payment-engine-integration.ENVIRONMENT1.loandepotdev.io/";
        }

        public struct PaymentEngineResourcePaths
        {
            public const string GetPaymentsAssociatedToLoanNumber = "LoanPaymentIntegrationService/Loan/{LoanNumber}/Payments";
            public const string PostPaymentsAssociatedToLoanNumber = "LoanPaymentIntegrationService/Loan/{LoanNumber}/Payments";
            public const string PostHelocDrawAssociatedToLoanNumber = "LoanPaymentIntegrationService/Loan/{LoanNumber}/HelocDraw";
            public const string PostKyribaResponseRequest = "LoanPaymentIntegrationService/KyribaResponse";
            public const string PostPaymentProfilesAssociatedToBarrower = "LoanPaymentIntegrationService/Borrower/{BarrowerId}/PaymentsProfile";
            public const string PostSendLetterRequest = "LoanPaymentIntegrationService/Loan/{LoanNumber}/SendLetter";
            public const string PostSendEmailRequest = "LoanPaymentIntegrationService/Loan/{LoanNumber}/SentEmail";
            public const string PostTran73RecordRequest = "LoanPaymentIntegrationService/Tran73Record";
            public const string GetPaymentProfilesAssociatedToBarrower = "LoanPaymentIntegrationService/Borrower/{BarrowerId}/PaymentsProfile";
            public const string DeletePaymentByPaymentId = "LoanPaymentIntegrationService/Loan/{LoanNumber}/Payments/{PaymentId}";
            public const string DeletePaymentProfileByProfileId = "LoanPaymentIntegrationService/Borrower/{BarrowerId}/PaymentsProfile/{PaymentProfileId}";
            public const string GetMspPostedPaymentsForGivenDate = "LoanPaymentIntegrationService/MSPPostedPayment?PaymentDate={PaymentDate}";
            public const string GetReturnedPaymentsForGivenDate = "LoanPaymentIntegrationService/ReturnedPayment?PaymentDate={PaymentDate}";
            public const string GetRejectedPaymentsForGivenDate = "LoanPaymentIntegrationService/RejectedPayment?PaymentDate={PaymentDate}&LoanNumber={LoanNumber}&IsAutoPay={isAutopay}&IsCancelled={isCancelled}";
            public const string GetDateWiseRecurringPendingPayments = "LoanPaymentIntegrationService/DateWiseRecurringPendingPaymentDetails?PaymentDate={PaymentDate}&ItemSize={Itemsize}";
            public const string GetInactivePaymentsAssociatedToLoanNumber = "LoanPaymentIntegrationService/InactivePaymentDetails?LoanNumber={LoanNumber}";
            public const string GetPaymentDetailsByPaymentId = "LoanPaymentIntegrationService/LoanNumber/{LoanNumber}/Payment/{PaymentId}";
            public const string GetPaymentCountAssociatedToLoanNumber = "LoanPaymentIntegrationService/Loan/{LoanNumber}/PaymentsCount";
            public const string GetPaymentStatusWisePaymentDetails = "LoanPaymentIntegrationService/PaymentStatusWisePaymentDetails?PaymentStatus={paymentStatus}&LoanNumber={LoanNumber}";
            public const string PutHelocAutopayPayment = "LoanPaymentIntegrationService/HelocAutopayPayment";
            public const string PutPaymentProfile = "LoanPaymentIntegrationService/Borrower/{BarrowerId}/PaymentsProfile/{profileId}";
            public const string PutDrawRequestAssociatedToLoanNumber = "LoanPaymentIntegrationService/Draw/{DrawRequestId}/Loan/{LoanNumber}";
            public const string PutPaymentDetailsToLoanNumber = "LoanPaymentIntegrationService/Payment/{PaymentId}/Loan/{LoanNumber}";
            public const string PutLoanPaymentStatusForLoanNumber = "LoanPaymentIntegrationService/Loan/{LoanNumber}/LoanPaymentStatus";
            public const string PutLoanRecurringPendingPaymentStatus = "LoanPaymentIntegrationService/Loan/{LoanNumber}/LoanRecurringPendingPaymentStatus";
            public const string PutPaymentAssociatedToLoanNumber = "LoanPaymentIntegrationService/Loan/{LoanNumber}/Payment/{PaymentId}";
            public const string PutPaymentSetup = "LoanPaymentIntegrationService/PaymentSetup";


        }

        public struct PaymentEngineConstants
        {
            public const string BorrowerId = "7B39B90943CF13C3680D7BFC039ECD3A";
            public const string RoutingNumber = "122199983";
            public const string AccountNumnber = "6123123";
            public const string FirstName = "test_firstname";
            public const string LastName = "test_lastname";
            public const string Payment = "payment";
            public const string PaymentProfile = "paymentprofile";
            public const string HelocDraw = "helocdraw";
            public const string DeleteProfile = "deleteprofile";
            public const string DeletePayment = "deletepayment";
            public const string KyribaResponse = "kyribaresponse";
            public const string SendLetter = "sendletter";
            public const string SendEmail = "sendEmail";
            public const string PaymentSetup = "paymentsetup";
            public const string LoanRecurringStatus = "recurringpendingstatus";
            public const string LoanPaymentStatus = "putloanpaymentstatus";
            public const string HelocAutopay = "helocautopay";
            public const string PutPaymentProfile = "putpaymentprofile";
            public const string PutHelocDraw = "puthelocdraw";
            public const string PutUpdateLoan = "putupdateloan";
            public const string PutUpdatePayment = "putupdatepayment";
            public const string Tran73Record = "tran73record";
        }
        #endregion PaymentEngine
        public struct APIMethods
        {
            public const string POST = "POST";
            public const string GET = "GET";
            public const string DELETE = "DELETE";
            public const string PUT = "PUT";
        }

        #region CustomerPortal
        public struct CustomerPortalURIs
        {
            public const string EncryptionURI = "https://app-servicinglincportalborrowers-azusw2-dvo-{ENVIRONMENT}1.azurewebsites.net";
            public const string ServisBotURI = "https://ldapi-{ENVIRONMENT}1.loandepotdev.works/ServisBotApi";
        }

        public struct CustomerPortalResourcePaths
        {
            public const string GET_EncryptionKey = "/api/v1/ServicingLINCPortal/Borrower/BorrowerIVRDetails/{loan_number}";

            public const string POST_GetLoan = "/api/v1/Servisbot/GetLoan/{loan_number}";
        }

        public struct ServisBotSubscriptions
        {
            public const string SubscriptionKey = "dcf4c565cf9e4c9ab01fc6311509942f";
        }


        #endregion CustomerPortal



    }
}
