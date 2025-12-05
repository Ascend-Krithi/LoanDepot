using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LD_AutomationFramework.APIConstants;

namespace LD_PaymentEngine.APIServices
{
    public partial class AddPaymentsToLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("paymentDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PaymentDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum PaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("totalDraftAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalDraftAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("monthlyPaymentAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal MonthlyPaymentAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentDue", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<PaymentDue> PaymentDue { get; set; }

        [Newtonsoft.Json.JsonProperty("fees", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Fees> Fees { get; set; }

        [Newtonsoft.Json.JsonProperty("delinquency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Delinquency Delinquency { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PaymentProfileId { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileDetails", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PaymentProfileDetails PaymentProfileDetails { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("additionalEscrow", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal AdditionalEscrow { get; set; }

        [Newtonsoft.Json.JsonProperty("additionalPrincipal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal AdditionalPrincipal { get; set; }

        [Newtonsoft.Json.JsonProperty("userEmail", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserEmail { get; set; }

        [Newtonsoft.Json.JsonProperty("authorizedBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AuthorizedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("badCheckStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BadCheckStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("pifStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PifStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("foreclosureStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ForeclosureStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("processStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProcessStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("isPaymentInSuspense", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsPaymentInSuspense { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentStartDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentStartDate { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentSetUpDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentSetUpDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentFrequency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentTypeEnum PaymentFrequency { get; set; }

        [Newtonsoft.Json.JsonProperty("sendToMSP", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SendToMSP { get; set; }

        [Newtonsoft.Json.JsonProperty("userName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [Newtonsoft.Json.JsonProperty("sendRecurringReminderEmail", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SendRecurringReminderEmail { get; set; }

        [Newtonsoft.Json.JsonProperty("maskedBankAccountNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string MaskedBankAccountNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("routingNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RoutingNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("bankName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BankName { get; set; }

        [Newtonsoft.Json.JsonProperty("createdByChannelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum CreatedByChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("nameOnBankAccount", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string NameOnBankAccount { get; set; }

        [Newtonsoft.Json.JsonProperty("typeOfAccount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AccountTypeEnum TypeOfAccount { get; set; }

        [Newtonsoft.Json.JsonProperty("nextPaymentDueDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? NextPaymentDueDate { get; set; }

        [Newtonsoft.Json.JsonProperty("lastEligibilityCheckDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? LastEligibilityCheckDate { get; set; }

        [Newtonsoft.Json.JsonProperty("defaultPayment", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Default DefaultPayment { get; set; }

        [Newtonsoft.Json.JsonProperty("suspenseAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal SuspenseAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("loanType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanType LoanType { get; set; }

    }

    public enum ChannelEnum
    {

        _0 = 0,

        _1 = 1,

        _2 = 2,

        _3 = 3,

        _4 = 4,

        _5 = 5,

        _6 = 6,

    }

    public enum LoanPaymentStatusEnum
    {

        _0 = 0,

        _1 = 1,

        _2 = 2,

        _3 = 3,

        _4 = 4,

        _5 = 5,

        _6 = 6,

        _7 = 7,

        _8 = 8,

    }

    public partial class PaymentDue
    {
        [Newtonsoft.Json.JsonProperty("dueDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset DueDate { get; set; }

        [Newtonsoft.Json.JsonProperty("totalMonthlyPayment", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalMonthlyPayment { get; set; }

        [Newtonsoft.Json.JsonProperty("principalAndInterest", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal PrincipalAndInterest { get; set; }

        [Newtonsoft.Json.JsonProperty("taxAndInsurance", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TaxAndInsurance { get; set; }

        [Newtonsoft.Json.JsonProperty("isSuspense", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsSuspense { get; set; }

    }

    public partial class PaymentProfileDetails
    {
        [Newtonsoft.Json.JsonProperty("firstName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [Newtonsoft.Json.JsonProperty("lastName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [Newtonsoft.Json.JsonProperty("businessAccountName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BusinessAccountName { get; set; }

        [Newtonsoft.Json.JsonProperty("accountProfile", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AccountProfileEnum AccountProfile { get; set; }

        [Newtonsoft.Json.JsonProperty("accountType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AccountTypeEnum AccountType { get; set; }

        [Newtonsoft.Json.JsonProperty("routingNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RoutingNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("accountNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AccountNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("bankName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BankName { get; set; }

    }

    public enum AccountProfileEnum
    {

        _0 = 0,

        _1 = 1,

    }

    public enum AccountTypeEnum
    {

        _0 = 0,

        _1 = 1,

    }

    public partial class Delinquency
    {
        [Newtonsoft.Json.JsonProperty("contactCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ContactCode { get; set; }

        [Newtonsoft.Json.JsonProperty("responseCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ResponseCode { get; set; }

        [Newtonsoft.Json.JsonProperty("reasonCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ReasonCode { get; set; }

        [Newtonsoft.Json.JsonProperty("forbearanceAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal ForbearanceAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("remindDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RemindDate { get; set; }

    }

    public enum LoanPaymentTypeEnum
    {

        _0 = 0,

        _1 = 1,

        _2 = 2,

        _3 = 3,

    }

    public enum LoanType
    {

        _0 = 0,

        _1 = 1,

    }

    public partial class Default
    {
        [Newtonsoft.Json.JsonProperty("additionalRepaymentPlanAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal AdditionalRepaymentPlanAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("defaultProgram", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DefaultProgram DefaultProgram { get; set; }

    }

    public enum DefaultProgram
    {

        _1 = 1,

        _2 = 2,

        _3 = 3,

    }

    public enum PaymentProfileTypeEnum
    {

        _0 = 0,

        _1 = 1,

    }

    public partial class PaymentSetupKeyValueApiRequest
    {
        [Newtonsoft.Json.JsonProperty("key", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Key { get; set; }

        [Newtonsoft.Json.JsonProperty("value", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Value { get; set; }

    }

    public partial class DeleteLoanStatisticsApiRequest
    {
        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long LoanNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PaymentDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentFrequency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentTypeEnum PaymentFrequency { get; set; }

    }

    public partial class DeletePaymentsDetailsApiResponse
    {
        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long LoanNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PaymentDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum PaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("totalDraftAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalDraftAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileDetails", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PaymentProfileDetails PaymentProfileDetails { get; set; }

        [Newtonsoft.Json.JsonProperty("confirmationNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ConfirmationNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("userEmail", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserEmail { get; set; }

        [Newtonsoft.Json.JsonProperty("authorizedBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AuthorizedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("createdAt", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset CreatedAt { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentStartDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentStartDate { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentSetUpDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentSetUpDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentFrequency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentTypeEnum PaymentFrequency { get; set; }

        [Newtonsoft.Json.JsonProperty("userName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [Newtonsoft.Json.JsonProperty("maskedBankAccountNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string MaskedBankAccountNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("routingNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RoutingNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("bankName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BankName { get; set; }

        [Newtonsoft.Json.JsonProperty("createdByChannelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum CreatedByChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("isDeleted", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsDeleted { get; set; }

        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

    }

    public partial class DeletePaymentsDetailsApiResponseServiceResponse
    {
        [Newtonsoft.Json.JsonProperty("outcomes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ServiceOutcomes Outcomes { get; set; }

        [Newtonsoft.Json.JsonProperty("details", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DeletePaymentsDetailsApiResponse Details { get; set; }

        [Newtonsoft.Json.JsonProperty("resultCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ResultCode { get; set; }

    }

    public partial class DeletePaymentsFromLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("badCheckStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BadCheckStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("pifStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PifStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("foreclosureStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ForeclosureStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("processStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProcessStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("authorizedBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AuthorizedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("deletedReason", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DeletedReason { get; set; }

        [Newtonsoft.Json.JsonProperty("userName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [Newtonsoft.Json.JsonProperty("loanPaymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum LoanPaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long LoanNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentsId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentsId { get; set; }

    }

    public partial class DeletePaymentsProfileApiResponse
    {
        [Newtonsoft.Json.JsonProperty("paymentProfileId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentProfileId { get; set; }

        [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

    }

    public partial class DeletePaymentsProfileApiResponseServiceResponse
    {
        [Newtonsoft.Json.JsonProperty("outcomes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ServiceOutcomes Outcomes { get; set; }

        [Newtonsoft.Json.JsonProperty("details", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DeletePaymentsProfileApiResponse Details { get; set; }

        [Newtonsoft.Json.JsonProperty("resultCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ResultCode { get; set; }

    }

    public partial class DeletePaymentsProfileFromBorrowerApiRequest
    {
        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

    }

    public partial class UpdateHelocAutopayPaymentApiRequest
    {
        [Newtonsoft.Json.JsonProperty("totalDraftAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalDraftAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("monthlyPaymentAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal MonthlyPaymentAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("fees", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Fees> Fees { get; set; }

        [Newtonsoft.Json.JsonProperty("sendToMSP", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SendToMSP { get; set; }

        [Newtonsoft.Json.JsonProperty("isBillGenerated", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool isBillGenerated { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long LoanNumber { get; set; }
    }

    public partial class UpdateLoanInDrawApiRequest
    {
        [Newtonsoft.Json.JsonProperty("transactionEndDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TransactionEndDate { get; set; }

        [Newtonsoft.Json.JsonProperty("lastUpdatedBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string LastUpdatedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("drawStatusId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DrawStatusEnum DrawStatusId { get; set; }

        [Newtonsoft.Json.JsonProperty("fileProcessingStatusId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public FileProcessingStatus FileProcessingStatusId { get; set; }

        [Newtonsoft.Json.JsonProperty("kyribaFileName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string KyribaFileName { get; set; }

        [Newtonsoft.Json.JsonProperty("kyribaTransactionNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string KyribaTransactionNumber { get; set; }

    }

    public partial class UpdateLoanInPaymentApiRequest
    {
        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long LoanNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PaymentDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum PaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("totalDraftAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalDraftAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("isDelayed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool isDelayed { get; set; }

    }

    public partial class UpdateLoanPaymentApiResponse
    {
        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long LoanNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PaymentDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum PaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("totalDraftAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalDraftAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileDetails", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PaymentProfileDetails PaymentProfileDetails { get; set; }

        [Newtonsoft.Json.JsonProperty("confirmationNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ConfirmationNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("userEmail", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserEmail { get; set; }

        [Newtonsoft.Json.JsonProperty("authorizedBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AuthorizedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("createdAt", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset CreatedAt { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentStartDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentStartDate { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentSetUpDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentSetUpDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentFrequency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentTypeEnum PaymentFrequency { get; set; }

        [Newtonsoft.Json.JsonProperty("userName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [Newtonsoft.Json.JsonProperty("maskedBankAccountNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string MaskedBankAccountNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("routingNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RoutingNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("bankName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BankName { get; set; }

        [Newtonsoft.Json.JsonProperty("createdByChannelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum CreatedByChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("updateDownStream", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool UpdateDownStream { get; set; }

        [Newtonsoft.Json.JsonProperty("previousPaymentDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? PreviousPaymentDate { get; set; }

    }

    public partial class UpdateLoanPaymentApiResponseServiceResponse
    {
        [Newtonsoft.Json.JsonProperty("outcomes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ServiceOutcomes Outcomes { get; set; }

        [Newtonsoft.Json.JsonProperty("details", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public UpdateLoanPaymentApiResponse Details { get; set; }

        [Newtonsoft.Json.JsonProperty("resultCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ResultCode { get; set; }

    }

    public partial class UpdateLoanPaymentStatusInLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum PaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("isPaymentInSuspense", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsPaymentInSuspense { get; set; }

        [Newtonsoft.Json.JsonProperty("badCheckStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BadCheckStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("pifStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PifStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("foreclosureStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ForeclosureStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("processStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProcessStopCode { get; set; }

    }

    public partial class UpdateLoanRecurringPendingPaymentStatusInLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum PaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("isPaymentInSuspense", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsPaymentInSuspense { get; set; }

        [Newtonsoft.Json.JsonProperty("badCheckStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BadCheckStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("pifStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PifStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("foreclosureStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ForeclosureStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("processStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProcessStopCode { get; set; }

    }

    public partial class UpdatePaymentInLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("paymentDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PaymentDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum PaymentStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("totalDraftAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalDraftAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("monthlyPaymentAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal MonthlyPaymentAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentDue", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<PaymentDue> PaymentDue { get; set; }

        [Newtonsoft.Json.JsonProperty("fees", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Fees> Fees { get; set; }

        [Newtonsoft.Json.JsonProperty("delinquency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Delinquency Delinquency { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PaymentProfileId { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileDetails", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PaymentProfileDetails PaymentProfileDetails { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("additionalEscrow", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal AdditionalEscrow { get; set; }

        [Newtonsoft.Json.JsonProperty("additionalPrincipal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal AdditionalPrincipal { get; set; }

        [Newtonsoft.Json.JsonProperty("userEmail", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserEmail { get; set; }

        [Newtonsoft.Json.JsonProperty("authorizedBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AuthorizedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("badCheckStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BadCheckStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("pifStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PifStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("foreclosureStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ForeclosureStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("processStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProcessStopCode { get; set; }

        [Newtonsoft.Json.JsonProperty("isPaymentInSuspense", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsPaymentInSuspense { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentStartDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentStartDate { get; set; }

        [Newtonsoft.Json.JsonProperty("recPaymentSetUpDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? RecPaymentSetUpDate { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentFrequency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentTypeEnum PaymentFrequency { get; set; }

        [Newtonsoft.Json.JsonProperty("sendToMSP", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SendToMSP { get; set; }

        [Newtonsoft.Json.JsonProperty("userName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [Newtonsoft.Json.JsonProperty("sendRecurringReminderEmail", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SendRecurringReminderEmail { get; set; }

        [Newtonsoft.Json.JsonProperty("maskedBankAccountNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string MaskedBankAccountNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("routingNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RoutingNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("bankName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BankName { get; set; }

        [Newtonsoft.Json.JsonProperty("createdByChannelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum CreatedByChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("nameOnBankAccount", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string NameOnBankAccount { get; set; }

        [Newtonsoft.Json.JsonProperty("typeOfAccount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AccountTypeEnum TypeOfAccount { get; set; }

        [Newtonsoft.Json.JsonProperty("nextPaymentDueDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? NextPaymentDueDate { get; set; }

        [Newtonsoft.Json.JsonProperty("lastEligibilityCheckDate", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? LastEligibilityCheckDate { get; set; }

        [Newtonsoft.Json.JsonProperty("defaultPayment", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Default DefaultPayment { get; set; }

        [Newtonsoft.Json.JsonProperty("suspenseAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal SuspenseAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("loanType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanType LoanType { get; set; }

        [Newtonsoft.Json.JsonProperty("updatePaymentMethod", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool UpdatePaymentMethod { get; set; }

    }

    public partial class UpdatePaymentSetupApiRequest
    {
        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long LoanNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("keyValueList", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<PaymentSetupKeyValueApiRequest> KeyValueList { get; set; }

        [Newtonsoft.Json.JsonProperty("sendToMSP", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SendToMSP { get; set; }

    }

    public partial class UpdatePaymentStatus
    {
        [Newtonsoft.Json.JsonProperty("paymentId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentId { get; set; }

        [Newtonsoft.Json.JsonProperty("isUpdateSuccess", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsUpdateSuccess { get; set; }

    }

    public partial class UpdatePaymentStatusServiceResponse
    {
        [Newtonsoft.Json.JsonProperty("outcomes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ServiceOutcomes Outcomes { get; set; }

        [Newtonsoft.Json.JsonProperty("details", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public UpdatePaymentStatus Details { get; set; }

        [Newtonsoft.Json.JsonProperty("resultCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ResultCode { get; set; }

    }

    public partial class UpdatePaymentsProfileFromBorrowerApiRequest
    {
        [Newtonsoft.Json.JsonProperty("coBorrowerId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string coBorrowerId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("accountType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AccountTypeEnum AccountType { get; set; }

        [Newtonsoft.Json.JsonProperty("routingNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RoutingNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("accountNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AccountNumber { get; set; }

    }

    public enum DrawStatusEnum
    {

        _0 = 0,

        _1 = 1,

        _2 = 2,

        _3 = 3,

    }

    public partial class Fees
    {
        [Newtonsoft.Json.JsonProperty("type", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Type { get; set; }

        [Newtonsoft.Json.JsonProperty("amount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal Amount { get; set; }

        [Newtonsoft.Json.JsonProperty("code", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Code { get; set; }

        [Newtonsoft.Json.JsonProperty("description", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty("breakdown", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<Fees> Breakdown { get; set; }

    }

    public enum FileProcessingStatus
    {

        _0 = 0,

        _1 = 1,

        _2 = 2,

        _3 = 3,

    }

    public partial class ServiceOutcomes
    {
        [Newtonsoft.Json.JsonProperty("errorCount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ErrorCount { get; set; }

        [Newtonsoft.Json.JsonProperty("messages", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<string> Messages { get; set; }

        [Newtonsoft.Json.JsonProperty("failedValidation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool FailedValidation { get; set; }

    }

    public partial class AddHelocDrawToLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("transactionAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TransactionAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("totalCreditLine", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal TotalCreditLine { get; set; }

        [Newtonsoft.Json.JsonProperty("currentBalance", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal CurrentBalance { get; set; }

        [Newtonsoft.Json.JsonProperty("transactionStartDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TransactionStartDate { get; set; }

        [Newtonsoft.Json.JsonProperty("transactionEndDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TransactionEndDate { get; set; }

        [Newtonsoft.Json.JsonProperty("lastUpdatedBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string LastUpdatedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("drawStatusId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public DrawStatusEnum DrawStatusId { get; set; }

        [Newtonsoft.Json.JsonProperty("createdBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("fileProcessingStatusId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public FileProcessingStatus FileProcessingStatusId { get; set; }

        [Newtonsoft.Json.JsonProperty("kyribaFileName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string KyribaFileName { get; set; }

        [Newtonsoft.Json.JsonProperty("kyribaTransactionNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string KyribaTransactionNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("memo", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Memo { get; set; }

        [Newtonsoft.Json.JsonProperty("payeeName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PayeeName { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Guid PaymentProfileId { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("inEligibleReason", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string InEligibleReason { get; set; }

        [Newtonsoft.Json.JsonProperty("requestorFirstName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestorFirstName { get; set; }

        [Newtonsoft.Json.JsonProperty("requestorLastName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestorLastName { get; set; }

        [Newtonsoft.Json.JsonProperty("requestorCustomerId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestorCustomerId { get; set; }

        [Newtonsoft.Json.JsonProperty("address", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Address { get; set; }

        [Newtonsoft.Json.JsonProperty("addressLine1", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AddressLine1 { get; set; }

        [Newtonsoft.Json.JsonProperty("addressCity", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AddressCity { get; set; }

        [Newtonsoft.Json.JsonProperty("addressState", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AddressState { get; set; }

        [Newtonsoft.Json.JsonProperty("addressZip", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AddressZip { get; set; }

    }

    
    public partial class AddKyribaResponseApiRequest
    {
        [Newtonsoft.Json.JsonProperty("responses", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<KyribaResponse> Responses { get; set; }

    }

    public partial class KyribaResponse
    {
        [Newtonsoft.Json.JsonProperty("transactionNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string TransactionNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("fedReference", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string FedReference { get; set; }

        [Newtonsoft.Json.JsonProperty("company", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Company { get; set; }

        [Newtonsoft.Json.JsonProperty("branch", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Branch { get; set; }

        [Newtonsoft.Json.JsonProperty("reason", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Reason { get; set; }

        [Newtonsoft.Json.JsonProperty("lastAcknowledgement", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string LastAcknowledgement { get; set; }

        [Newtonsoft.Json.JsonProperty("createdBy", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        [Newtonsoft.Json.JsonProperty("transactionDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TransactionDate { get; set; }

        [Newtonsoft.Json.JsonProperty("createdDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset CreatedDate { get; set; }

    }

    public partial class AddPaymentsProfileToBorrowerApiRequest
    {
        [Newtonsoft.Json.JsonProperty("profileName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProfileName { get; set; }

        [Newtonsoft.Json.JsonProperty("profileType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PaymentProfileTypeEnum ProfileType { get; set; }

        [Newtonsoft.Json.JsonProperty("defaultPaymentProfile", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool DefaultPaymentProfile { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentProfileDetails", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PaymentProfileDetails PaymentProfileDetails { get; set; }

        [Newtonsoft.Json.JsonProperty("saveAccountInformation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool SaveAccountInformation { get; set; }

        [Newtonsoft.Json.JsonProperty("channelId", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public ChannelEnum ChannelId { get; set; }

        [Newtonsoft.Json.JsonProperty("userId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [Newtonsoft.Json.JsonProperty("validateAccountInformation", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool ValidateAccountInformation { get; set; }

        [Newtonsoft.Json.JsonProperty("loanType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanType LoanType { get; set; }

    }

    public partial class AddSendLetterToLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("letterID", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LetterIDEnum LetterID { get; set; }

        [Newtonsoft.Json.JsonProperty("borrowerFullName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BorrowerFullName { get; set; }

        [Newtonsoft.Json.JsonProperty("coBorrowerFullName", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string CoBorrowerFullName { get; set; }

        [Newtonsoft.Json.JsonProperty("billingAddress", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BillingAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("billingCity", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BillingCity { get; set; }

        [Newtonsoft.Json.JsonProperty("billingState", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BillingState { get; set; }

        [Newtonsoft.Json.JsonProperty("billingZip", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BillingZip { get; set; }

        [Newtonsoft.Json.JsonProperty("propertyAddress", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PropertyAddress { get; set; }

        [Newtonsoft.Json.JsonProperty("propertyCity", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PropertyCity { get; set; }

        [Newtonsoft.Json.JsonProperty("propertyState", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PropertyState { get; set; }

        [Newtonsoft.Json.JsonProperty("propertyZip", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PropertyZip { get; set; }

        [Newtonsoft.Json.JsonProperty("defaultStatus", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string DefaultStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("bkRemovalCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BkRemovalCode { get; set; }

        [Newtonsoft.Json.JsonProperty("bkCh7DischargeFlag", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BkCh7DischargeFlag { get; set; }

        [Newtonsoft.Json.JsonProperty("bkCh11ToCh13DischargeFlag", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BkCh11ToCh13DischargeFlag { get; set; }

        [Newtonsoft.Json.JsonProperty("draftDay", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int? DraftDay { get; set; }

        [Newtonsoft.Json.JsonProperty("pmtDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PmtDate { get; set; }

        [Newtonsoft.Json.JsonProperty("pmtAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal PmtAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("authorizationMethod", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AuthorizationMethod { get; set; }

        [Newtonsoft.Json.JsonProperty("pmtTrackingNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PmtTrackingNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("pmtScheduledAt", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset PmtScheduledAt { get; set; }

        [Newtonsoft.Json.JsonProperty("description", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty("dayBeforeProcessingDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset DayBeforeProcessingDate { get; set; }

        [Newtonsoft.Json.JsonProperty("status", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum Status { get; set; }

    }

    
    public partial class AddSentEmailToLoanApiRequest
    {
        [Newtonsoft.Json.JsonProperty("dateSend", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset DateSend { get; set; }

        [Newtonsoft.Json.JsonProperty("letterID", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LetterIDEnum LetterID { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentFrequency", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentTypeEnum PaymentFrequency { get; set; }

    }

    
    public partial class AddTran73RecordApiRequest
    {
        [Newtonsoft.Json.JsonProperty("transactionDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset TransactionDate { get; set; }

        [Newtonsoft.Json.JsonProperty("loanNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string LoanNumber { get; set; }

        [Newtonsoft.Json.JsonProperty("paymentAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal PaymentAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("suspenseAmount", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal SuspenseAmount { get; set; }

        [Newtonsoft.Json.JsonProperty("suspenseSweepStatus", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public LoanPaymentStatusEnum SuspenseSweepStatus { get; set; }

        [Newtonsoft.Json.JsonProperty("nextPaymentDueDate", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset NextPaymentDueDate { get; set; }

        [Newtonsoft.Json.JsonProperty("processStopCode", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProcessStopCode { get; set; }

    }
    public enum LetterIDEnum
    {

        _0 = 0,

        _1 = 1,

        _2 = 2,

        _3 = 3,

        _4 = 4,

        _5 = 5,

        _6 = 6,

        _7 = 7,

        _8 = 8,

        _9 = 9,

        _10 = 10,

        _11 = 11,

        _12 = 12,

        _13 = 13,

        _14 = 14,

        _15 = 15,

        _16 = 16,

        _17 = 17,

        _18 = 18,

        _19 = 19,

        _20 = 20,

        _21 = 21,

        _22 = 22,

        _23 = 23,

        _24 = 24,

        _25 = 25,

    }

  




}
