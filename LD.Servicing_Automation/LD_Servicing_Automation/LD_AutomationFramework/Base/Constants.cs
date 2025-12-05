using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LD_AutomationFramework
{
    public class Constants
    {
        public struct EncryptionKey
        {
            public const string EncryptKey = "m2FlrKnOYurIHl9/7n38zRffbOBNO1cdOFW413NEc70=";

            public const string InitializationVector = "qXvS3KkYxM+uZzmqGqfPbQ==";
        }
        public struct DeleteAutopayCredentials
        {
            public const string Email = "servicingtestautomation_msp@yopmail.com";//Generic email to be used for email testing

            public const string Username = "servicingtestaut_msp@loandepot.com";

            public const string Password = "gFbl16+tDDxAyFN1MBmxpTgCLUghaQK1EOeCwm14lBQ=";
        }
        public struct SolutionProjectNames
        {
            public const string AgentPortal = "LD_AgentPortal";

            public const string CustomerPortal = "LD_CustomerPortal";

            public const string PaymentEngine = "LD_PaymentEngine";

            public const string MSP = "MSP";
        }

        public struct PortalNames
        {
            public const string AgentPortal = "Agent Portal";

            public const string CustomerPortal = "CustomerPortal";
        }

        public struct AgentPortalTabNames
        {
            public const string LoanSummaryTab = "Loan Summary";

            public const string PaymentsTab = "Payments";
        }

        public struct PageNames
        {
            public const string EditPayment = "Edit Payment";

            public const string MakeAOneTimePayment = "Make a One-Time Payment";
        }

        public struct BannerDetails
        {
            public const string YourLoanCurrentlyOnAnActiveForbearancePlan = "Your loan is currently on an Active Forbearance Plan";

            public const string YourLoanCurrentlyOnAnActiveRepaymentPlan = "Your loan is currently on an Active Repayment Plan - Installment ";
        }

        public struct PromiseDataColumns
        {
            public const string PromiseNumber = "promise_number";
            public const string TotalPromiseNumber = "total_promisenumber";
            public const string AllDatesEnabledStatus = "alldates_enabledstatus";
        }

        public struct LoanLevelDataColumns
        {
            public const string LoanNumber = "loan_number";

            public const string PropertyZip = "property_zip";

            public const string BorrowerSsn = "borrower_ssn";

            public const string BorrowerFullName = "borrower_full_name";

            public const string BorrowerFirstName = "borrower_first_name";

            public const string BorrowerLastName = "borrower_last_name";

            public const string NextPaymentDueDate = "next_payment_due_date";

            public const string PropertyAddress = "property_address";

            public const string PropertyCity = "property_city";

            public const string PropertyState = "property_state";

            public const string LoanLevelID = "LoanLevelID";

            public const string ClientNumber = "client_number";

            public const string lnkey = "lnkey";

            public const string IsActive = "is_active";

            public const string InvestorId = "investor_id";

            public const string InvestorName = "investor_name";

            public const string InvestorGroup = "investor_group";

            public const string ProcessStopCode = "process_stop_code";

            public const string BadCheckStopCode = "bad_check_stop_code";

            public const string DisbursementStopCode = "disbursement_stop_code";

            public const string NoAnalysisStopCode = "no_analysis_stop_code";

            public const string LateChargeStopCode = "late_charge_stop_code";

            public const string NoNoticeStopCode = "no_notice_stop_code";

            public const string ForeclosureStopCode = "foreclosure_stop_code";

            public const string PifStopCode = "pif_stop_code";

            public const string FirstDueDate = "first_due_date";

            public const string MaturityDate = "maturity_date";

            public const string OriginationDate = "origination_date";

            public const string OriginalMortgageAmount = "original_mortgage_amount";

            public const string ProductType = "product_type";

            public const string LoanPurpose = "loan_purpose";

            public const string LoanPpurposeDescription = "loan_purpose_description";

            public const string LoanClosingDate = "loan_closing_date";

            public const string FirstPrincipalBalance = "first_principal_balance";

            public const string SecondPrincipalBalance = "second_principal_balance";

            public const string InterestRates = "interest_rates";

            public const string LoanMaturityDate = "loan_maturity_date";

            public const string EscrowBalance = "escrow_balance";

            public const string EscrowAdvanceBalance = "escrow_advance_balance";

            public const string SuspenseBalance = "suspense_balance";

            public const string DelinquentStatus = "delinquent_status";

            public const string DefaultStatus = "default_status";

            public const string LastFullPaymentDate = "last_full_payment_date";

            public const string RemainingTerm = "remaining_term";

            public const string BorrowerAdvocateProcessorId = "borrower_advocate_processor_id";

            public const string BorrowerAdvocateName = "borrower_advocate_name";

            public const string PrincipalInterestAmount = "principal_interest_amount";

            public const string TaxAndInsuranceAmount = "tax_and_insurance_amount";

            public const string ReplacementReserveAmount = "replacement_reserve_amount";

            public const string TotalMonthlyPayment = "total_monthly_payment";

            public const string AccruedLateChargeAmount = "accrued_late_charge_amount";

            public const string NsfFeeBalance = "nsf_fee_balance";

            public const string OtherFees = "other_fees";

            public const string CountyTax = "county_tax";

            public const string CityTax = "city_tax";

            public const string LienAmount = "lien_amount";

            public const string HazardAmount = "hazard_amount";

            public const string MiMonthlyAmount = "mi_monthly_amount";

            public const string OverShortAmount = "over_short_amount";

            public const string DaysPastDue = "days_past_due";

            public const string DelinquentPaymentCount = "delinquent_payment_count";

            public const string DelinquentPaymentBalance = "delinquent_payment_balance";

            public const string BankruptcyStatusCode = "bankruptcy_status_code";

            public const string ForeclosureStatusCode = "foreclosure_status_code";

            public const string ForeclosureStartDate = "foreclosure_start_date";

            public const string BorrowerMiddleName = "borrower_middle_name";

            public const string BorrowerEmail = "borrower_email";

            public const string CoBorrowerFirstName = "co_borrower_first_name";

            public const string CoBorrowerMiddleName = "co_borrower_middle_name";

            public const string CoBorrowerLastName = "co_borrower_last_name";

            public const string CoBorrowerFullName = "co_borrower_full_name";

            public const string CoBorrowerSsn = "co_borrower_ssn";

            public const string CoBorrowerEmail = "co_borrower_email";

            public const string PrimaryTelephoneNumber = "primary_telephone_number";

            public const string SecondaryTelephoneNumber = "secondary_telephone_number";

            public const string PropertyUnitNumber = "property_unit_number";

            public const string CurrentOccupancyStatusCode = "current_occupancy_status_code";

            public const string CurrentOccupancyStatusDescription = "current_occupancy_status_description";

            public const string BillingAddressLine3 = "billing_address_line_3";

            public const string BillingAddressLine4 = "billing_address_line_4";

            public const string BillingAddress = "billing_address";

            public const string BillingCity = "billing_city";

            public const string BillingState = "billing_state";

            public const string BillingZip = "billing_zip";

            public const string ServicingSoldId = "servicing_sold_id";

            public const string ServicingSoldDate = "servicing_sold_date";

            public const string NewServicerName = "new_servicer_name";

            public const string NewServicerPhoneNumber = "new_servicer_phone_number";

            public const string YtdPrincipalPaidAmount = "ytd_principal_paid_amount";

            public const string YtdInterestPaidAmount = "ytd_interest_paid_amount";

            public const string YtdTaxAmount = "ytd_tax_amount";

            public const string YtdLienAmount = "ytd_lien_amount";

            public const string YtdInsurancePaidAmount = "ytd_insurance_paid_amount";

            public const string YtdMipPmiPaidAmount = "ytd_mip_pmi_paid_amount";

            public const string YtdLateChargeAmount = "ytd_late_charge_amount";

            public const string YtdNsfAmount = "ytd_nsf_amount";

            public const string YtdOtherFees = "ytd_other_fees";

            public const string DaysPastDueStatus = "days_past_due_status";

            public const string IsEarlyTransferToCenlar = "is_early_transfer_to_cenlar";

            public const string IsTransferredToCenlar = "is_transferred_to_cenlar";

            public const string IsTransferredToThirdParty = "is_transferred_to_third_party";

            public const string IsPaidInFull = "is_paid_in_full";

            public const string LastFullPaymentAmount = "last_full_payment_amount";

            public const string BankruptcyChapter = "bankruptcy_chapter";

            public const string BankruptcyDisclosureStatus = "bankruptcy_disclosure_status";

            public const string BankruptcyRemovalCode = "bankruptcy_removal_code";

            public const string BankruptcyRemovalCodeDescription = "bankruptcy_removal_code_description";

            public const string LastModifiedDate = "last_modified_date";

            public const string CreatedDate = "created_date";

            public const string CreatedBy = "created_by";

            public const string UpdatedDate = "updated_date";

            public const string UpdatedBy = "updated_by";

            public const string CenlarLoanNumber = "cenlar_loan_number";

            public const string ManCode = "man_code";

            public const string LossMitigationStatusCode = "loss_mitigation_status_code";

            public const string ServicingTransferDate = "servicing_transfer_date";

            public const string MostRecentDisasterCode = "most_recent_disaster_code";

            public const string MostRecentDisasterName = "most_recent_disaster_name";

            public const string MostRecentDisasterDeclarationDate = "most_recent_disaster_declaration_date";

            public const string BkCh7DischargeCode = "bk_ch7_discharge_code";

            public const string FcSaleScheduledDate = "fc_sale_scheduled_date";

            public const string MsrStatusCode = "msr_status_code";

            public const string BkCh7DischargeDate = "bk_ch7_discharge_date";

            public const string BkCh111213DischargeCode = "bk_ch11_12_13_discharge_code";

            public const string BkCh111213DischargeDate = "bk_ch11_12_13_discharge_date";

            public const string DraftOption = "draft_option";

            public const string NextDraftDate = "next_draft_date";

            public const string WebRegistrationDate = "web_registration_date";

            public const string EConsent1098Flag = "e_consent_1098_flag";

            public const string EConsent1098Date = "e_consent_1098_date";

            public const string EConsentBillingFlag = "e_consent_billing_flag";

            public const string EConsentBillingDate = "e_consent_billing_date";

            public const string EConsentArmChangeNoticeFlag = "e_consent_arm_change_notice_flag";

            public const string EConsentArmChangeNoticeDate = "e_consent_arm_change_notice_date";

            public const string EConsentEscrowAnalysisFlag = "e_consent_escrow_analysis_flag";

            public const string EConsentEscrowAnalysisDate = "e_consent_escrow_analysis_date";

            public const string IsTestLoan = "is_test_loan";

            public const string PreviousYtdInterestPaidAmt = "previous_ytd_interest_paid_amt";

            public const string IsDeleted = "IsDeleted";

            public const string RecoverableCorpAdvBalance = "recoverable_corp_adv_balance";

            public const string CeaseDesistStopCode = "cease_desist_stop_code";

            public const string OptOutStopCode = "opt_out_stop_code";

            public const string RestrictedEscrowBalance = "restricted_escrow_balance";

            public const string LateChargeGraceDay = "late_charge_grace_day";

            public const string NewLoanSetupDate = "new_loan_setup_date";

            public const string acquisitionDate = "acquisition_date";

            public const string EmployeeCode = "employee_code";

            public const string TimeZone = "time_zone";

            public const string NewServicerLoanNumber = "new_servicer_loan_number";

            public const string LossMitigationIndicator = "loss_mitigation_indicator";

            public const string ProcessorPhoneNumber = "processor_phone_number";

            public const string ProcessorPhoneExtension = "processor_phone_extension";

            public const string BadMailingAddress = "bad_mailing_address";

            public const string DisasterTrackingFlag = "disaster_tracking_flag";

            public const string BorrowerCustomerID = "borrower_customerID";

            public const string CoBorrowerCustomerID = "co_borrower_customerID";

            public const string ConsumerIndicatorCode = "consumer_indicator_code";

            public const string ClsProductCode = "cls_product_code";

            public const string DemandLetterIssueDate = "demand_letter_issue_date";

            public const string DemandLetterExpireDate = "demand_letter_expire_date";

            public const string ForeclosureSetupDate = "foreclosure_setup_date";

            public const string LossMitigationSetupDate = "loss_mitigation_setup_date";

            public const string RepayPlanStatusCode = "repay_plan_status_code";

            public const string RepayPlanStatusDate = "repay_plan_status_date";

            public const string RepayPlanTotalAmount = "repay_plan_total_amount";

            public const string RepayPlanIndicator = "repay_plan_indicator";

            public const string RepayPlanType = "repay_plan_type";

            public const string RepayPlanPromiseCount = "repay_plan_promise_count";

            public const string ModTotalMonthlyPaymentAfter = "mod_total_monthly_payment_after";

            public const string CurrentPropertyValue = "current_property_value";

            public const string DraftingIndicator = "drafting_indicator";

            public const string DraftOptionCode = "draft_option_code";

            public const string Zone = "zone";

            public const string PaymentDueDate = "payment_due_date";

            public const string SuspenseAmount = "suspense_amount";

            public const string SubsidyAmount1 = "subsidy_amount1";

            public const string SubsidyAmount2 = "subsidy_amount2";

            public const string SubsidyAmount3 = "subsidy_amount3";

            public const string SubsidyAmount4 = "subsidy_amount4";

            public const string SubsidyAmount5 = "subsidy_amount5";

            public const string SubsidyPeriod1Months = "subsidy_period1_months";

            public const string SubsidyPeriod2Months = "subsidy_period2_months";

            public const string SubsidyPeriod3Months = "subsidy_period3_months";

            public const string SubsidyPeriod4Months = "subsidy_period4_months";

            public const string SubsidyPeriod5Months = "subsidy_period5_months";


            public const string MiscellaneousAmount = "miscellaneous_amount";

            public const string ReplacementReserveBalance = "replacement_reserve_balance";

            public const string DueAmount = "due_amount";

            public const string EscrowIndicator = "escrowed_indicator";
        }

        public struct CustomerPortalFrameNames
        {
            public const string ServiceBotFrame = "servisbot-messenger-iframe";
        }

        public struct ButtonNames
        {
            public const string MakeAPayment = "Make a Payment";

            public const string Confirm = "Confirm";

            public const string Cancel = "Cancel";

            public const string UpdatePayment = "Update Payment";

            public const string Apply = "Apply";

            public const string BackToAccountSummary = " Back to Account Summary ";

            public const string ManageAccounts = "Manage Accounts";
        }

        public struct AlertPopupNames
        {
            public const string ActiveForbearancePlanAlert = "Active Forbearance Plan Alert";

            public const string ActiveRepaymentPlanAlert = "Active Repayment Plan Alert";

            public const string BrokenRepaymentPlanAlert = "Broken Repayment Plan Alert";

            public const string DeletedForbearancePlanAlert = "Deleted Forbearance Plan Alert";

            public const string HelocPlanAlert = "Heloc Plan Alert";
        }

        public struct SectionNames
        {
            public const string PaymentSummary = "Payment Summary";
            public const string LoanSummary = "Loan Summary";
        }

        public struct ForbearancePlans
        {
            public const string Active = "Active";
            public const string Deleted = "Deleted";
            public const string Broken = "Broken";
        }

        public struct PaymentFlowType
        {
            public const string Edit = "Edit";
            public const string SetUp = "setup";
        }

        public struct BankAccountData
        {
            public const string BankAccountName = "AutomationTeamBankAccount";
            public const string BankAccountNumber = "1020308090";
            public const string BankAccountNumberWhileEdit = "9230361000";
            public const string RoutingNumber = "122199983";
            public const string AccountNumber = "61231234321";
            public const string MaskedBankAccountNumber = "*******9921";
            public const string BankName = "LoanDepotTest";
        }

        public struct Colors
        {
            public const string White = "rgba(0, 0, 0, 0)";
            public const string Red = "rgba(211, 68, 82, 1)";
            public const string LightRed = "rgba(253, 246, 246, 1)";
            public const string BrickRed = "rgba(207, 68, 82, 1)";
            public const string Orange = "rgba(255, 171, 0, 1)";
            public const string Purple = "rgba(80, 18, 131, 1)";
            public const string DeepPurple = "rgba(41, 9, 67, 0.22)";
            public const string Green = "rgba(5, 166, 120, 1)";
            public const string Grey = "rgba(0, 0, 0, 0.12)";
            public const string Yellow = "rgba(255, 171, 0, 1)";
            public const string NeutralGrey = "rgba(128, 128, 128, 1)";
            public const string LightGrey = "rgba(0, 0, 0, 0.12)";
            public const string NeonViolet = "rgba(122, 63, 246, 1)";
        }

        public struct CssAttributes
        {
            public const string BackgroundColor = "background-color";
            public const string Color = "color";
        }

        public struct ElementAttributes
        {
            public const string Disabled = "disabled";
            public const string AriaDisabled = "aria-disabled";
            public const string AriaLabel = "aria-label";
            public const string AriaChecked = "aria-checked";
            public const string Class = "class";
            public const string OuterHTML = "outerHTML";
            public const string InnerHTML = "innerHTML";
            public const string Value = "value";
            public const string ReadOnly = "readonly";
            public const string Href = "href";
        }

        public struct Messages
        {
            public const string YouHaveSuccessfullyScheduledPaymentBelow = "You’ve successfully scheduled the payment below";
            public const string NextPaymentToolTipInfoHeloc = "Please note that the amount due for your next payment will be available after the 16th of each month.";
            public const string NextPaymentToolTipInfoWhenUPBIsZeroHeloc = "There are no bills available for your account at this time. If you have recently paid down the principal balance to zero, you may still have an unbilled interest balance that will be available to pay online at the next billing.";
            public const string PaymentBreakDownSectionMessageForHeloc = "Please note that the payment breakdown for your next payment will be available after the 16th of each month.";
            public const string PaymentBreakDownSectionMessageWhenUPBIsZeroForHeloc = "There are no bills available for your account at this time. If you have recently paid down the principal balance to zero, you may still have an unbilled interest balance that will be available to pay online at the next billing.";
            public const string ProcessStopCodeBOverrideMessage = "This loan has a process stop code - B. An override is required in order to continue to make a payment. Are you sure you want to continue?";
            public const string MaximumAdditionalPrincipalPayment = "The maximum additional principal payment that can be scheduled using AutoPay is $9,999.99";
            public const string OptedForPaperLess = "Customer has opted to receive paperless notifications.";
            public const string NotOptedForPaperLess = "Customer has opted not to receive paperless notifications. Payment notices will be mailed to the mailing address on file.";
        }

        public struct FeeHints
        {
            public const string RemainingNSFFeeDue = "Remaining NSF Fee Due: $";
            public const string RemainingLateFeeDue = "Remaining Late Fee Due: $";
            public const string RemainingOtherFeesDue = "Remaining Other Fees Due: $";
        }

        public struct ErrorMessages
        {
            public const string TotalPaymentAmountGreaterThanUnpaidPrincipalBalance = "Total payment amount is greater than unpaid principal balance. Please update or submit payoff request.";
            public const string PleaseEnterAnAmountEqualToOrLessThanTotalFeesDue = "Please enter an amount equal to or less than the total fees due.";
            public const string MaximumAdditionalPrincipalPaymentAcceptedIs25000 = "Maximum additional principal payment accepted is $25,000.00";
        }

        public struct AmountValues
        {
            public const double MaximumAdditionalPrincipalPaymentValue = 25000.00;
            public const double AdditionalPrincipalPaymentParamValue = 10.00;
            public const double AdditionalPrincipalPaymentEditParamValue = 15.00;
        }

        public struct StopCodeTypes
        {
            public const string Soft = "Soft";

            public const string Hard = "Hard";

            public const string BadCheck = "Bad Check";
        }

        public struct IndicatorMessage
        {
            public const string SoftProcessStopCodeWhen3 = " Loan has process stop code 3 - Loan Correction and requires an override to make a payment. ";
            public const string SoftProcessStopCodeWhenB = " Loan has process stop code B - Bankruptcy and requires an override to make a payment. ";
            public const string SoftProcessStopCodeWhenH = " Loan has process stop code H - Legal and requires an override to make a payment. ";
            public const string SoftProcessStopCodeWhenL = " Loan has process stop code L - Loss Mitigation and requires an override to make a payment. ";
            public const string SoftProcessStopCodeWhenN = " Loan has process stop code N - NOI/Breach Issued and requires an override to make a payment. ";
            public const string HardProcessStopCodeWhen2 = " Loan has process stop code 2 - DPA , No Activity and payments are not allowed. ";
            public const string HardProcessStopCodeWhen8 = " Loan has process stop code 8 - Acquisitions and payments are not allowed. ";
            public const string HardProcessStopCodeWhenA = " Loan has process stop code A - Assumptions and payments are not allowed. ";
            public const string HardProcessStopCodeWhenF = " Loan has process stop code F - Foreclosure and payments are not allowed. ";
            public const string HardProcessStopCodeWhenM = " Loan has process stop code M - Matured Loan and payments are not allowed. ";
            public const string HardProcessStopCodeWhenP = " Loan has process stop code P - Payoff and payments are not allowed. ";
            public const string HardProcessStopCodeWhenR = " Loan has process stop code R - REO and payments are not allowed. ";
            public const string HardProcessStopCodeWhenU = " Loan has process stop code U - Unfunded / DeBoard and payments are not allowed. ";
            public const string HardProcessStopCodeWhenW = " Loan has process stop code W - Charge Off and payments are not allowed. ";
            public const string HardProcessStopCodeWhenExclamatory = " Loan has process stop code ! - Service Released and payments are not allowed. ";
            public const string HardProcessStopCodeWhen9 = " Loan has process stop code 9 - Repay/FB Plan and payments are not allowed. ";
            public const string BadCheckStopCodeWhen1 = " Loan has Bad Check stop code 1 - Draft / ACH Stop and payments are not allowed. ";
            public const string BadCheckStopCodeWhen4 = " Loan has Bad Check stop code 4 - Payoff Only No Pymts and payments are not allowed. ";
            public const string BadCheckStopCodeWhen6 = " Loan has Bad Check stop code 6 - Certified Funds Only and payments are not allowed. ";
        }

        public struct LabelNames
        {
            public const string ProfileIconDropdown = "arrow_drop_down";
            public const string SignOutOption = "Sign Out";
            public const string HelocNextPayment = "$--.--";
        }

        public struct DefaultsPlan
        {
            public const string Forbearance = "Forbearance";
            public const string Repayment = "Repayment";
            public const string OTP = "OTP";
        }

        public struct SettingDataColumns
        {
            public const string SettingValue = "SettingValue";
        }

        public struct FeesDataColumns
        {
            public const string AccruedLateChargeAmount = "accrued_late_charge_amount";

            public const string NsfFeeBalance = "nsf_fee_balance";

            public const string OtherFees = "other_fees";
        }

        public struct PastAndUpcomingFessDataColumns
        {
            public const string PastAccruedLateChargeAmount = "past_accrued_late_charge_amount";
            public const string UpcomingAccruedLateChargeAmount = "upcoming_accrued_late_charge_amount";
            public const string PastOtherFees = "past_other_fees";
            public const string UpcomingOtherFees = "upcoming_other_fees";
            public const string PastNSFFeeBalance = "past_nsf_fee_balance";
            public const string UpcomingNSFFeeBalance = "upcoming_nsf_fee_balance";
        }

        public struct Plans
        {
            public const string Active = "Active";
            public const string Completed = "Completed";
            public const string Deleted = "Deleted";
            public const string Broken = "Broken";
            public const string Empty = "Empty";
        }

        public struct DBNames
        {
            public const string MelloServETL = "MelloServETL";

            public const string MelloServ = "MelloServ";
        }

        public struct DBQueryFileName
        {
            public const string LoanDetails = "LoanDetails.xml";
        }

        public struct DBQueries
        {
            public const string GetLoanLevelDetailsForAutoPay = "xml/Query_GetLoanLevelDetailsForAutoPay";

            public const string GetLoanLevelDetailsForEscrowedAutoPayPastDueWithoutLateFees = "xml/Query_GetLoanLevelDetailsForEscrowedAutoPayPastDueWithoutLateFees";

            public const string GetLoanLevelDetailsForEscrowedAutopayWhenDueDateIsNextMonthOrTwoMonthsFromCurrentMonth = "xml/Query_GetLoanLevelDetailsForEscrowedAutopayWhenDueDateIsNextMonthOrTwoMonthsFromCurrentMonth";

            public const string GetNonEscrowedLoanLevelDetailsForAutoPay = "xml/Query_GetNonEscrowedLoanLevelDetailsForAutoPay";

            public const string GetEscrowedLoanLevelDetailsForAutoPay = "xml/Query_GetEscrowedLoanLevelDetailsForAutoPay";

            public const string GetLoanLevelDetailsWithActiveAutoPay = "xml/Query_GetLoanLevelDetailsWithActiveAutoPay";

            public const string GetLoanLevelDetailsForFmAutopayInEligibleProcessStopCodeAutopay = "xml/Query_GetLoanLevelDetailsForFmAutopayInEligibleProcessStopCodeAutopay";

            public const string GetLoanLevelDetailsForFmAutopayInEligibleANDPifStopCodeAutopay = "xml/Query_GetLoanLevelDetailsForFmAutopayInEligibleANDPifStopCodeAutopay";

            public const string GetLoanLevelDetailsForFmAutopayInEligibleBadCheckStopCodeAutopay = "xml/Query_GetLoanLevelDetailsForFmAutopayInEligibleBadCheckStopCodeAutopay";

            public const string GetLoanLevelDetailsForFmAutopayInEligibleForeclosureStopCodeAutopay = "xml/Query_GetLoanLevelDetailsForFmAutopayInEligibleForeclosureStopCodeAutopay";

            public const string GetLoanLevelDetailsForFmOTPInEligibleProcessStopCodeOTP = "xml/Query_GetLoanLevelDetailsForFmOTPInEligibleProcessStopCodeOTP";

            public const string GetLoanLevelDetailsForFmOTPInEligibleANDPifStopCodeOTP = "xml/Query_GetLoanLevelDetailsForFmOTPInEligibleANDPifStopCodeOTP";

            public const string GetLoanLevelDetailsForFmOTPInEligibleBadCheckStopCodeOTP = "xml/Query_GetLoanLevelDetailsForFmOTPInEligibleBadCheckStopCodeOTP";

            public const string GetLoanLevelDetailsForFmOTPInEligibleForeclosureStopCodeOTP = "xml/Query_GetLoanLevelDetailsForFmOTPInEligibleForeclosureStopCodeOTP";

            public const string GetLoanLevelDetailsForPendingPrepaidAutopay = "xml/Query_GetLoanLevelDetailsForPendingPrepaidAutopay";

            public const string GetLoanLevelDetailsForIneligiblePrepaidAutopay = "xml/Query_GetLoanLevelDetailsForIneligiblePrepaidAutopay";

            public const string UpdateBorrowerEmailID = "xml/Query_UpdateBorrowerEmailID";

            public const string UpdateTCPAFlagIsGlobalValue = "xml/QueryToUpdateTCPAFlagIsGlobalValue";

            public const string GetFlagIDForASpecificLoanNumber = "xml/Query_GetFlagIDForASpecificLoanNumber";

            public const string GetEligibleAutopayDeleteLoans = "xml/Query_GetLoanLevelDetailsForDeleteAutoPay";

            public const string GetEligibleAutopayDeleteLoansForPastDue = "xml/Query_GetLoanLevelDetailsForDeleteAutoPayForPastDue";

            public const string GetEligibleAutopayDeleteLoansForOntime = "xml/Query_GetLoanLevelDetailsForDeleteAutoPayForOntime";

            public const string GetEligibleAutopayDeleteLoansForPrepaidOneMonth = "xml/Query_GetLoanLevelDetailsForDeleteAutoPayForPrepaidOneMonth";

            public const string GetLoanLevelDetailsForEligibleAutoPayPastDue = "xml/Query_GetLoanLevelDetailsForEligibleAutoPayPastDue";

            public const string GetLoanLevelDetailsForEligibleAutoPayOntime = "xml/Query_GetLoanLevelDetailsForEligibleAutoPayOntime";

            public const string GetLoanLevelDetailsForEligibleAutoPayOntimeW01 = "xml/Query_GetLoanLevelDetailsForEligibleAutoPayOntimeW01";

            public const string GetLoanLevelDetailsForBiWeeklyAutoPay = "xml/Query_GetLoanLevelDetailsForBiWeeklyAutoPay";

            public const string GetLoanLevelDetailsForBiWeeklyAutoPayOntime = "xml/Query_GetLoanLevelDetailsForBiWeeklyAutoPayOntime";

            public const string GetLoanLevelDetailsForBiWeeklyAutoPayOneMonthPrepaid = "xml/Query_GetLoanLevelDetailsForBiWeeklyAutoPayOneMonthPrepaid";

            public const string GetLoanLevelDetailsForBiWeeklyAutoPayTwoMonthPrepaid = "xml/Query_GetLoanLevelDetailsForBiWeeklyAutoPayTwoMonthPrepaid";

            public const string GetLoanLevelDetailsForEligibleAutoPayPrepaid = "xml/Query_GetLoanLevelDetailsForEligibleAutoPayPrepaid";

            public const string Get100LoanLevelDetails = "xml/Query_GetTop100LoanLevelData";

            public const string GetActiveModificationTrialLoanLevelDetails = "xml/Query_GetLoanLevelDetailsForActiveModificationTrial";

            public const string GetSystemChangesInProgressTrialLoanLevelDetails = "xml/Query_GetLoanLevelDetailsForModificationTrialSystemChangesInProgress";

            public const string GetBrokenOrDeletedTrialLoanLevelDetails = "xml/Query_GetLoanLevelDetailsForBrokenOrDeletedModificationTrial";

            public static string GetEligibleAutopayBiWeeklyDeleteLoans = "xml/Query_GetLoanLevelDetailsForDeleteBiWeeklyAutoPay";

            public static string GetLoanLevelDetailsForDifferentLoanTypes = "xml/Query_GetLoanLevelDetailsForDifferentLoanTypes";

            public static string GetLoanDataFor0CDTesting = "xml/Query_GetLoanDataFor0CDTesting";

            public static string GetLoanLevelDetailsForEligibleOTPPrepaid = "xml/Query_GetLoanLevelDetailsForEligibleOTPPrepaid";

            public static string GetLoanLevelDetailsForEligibleOTPPrepaidOptOut = "xml/Query_GetLoanLevelDetailsForEligibleOTPPrepaidOptOut";

            public static string GetLoanLevelDetailsForEligibleOTPOntime = "xml/Query_GetLoanLevelDetailsForEligibleOTPOntime";

            public static string GetLoanLevelDetailsForEligibleOTPOntimeOptOut = "xml/Query_GetLoanLevelDetailsForEligibleOTPOntimeOptOut";

            public static string GetLoanLevelDetailsForEligibleOTPPastDue = "xml/Query_GetLoanLevelDetailsForEligibleOTPPastDue";

            public static string GetLoanLevelDetailsForEligibleOTPPastDueOptOut = "xml/Query_GetLoanLevelDetailsForEligibleOTPPastDueOptOut";

            public static string GetLoanLevelDetailsForEligibleOTPDelinquent = "xml/Query_GetLoanLevelDetailsForEligibleOTPDelinquent";

            public static string GetLoanLevelDetailsForEligibleOTPDelinquentOptOut = "xml/Query_GetLoanLevelDetailsForEligibleOTPDelinquentOptOut";

            public static string GetLoanLevelDetailsForEligibleHelocAutopayPastDue = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayPastDue";

            public static string GetLoanLevelDetailsForEligibleHelocAutopayPastDueWithoutFee = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayPastDueWithoutFee";

            public static string GetLoanLevelDetailsForEligibleHelocAutopayDelinquent = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayDelinquent";

            public const string GetLoanLevelDetailsForEligibleHelocAutopayOntime = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayOntime";

            public const string GetLoanLevelDetailsForEligibleHelocAutopayPrepaidOneMonth = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayPrepaidOneMonth";

            public static string GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioPastDue = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioPastDue";

            public const string GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioOntime = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioOntime";

            public const string GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioPrepaid = "xml/Query_GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioPrepaid";

            public const string GetLoanLevelDetailsForEligiblePaperless = "xml/Query_GetLoanLevelDetailsForPaperless";

            public const string GetActiveLoanForNonPayments = "xml/Query_GetActiveLoanForNonPayments";

            public const string GetLoanLevelDetailsforPaperless = "xml/Query_GetLoanLevelDetailsForPaperless";

            public const string GetLoansLevelDetailsForPayoffquote = "xml/Query_GetLoansLevelDetailsForPayoffQuote";

            public const string GetHelocLoanLevelDetailsforPaperless = "xml/Query_GetHelocLoanLevelDetailsforPaperless";

            public const string GetLoansLevelDetailsForSMCVerification = "xml/Query_GetLoansLevelDetailsForSMCVerification";

            public const string GetLoansPayoffIneligibleDPA = "xml/Query_IneligiblePayoffRequestDPALoans";

            public const string GetLoansPayoffIneligibleInactive = "xml/Query_IneligiblePayoffRequestInactiveLoans";

            public const string GetLoansPayoffIneligibleFHA_RHS = "xml/Query_IneligiblePayoffRequestFHA_RHSLoans";

            public const string GetLoansPayoffIneligibleForeclosure = "xml/Query_IneligiblePayoffRequestForeclosureASLoans";

            public const string GetLoansPayoffIneligibleDelinquent = "xml/Query_IneligiblePayoffRequestDelinquentLoans";

            public const string GetLoansPayoffIneligibleVA = "xml/Query_IneligiblePayoffRequestVALoans";

            public const string GetLoansPayoffIneligiblePIF = "xml/Query_IneligiblePayoffRequestPIFLoans";

            public const string GetLoansPayoffIneligibleTransferred = "xml/Query_IneligiblePayoffRequestTransferredLoans";

            public const string GetLoansAutopayPromptFM = "xml/Query_MortgageAutopayPrompt";

            public const string GetLoansForOTPWithSubsidy = "xml/Query_MortgageOTPOntimeWithSubsidy";

            public const string ServisBotEligibleLoans = "xml/Query_ServisBotEligibleLoans";

            public const string GetLoansForActiveHelocDraws = "xml/Query_ActiveHelocDrawsLoans";

            public const string UpdateBorrowersFirstLastAndFullName = "xml/Query_UpdateBorrowersFirstLastAndFullName";

            public const string GetUsersRegisteredViaAutomation = "xml/Query_GetUsersRegisteredViaAutomation";

            public const string HelocIneligibleAutopayForeclosureStopCodes = "xml/Query_HelocLoansWithIneligibleForeclosureStopCodes";

            public const string HelocIneligibleAutopayProcessStopCodes = "xml/Query_HelocLoansWithIneligibleProcessStopCodes";

            public const string HelocIneligibleAutopayBadCheckStopCodes = "xml/Query_HelocLoansWithIneligibleBadCheckStopCodes";

            public const string HelocIneligibleAutopayPIFStopCodes = "xml/Query_HelocLoansWithIneligiblePIFStopCodes";

            public const string HelocIneligibleAutopayInvCodes = "xml/Query_HelocLoansWithIneligibleInvCodes";

            public const string HelocIneligibleAutopayPrepay2Months = "xml/Query_HelocLoansWithPrepay2Months";

            public const string HelocIneligibleAutopayDelinquentCountMorethanTwo = "xml/Query_HelocLoansWithDelinquentCountMorethanTwo";

            public const string FMPendingAutopayLoans = "xml/Query_FMExistingAutopaySetup";

            public const string FMPendingOTPLoans = "xml/Query_FMExistingOTPSetup";

            public const string HelocPendingAutopayLoans = "xml/Query_HelocExistingAutopaySetup";

            public const string HelocPendingOTPLoans = "xml/Query_HelocExistingOTPSetup";

            public const string HelocExsitingOTPForBankAccountValidation = "xml/Query_HElocExistingOTPForBankValidation";

            public const string FMExistingOTPForBankAccountValidation = "xml/Query_FMExistingOTPForBankValidation";

            public const string FMExistingAutopayOnTodayDraftDateLoans = "xml/Query_FMExistingAutopayOnToday";

            public const string HelocExistingAutopayOnTodayDraftDateLoans = "xml/Query_HelocExistingAutopayOnToday";

            public const string GetPhoneNumberWithPhoneId1 = "xml/Query_GetPhoneNumberWithPhoneId1";

            public const string UpdatePhoneNumberWithPhoneId1 = "xml/Query_UpdatePhoneNumberWithPhoneId1";

            #region Common

            public const string GetSettingDetailsForBankHolidays = "xml/Query_GetSettingDetailsForBankHolidays";

            public const string GetSettingDetailsBySettingKeyValue = "xml/Query_GetSettingDetailsBySettingKeyValue";

            public const string GetActiveRepaymentPlanPromiseDetails = "xml/Query_GetActiveRepaymentPlanPromiseDetails";

            public const string GetFeesWithLoanNumber = "xml/Query_GetFeesWithLoanNumber";


            #endregion Common

            #region Forbearence

            public const string GetLoanLevelDetailsForActiveForbearance = "xml/Query_GetLoanLevelDetailsForActiveForbearance";

            public const string GetLoanLevelDetailsForDeletedForbearance = "xml/Query_GetLoanLevelDetailsForDeletedForbearance";

            public const string GetLoanLevelDetailsForCurrentMonthDeletedForbearance = "xml/Query_GetLoanLevelDetailsForCurrentMonthDeletedForbearance";

            #endregion Forbearence

            #region Repayment

            public const string GetLoanLevelDetailsForActiveRepayment = "xml/Query_GetLoanLevelDetailsForActiveRepayment";

            public const string GetLoanLevelDetailsForBrokenRepayment = "xml/Query_GetLoanLevelDetailsForBrokenRepayment";

            public const string GetLoanLevelDetailsForDeletedRepayment = "xml/Query_GetLoanLevelDetailsForDeletedRepayment";

            public const string GetLoanLevelDetailsForCompletedActiveRepayment = "xml/Query_GetLoanLevelDetailsForCompletedActiveRepayment";

            public const string GetLoanLevelDetailsByNextOrAfterMonthActiveRepayment = "xml/Query_GetLoanLevelDetailsByNextOrAfterMonthActiveRepayment";

            public const string GetLoanLevelDetailsByCurrentMonthAfterCutOffTimeActiveRepayment = "xml/Query_GetLoanLevelDetailsByCurrentMonthAfterCutOffTimeActiveRepayment";

            #endregion Repayment

            #region ModificationTrail

            public const string GetLoanLevelDetailsForModificationTrail = "xml/Query_GetLoanLevelDetailsForActiveModificationTrial";

            #endregion ModificationTrail

            #region OTP

            public const string GetLoanLevelDetailsForOTPSuspenseWithProcessStopCode = "xml/Query_GetLoanLevelDetailsForOTPSuspenseWithProcessStopCode";
            public const string GetLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot = "xml/Query_GetLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot";
            public const string GetLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9 = "xml/Query_GetLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9";
            public const string GetLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9 = "xml/Query_GetLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9";
            public const string GetLoanLevelDetailsForOTPSuspenseGreaterThanZero = "xml/Query_GetLoanLevelDetailsForOTPSuspenseGreaterThanZero";
            public const string GetLoanLevelDetailsForOTPSuspenseIsZero = "xml/Query_GetLoanLevelDetailsForOTPSuspenseIsZero";
            public const string GetLoanLevelDetailsForOTPSuspenseWithDelinquentPaymentCountAndProcessStopCode = "xml/Query_GetLoanLevelDetailsForOTPSuspenseWithDelinquentPaymentCountAndProcessStopCode";
            public const string GetLoanLevelDetailsForOTPSuspenseIsZeroAndProcessStopCodeELN = "xml/Query_GetLoanLevelDetailsForOTPSuspenseIsZeroAndProcessStopCodeELN";
            public const string GetLoanLevelDetailsForOTPWithProcessStopCode = "xml/Query_GetLoanLevelDetailsForOTPWithProcessStopCode";
            public const string GetLoanLevelDetailsForOTPConditionWithProcessStopCode = "xml/Query_GetLoanLevelDetailsForOTPConditionWithProcessStopCode";
            public const string GetLoanLevelDetailsForOTPWithBadCheckStopCode = "xml/Query_GetLoanLevelDetailsForOTPWithBadCheckStopCode";
            public const string GetLoanLevelDetailsForOTPNextPaymentDueDate = "xml/Query_GetLoanLevelDetailsForOTPNextPaymentDueDate";
            public const string GetLoanLevelDetailsForOTPMiscellaneousGreaterThanZero = "xml/Query_GetLoanLevelDetailsForOTPMiscellaneousGreaterThanZero";
            public const string GetLoanLevelDetailsForOTPTaxAndInsuranceGreaterThanZero = "xml/Query_GetLoanLevelDetailsForOTPTaxAndInsuranceGreaterThanZero";
            public const string GetLoanLevelDetailsForOTPWhenProcessStopCodeIsB = "xml/Query_GetLoanLevelDetailsForOTPWhenProcessStopCodeIsB";
            public const string GetLoanLevelDetailsForOTPForPaymentAdded = "xml/Query_GetLoanLevelDetailsForOTPForPaymentAdded";
            public const string GetLoanLovelDetailsForOTPWithFees = "xml/Query_GetLoanLovelDetailsForOTPWithFees";
            public const string GetLoanLevelDetailsForOTPCutOff = "xml/Query_GetLoanLevelDetailsForOTPCutOff";
            public const string GetLoanLevelDetailsForOTPTimeSensitiveCheck = "xml/Query_ExistingOTPSetupForTimeSensitiveCheck";
            public const string GetLoanLevelDetailsForHelocOTPTimeSensitiveCheck = "xml/Query_ExistingHelocOTPSetupForTimeSensitiveCheck";
            public const string GetLoanLevelDetailsForHELOCOTPEligible = "xml/Query_OTPEligibleHELOCLoans";
            public const string GetLoanLevelDetailsForOntimePastdue = "xml/Query_OntimeToPastdue";
            public const string GetLoanLevelDetailsForOntimePastdueHeloc = "xml/Query_OntimeToPastdueHeloc";
            public const string GetE2EOTPOnTimeSetupEditPayment = "xml/Query_GetE2EOTPOnTimeSetupEditPayment";
            public const string GetLoanLevelDetailsForOTPSuspenseGreaterThenZero = "xml/Query_GetLoanLevelDetailsForOTPSuspenseGreaterThenZero";
            public const string GetE2EOTPPrepaidSetupEditPayment = "xml/Query_GetE2EOTPPrepaidSetupEditPayment";
            public const string GetE2EOTPPrimaryBorrowerIsOptedInForPaperless = "xml/Query_GetE2EOTPPrimaryBorrowerIsOptedInForPaperless";
            public const string GetE2EOTPPrimaryBorrowerOptedOutForPaperless = "xml/Query_GetE2EOTPPrimaryBorrowerOptedOutForPaperless";
            public const string GetE2EOTPPastdueSetupEditPayment = "xml/Query_GetE2EOTPPastdueSetupEditPayment";
            public const string GetE2EOTPDelinquentSetupEditPayment = "xml/Query_GetE2EOTPDelinquentSetupEditPayment";

            #endregion OTP

            #region HelocOTP

            public const string GetLoanLevelDetailsForOnTimeHelocOTP = "xml/Query_GetLoanLevelDetailsForOnTimeHelocOTP";

            public const string GetLoanLevelDetailsForPastDueHelocOTP = "xml/Query_GetLoanLevelDetailsForPastDueHelocOTP";

            public const string GetLoanLevelDetailsForPastDueHelocOTPWithSoftProcessStopCode = "xml/Query_GetLoanLevelDetailsForPastDueHelocOTPWithSoftProcessStopCode";

            public const string GetLateChargesForHelocOTP = "xml/Query_GetLateChargesForHelocOTP";

            public const string GetLoanLevelDetailsForPastDueOrDelinquentHelocOTP = "xml/Query_GetLoanLevelDetailsForPastDueOrDelinquentHelocOTP";

            public const string GetPastAndUpcomingFeesWithLoanNumberForHelocOTP = "xml/Query_GetPastAndUpcomingFeesWithLoanNumberForHelocOTP";

            public const string GetPendingPaymentDetailsWithLoanNumber = "xml/Query_GetPendingPaymentDetailsWithLoanNumber";

            public const string GetLoanLevelDetailsForOnTimeOrPrepaidHelocOTP = "xml/Query_GetLoanLevelDetailsForOnTimeOrPrepaidHelocOTP";

            public const string GetLoanLevelDetailsForOnTimeWithUPBHelocOTP = "xml/Query_GetLoanLevelDetailsForOnTimeWithUPBHelocOTP";

            #endregion HelocOTP

            #region CutOffTime

            public static string CutOffTime_GetPaymentsSubmittedByServiceAccount = "xml/Query_CutOffTime_PaymentsSubmittedByServiceAccount";

            #endregion CutOffTime

            public const string InsertLoanIntoLoanFlagTableForFlagID35 = "xml/Query_InsertLoanIntoLoanFlagTableForSkipMFA";

            public const string DeleteLoanFromLoanFlagTableForFlagID35 = "xml/Query_DeleteLoanFromLoanFlagTableForSkipMFA";
        }

        public struct DBQueriesForMSP
        {
            public const string GetTop100LoanLevelData = "xml/Query_GetTop100LoanLevelData";
            public const string GetHelocDelinquentPlusLoanData = "xml/Query_HelocDelinquentPlusLoanData";
            public const string GetDelinquentPlusLoanData = "xml/Query_DelinquentPlusLoanData";
        }

        public struct CustomerPortalErrorMsgs
        {
            public const string IneligibleLoanForPaymentSetupErrorMsg = "Your loan is currently ineligible for online payments. Please contact Customer Service at 866-258–6572 or send us a secure message for additional information.";

            public const string AutopayNotAllowedOnAutopayPageErrorMsg = "Autopay not allowed at this time.\r\nYour loan is currently ineligible to enroll in AutoPay. Please contact Customer Service at (866) 258 – 6572 Monday through Friday from 7 am – 7 pm, and Saturday from 8 am – 5pm CT,or send us a secure message for additional information.\r\nSetup Autopay\r\nSend a Secure Message";

            public const string HelocAutopayNotAllowed = "Your loan is currently ineligible to enroll in AutoPay. Please contact Customer Service at (866) 790 – 4254 Monday through Friday from 7 am – 7 pm, and Saturday from 8 am – 5pm CT, or send us a secure message for additional information.";

            public const string PaperlessErrorMessage = "Some services are currently unavailable. Please try again later";

            public const string AutopayNotEligibleForW01 = "Your loan may take up to 90 days to fully reconcile, during this time the automatic payment option is unavailable. Once this message disappears you will be able to register for monthly or bi-weekly payment plans.";

            public const string ModificationTrailOTPErrorMsg = "Your loan is currently ineligible for online payments. Please contact Customer Service at 866-258–6572 or send us a secure message for additional information.";

            public const string IneligiblePayoffQuoteErrorMsg = "Unfortunately, we cannot process payoff quote at this time. Please call us at (866) 258-6572 Monday - Friday, from 7:00 am - 7:00 pm CST and Saturday 8:00 am - 5:00 pm CST for further assistance with your request.";

            public const string HelocIneligibleForPayment = "Your HELOC account is currently ineligible for online payments. Please contact customer service at 866-790-4254 Monday-Friday, from 7:00 a.m. - 7:00 p.m. CST and Saturday 8:00 a.m. - 5:00 p.m. CST.";

            public const string PaymentAlreadyExistsOnSameDay = "The payment date selected already has a scheduled payment pending.";

            public const string AccountNicknameErrorMsg = " Only 100 alphanumeric characters allowed ";

            public const string AccountFirstNameErrorMsg = " Please enter the first name on your bank account. ";

            public const string AccountLastNameErrorMsg = " Please enter the last name on your bank account. ";

            public const string RoutingNumberErrorMsg = " The routing number entered cannot be recognized. Please verify and try again ";

            public const string AccountNumberErrorMsg = " Invalid account number – only numbers are permitted ";

            public const string ConfirmAccountNumberErrorMsg = " The account numbers entered do not match. Please verify and try again. ";

            public static readonly string LateFeeMessage = $"A late fee may be assessed for payments made after {new DateTime(DateTime.Now.Year, DateTime.Now.Month, 16):M/d/yyyy}.";

            public static string BiweeklyNotAllowedMessage = "Bi-weekly Payment Plan is not allowed. Loan must be prepaid.";

            public const string PleaseEnterDollorGreaterOrEqualTo1000AndUptoAvailableCreditLineErrorMsg = " Please enter a dollar amount equal to or greater than $1,000.00 and up to your available credit line. ";

            public const string PleaseEnterAnAmountEqualToOrLessThanYourAvailableCreditLineErrorMsg = " Please enter an amount equal to or less than your available credit line. ";

            public const string UnableToProceedPaymentWarningMsg = "Unable to Process Payment\r\nYou are not able to setup another payment as there is already a monthly payment scheduled for your loan using Autopay\r\nClose";

            public const string UnableToProceedPaymentWarningForFMOntimeMsg = "Unable to Process Payment\r\nThere is already a monthly payment scheduled for your loan using Autopay. However, you may setup additional payments to be added to your principal and escrow.\r\nClose";

            public const string UnableToProceedPaymentWarningForHelocMsg = "Unable to Process Payment\r\nYou are not able to setup another payment as there is already a monthly payment scheduled for your loan using Autopay.\r\nClose";

            public const string UnableToProceedPaymentWarningForHelocOntimeMsg = "Unable to Process Payment\r\nThere is already a monthly payment scheduled for your loan using Autopay. However, you may setup additional principal payments if your current bill is paid.\r\nClose";

            public const string BankAccountDeleteErrorMsgHeloc = "This account has a pending payment and cannot be deleted until the payment is complete or canceled";

            public const string BankAccountDeleteErrorMsgFM = "This account has a pending payment and cannot be deleted until the payment has been completed or deleted";
        }

        public struct TimeZones
        {
            public const string CentralStandardTime = "Central Standard Time";
            public const string EasternStandardTime = "Eastern Standard Time";
            public const string PacificStandardTime = "Pacific Standard Time";
        }

        public struct Urls
        {
            public const string LoanDepotUrl = "https://www.loandepot.com/";
        }
        public struct ToolsAndResourcesLinks
        {
            public const string MSP = "https://onepassuat1.lendingsvcs.com/adfs/ls/?wa=wsignin1.0&wtrealm=urn%3abkfs%3asso%3arp%3aportal%3auat1&wctx=rm%3d0%26id%3dpassive%26ru%3d%252fportal%252fPortalchrome.aspx&wct=2023-10-30T19%3a36%3a18Z";
            public const string MelloAssist = "https://assist.loandepot.com/sp";
            public const string Workday = "https://wd5.myworkday.com/loandepot/d/home.htmld";
            public const string Proctor = "https://www.expressinsuranceinfo.com/2917832";
            public const string AIQ = "https://loandepotsg.emaiq-pp8.net/";
        }
        public struct CustomerPortalTextMessages
        {
            public const string AutopaySetup1BusinessDayPopupMsg = "The autopay plan setup may take up to 1 business day to process and cannot be deleted on the same day the plan was added. You may return on the following business day to delete the payment plan.";

            public const string AutopaySetup2BusinessDayPopupMsg = "The autopay plan setup may take up to 2 business days to process and cannot be deleted on the same day the plan was added.";

            public const string AutopayDelete1BusinessdayPopupMsg = "The autopay plan deletion may take up to 1 business day to process and cannot be reactivated on the same day the plan was deleted. You may return on the following business day to reactivate your payment plan. Click the Confirm button if you wish to continue deleting this plan.";

            public const string AutopayDelete2BusinessdayPopupMsg = "The autopay plan deletion may take up to 2 business days to process and cannot be reactivated on the same day the plan was deleted.";

            public const string AutopayAfterDeleteAllowUs1BusinessDayForProcessingMsg = "Autopay not allowed at this time.\r\nWe are currently processing your request to delete the previous automatic payment. Please allow up to 1 business day for processing. Once completed, you may set up your new automatic payment.\r\nSetup Autopay";

            public const string AutopaySetupReviewPageDisclosureText = "By clicking \"Confirm Autopay\" you are authorizing loanDepot and their respective successors and assigns (collectively referred to as loanDepot) to process recurring ACH payments on this loan that will be debited according to the information above. Funds for these transactions must be available in the bank account provided on the date the transaction is scheduled for payment. If there are insufficient funds when the payment is scheduled to post, bank fees may be assessed. If loanDepot is unable to collect the funds, the bank will return the electronic payment and the payment will be reversed from the loan. This action may result in a fee being assessed to the mortgage account and the cancelation of the recurring ACH payment plan. By clicking the “Confirm Autopay\" button, you are agreeing to these terms.\r\nThe monthly payment amount is subject to change. The total draft will be determined at the time each monthly draft is processed.\r\nIf you have questions regarding the servicing of your loan, please send us a Secure Message or contact us at 866-258–6572 . We are here to assist Monday - Friday, from 7:00 a.m. - 7:00 p.m. CST and Saturday 8:00 a.m. - 5:00 p.m. CST.\r\nEdit Autopay\r\nConfirm Autopay";

            public const string AutopaySetupConfirmationPageDisclosureText = "To change this email or to modify your notification preferences, please go to your profile section and update your preferences.\r\nPrint this Page\r\nBack To Manage Autopay";

            public const string AutopaySetupBiweeklyNotAllowedWarningForOntimeText = " Bi-weekly Payment Plan is not allowed. Loan must be prepaid. ";

            public const string HelocAutopaySetupReviewPageDisclosureText = "By clicking “Confirm Autopay\" you are authorizing loanDepot to process recurring ACH payments on this loan that will be debited according to the information above. Funds for these transactions must be available in the bank account provided on the date the transaction is scheduled for payment. If there are insufficient funds when the payment is scheduled to post, loanDepot will attempt to withdraw funds from the bank account a second time and bank fees may be assessed for one or both attempts. If loanDepot is unable to collect the funds, the bank will return the electronic payment and the payment will be reversed from the loan. This action may result in a fee being assessed to the mortgage account and the cancelation of the recurring ACH payment plan. By clicking the Create Autopayment button, you are agreeing to these terms. For questions, please contact our Customer Service team at (866) 790-4254, Monday - Friday, from 7:00 a.m. - 7:00 p.m CST and Saturday 8:00 a.m. - 5:00 p.m. CST.\r\nThe monthly payment amount is subject to change. The total draft will be determined at the time each monthly draft is processed. For questions, please contact our Customer Service team at (866) 790-4254, Monday - Friday, from 7:00 a.m. - 7:00 p.m CST and Saturday 8:00 a.m. - 5:00 p.m. CST.\r\nIf you have questions regarding the servicing of your loan, please send us a Secure Message or contact us at 866-790 4254 . We are here to assist Monday - Friday, from 7:00 a.m. - 7:00 p.m. CST and Saturday 8:00 a.m. - 5:00 p.m CST.\r\nEdit Autopay\r\nConfirm Autopay";

            public const string HelocAutopaySetupConfirmationPageDisclosureText = "Contact Us\r\nNeed help? Start a conversation with one of our customer service agents.\r\nCustomer Service Hours:\r\nMon - Fri, 7:00 a.m. - 7:00 p.m. CST\r\nSat, 8:00 a.m - 5:00 p.m. CST\r\nLet's Chat Send us a secure message\r\nCall us at (866)-790-4254";

            public const string HelocAutopayNextPaymentAvailableAfter16thOnDashboardPageText = " Please note that the amount due for your next payment will be available after the 16th of each month. Please return after 16th to make a payment or contact customer service at 866-790-4254 ";

            public const string HelocAutopayPleaseRevisitAfter16thOfEveryMonthOnSetupAutopayPageText = "Please revisit after the 16th of the month to confirm your payment amount.";

            public const string VerifyYourIdentityPopupTextContent = "Verify Your Identity\r\nWe need to verify your identity in order to link your loan by sending you a code via email.\r\nEmail\r\nReceive Code Via Email\r\nWhat is a multi-factor authentication code?\r\nCancel";

            public const string VerifyYourIdentityPopupOnContactInfoPageTextContent = "Verify Your Identity\r\nWe need to verify your identity to save your profile changes by sending you a code via email.\r\nEmail\r\nReceive Code Via Email\r\nPlease refer to our Frequently Asked Questions (FAQ) page if you have questions regarding the multi-factor authentication process.\r\nCancel";

            public const string VerifyYourIdentityPopupForOtherThanFirstLoanTextContent = "Verify Your Identity\r\nWe need to verify your identity in order to link your loan by sending you a code via email.\r\nEmail\r\nReceive Code Via Email\r\nPlease refer to our Frequently Asked Questions (FAQ) page if you have questions regarding the multi-factor authentication process.\r\nCancel";

            public const string MultiFactorAuthenticatorCodeTooltipTextContent = "A multi-factor authentication code is generated to verify your identity when linking your online account to your loan or when you have made changes to your online registration details. Click ‘Send Code’ on the popup window and enter the code sent via SMS text or email in the designated field. Then click ‘Verify’ to receive confirmation or notice that your profile changes are saved. The MFA code is valid for 10 minutes after it is generated. If it expires before you can use it or you don’t receive one, click ‘Resend’ in the popup window and a new code will be sent  to your cell phone or email.";

            public const string VerifyYourIdentifyVerificationCodeInputPopupTextContent = "Verify Your Identity\r\nWe have sent a code to AUTOT***************@YOPMAIL.COM . Please input down below.\r\nVerification Code\r\nDidn't get a code? Resend\r\nWhat is a multi-factor authentication code?\r\nVerify\r\nCancel";

            public const string VerifyYourIdentifyVerificationCodeInputForOtherThanFirstLoanPopupTextContent = "Verify Your Identity\r\nWe have sent a code to AUTOT***************@YOPMAIL.COM . Please input down below.\r\nVerification Code\r\nDidn't get a code? Resend\r\nPlease refer to our Frequently Asked Questions (FAQ) page if you have questions regarding the multi-factor authentication process.\r\nVerify\r\nCancel";

            public const string VerifyYourIdentifyVerificationCodeInputOnContactInfoPopupTextContent = "Verify Your Identity\r\nWe have sent a code to autot***************@yopmail.com . Please input down below.\r\nVerification Code\r\nDidn't get a code? Resend\r\nPlease refer to our Frequently Asked Questions (FAQ) page if you have questions regarding the multi-factor authentication process.\r\nVerify\r\nCancel";

            public const string PayoffDescription = "A payoff quote shows the date your payoff is good through (referred to as the \"good-through date\"), and the remaining balance of your mortgage loan, which includes your outstanding principal balance, accrued interest, late charges/fees, and any other amounts owed. Request your payoff quote as you think about paying off your mortgage. Your payoff amount will likely be different from the principal balance shown on your monthly statement because of the additional days of interest or other amounts due.";

            public const string PaperlessDescription = "The paperless settings may take up to 24 hours to reflect on your account and will override any previous selections. You will not be able to make multiple updates to the paperless settings on the same day; however, you may return on the following business day to make any additional updates.";

            public const string PaperlessToggleDescription = "Please use the toggles to select the documents you would like to receive electronically. Turn a toggle off for any documents you prefer to receive in paper form.";

            public const string VerifiedMFAAuthenticationPopupTextContent = "Verified!\r\nYou will now have access to all of your loan information.\r\nContinue";

            public const string NAIHCDescription = "If you are an American Indian, Alaska Native, or Native Hawaiian homeowner seeking housing assistance on or off tribal land, please";

            public const string VerifiedMFAAuthenticationPopupOnContactInfoPageTextContent = "Verified!\r\nYour profile changes have been saved.\r\nContinue";

            public const string PaperlessDashboardDescription = "You'll soon be able to eliminate clutter by choosing paperless statements and documents";

            public const string whatIsMultiFactorAuthenticationTextContentOnFaqsPage = "A multi-factor authentication code is generated to verify your identity when linking your online account to your loan or when you have made changes to your online registration details. Click ‘Send Code’ on the popup window and enter the code sent via SMS text or email in the designated field. Then click ‘Verify’ to receive confirmation or notice that your profile changes are saved.\r\nThe MFA code is valid for 10 minutes after it is generated. If it expires before you can use it or you don’t receive one, click ‘Resend’ in the popup window and a new code will be sent  to your cell phone or email.";

            public const string NeedAnEvenQuickerResponsePopupTextContentOnSMCPage = "Need an even quicker response?\r\nclose\r\nTry our new chat feature for immediate assistance with payments, documents, account details and more!\r\nContinue With Secure Message CenterLet'S Chat!";

            public const string NeedAnEvenQuickerResponsePopupTextContentOnSMCPageAfter5PmPst = "Need an even quicker response?\r\nclose\r\nOur dedicated live agents are standing by ready to assist with payments, documents, account details and more!\r\nContinue With Secure Message CenterLet'S Chat!";

            public const string CreateYourPasswordTextContent = "PASSWORD RESET: 3 of 3\r\nCreate your password.\r\nNow that we've verified your identity, please create your password below.\r\nNew Password\r\nvisibility\r\nPassword must contain:\r\nOne letter character\r\nOne number\r\nOne Special Character\r\n8 characters minimum\r\nConfirm New Password\r\nvisibility\r\nCONTINUE";

            public const string AutopayPromptMsg = "Setup recurring payments with our Autopay feature to make regular, timely payments and avoid late fees";

            public const string ProdClientLogin = "c7d69de2-6420-4ab3-ac37-c32fc45b380e/b2c_1_unified_signup_signin";

            public const string ProdRegSignup = "c7d69de2-6420-4ab3-ac37-c32fc45b380e/oauth2/v2.0/authorize?p=B2C_1_UNIFIED_SIGNUP";

            public const string AdvanceRequestFromLineOfCreditTextContnetForActiveHelocDraws = "Advance Request from Line of Credit\r\nRequest Funds";

            public const string MFAPopUpAddAnAccountOnAdvanceFromLineOfCreditTextContent = "Please check your email\r\nWe have sent a code to <EMAIL_ID>.\r\nDidn't get a code? Resend\r\nIf you have questions regarding HELOC Draws, please review our Frequently Asked Questions (FAQ) page.\r\nVERIFY\r\nCANCEL";

            public const string DrawRequestProcessingTimePopUpTextContent = "Draw Request Processing Time\r\nclose\r\nPlease note that:\r\nAll requests received before 3 PM CST / 1 PM PST will be processed the same business day.\r\nAll requests received after 3 PM CST / 1 PM PST will be processed the next business day.\r\nRequests will not be processed during weekends or banking holidays.\r\nCONFIRM\r\nCANCEL";

            public const string DrawRequestSuccessRequestFundsTextContent = "The Draw Fund has been Requested Successfully\r\n×";

            public const string RequestFundsAdvanceRequestFromLineOfCreditTextContent = "Advance Request from Line of Credit\r\nRequest Date\r\n<CURRENT_DATE>\r\nAmount\r\n$1,000.00\r\nDraw Pending";

            public const string EditAutopayMsg = "In order to modify the frequency on your existing autopay schedule you need to disable the current autopay and set up a new one with your preferred plan.";

            public const string LoginMFAPopUpBeforeRequestingCodeTextContent = "Identity Verification\r\nWe need to confirm your identity before logging you into your account by sending a verification code through email, text message, or phone call.\r\nEmail\r\nReceive Code Via Email\r\nPhone Number\r\nReceive Code Via Text\r\nPhone Number\r\nReceive Code Via Phone Call\r\nCancel";

            public const string LoginMFAPopUpAfterRequestingCodeTextContent = "Identity Verification\r\nWe have sent a code to autot***************@yopmail.com . Please input down below.\r\nVerification Code\r\nRemember this device\r\nVERIFY\r\nDidn't get a code? Resend";

            public const string LoginMFAPopPleaseSelectPhoneNumberInfoText = "Please select a phone number where you are able to receive text messages.";

            public const string LoginMFAVerificationCodeEmailTemplateText = "This code expires in 10 minutes.\r\nIf you did not request a code, we suggest you change your password and contact our Customer Service team as soon as possible.\r\nWe can be reached at servicing.loandepot.com to send us a secure message or call our Customer Service Team at (866)258-6572 Monday through Friday between the hours of 7:00 a.m. and 7:00 p.m. and Saturday 8:00 a.m. to 5 p.m. CST.\r\nRegards,\r\nLoan Servicing Department\r\nloanDepot.com, LLC\r\nNotifications of Error, Requests for Information, Qualified Written Requests, or Billing Error Notifications (Home Equity Lines of Credit only) concerning your account must be directed to:\r\nloanDepot\r\nP.O. Box 251027\r\nPlano, TX 75025\r\n";
        }

        public struct FeeDataColumns
        {
            public const string TransactionAmount = "transaction_amount";
        }

        public struct FeeType
        {
            public const string LateCharges = "Late Charges";
            public const string OtherFees = "Other Fees";
            public const string NSFFees = "NSF Fees";
            public const string PastDuePayments = "Past Due Payments";
            public const string AnnualMaintenanceFees = "Annual Maintenance Fees";
        }

        public struct PaymentsDue
        {
            public const string DueDate = "DueDate";
            public const string TotalMonthlyPayment = "TotalMonthlyPayment";
            public const string PrincipalAndInterest = "PrincipalAndInterest";
            public const string TaxAndInsurance = "TaxAndInsurance";
            public const string IsSuspense = "IsSuspense";
        }

        public struct LoanInformationDataColumns
        {
            public const string LoanNumber = "LoanNumber";

            public const string Paperless = "Paperless";
        }

        public struct LoanStatus
        {
            public const string Delinquent = "Delinquent";

            public const string PastDue = "Past Due";

            public const string Ontime = "On Time";

            public const string PrepaidOneMonth = "PrepaidOneMonth";

            public const string PrepaidTwoMonth = "PrepaidTwoMonth";
        }

        public struct SMCCode
        {
            public const string SM203 = "SM203";

            public const string SM256 = "SM256";

            public const string SM257 = "SM257";

            public const string SM258 = "SM258";

            public const string SM260 = "SM260";
        }

        public struct EmailNotificationFromYopmail
        {
            public const string OTPSetup = "A New Message Regarding Your Onetime Payment Setup";

            public const string OTPDelete = "A New Message Regarding Your Onetime Payment Cancellation";

            public const string AutopaySetup = "A New Message Regarding Your Monthly Autopay Setup";

            public const string AutopayDelete = "A New Message Regarding Your Autopay Cancellation";

            public const string VerificationCode = "VerificationCode";

            public const string EmailTemplateYourAuthenticationCodeTextContent = "Your Authentication Code";
        }

        public struct EmailContents
        {
            public const string OTPSetupTitle = "A New Message Regarding Your Onetime Payment Setup";
            public const string SenderEmailId = "DoNotReply@loanDepot.com <DoNotReply@dispatch-loanDepot.com>";
            public const string OTPSetupEmailContentBody = "Please log in to your loanDepot account and access the Secure Message Center for additional details regarding the one-time payment scheduled for your account. If you have questions regarding your account, please send us a secure message or contact Customer Service at (866)258-6572 Monday - Friday, 7:00 a.m. - 7:00 p.m. and Saturday from 8:00 a.m. - 5:00 p.m. CST.\r\nThank you for being the best part of loanDepot.\r\nNotifications of Error, Requests for Information, or Qualified Written Requests concerning your loan must be directed to:\r\nloanDepot\r\nP.O. Box 251027\r\nPlano, TX 75025\r\nNOTE: THIS E-MAIL ADDRESS IS NOT MONITORED PLEASE DO NOT REPLY TO THIS MESSAGE.\r\nNMLS#174457 - NMLS Consumer Access Site | 6561 Irvine Center Drive Irvine, CA 92618 | loanDepot.com, LLC";

            public const string OTPDeleteTitle = "A New Message Regarding Your Onetime Payment Cancellation";
            public const string OTPDeleteEmailContentBody = "Please log in to your loanDepot account and access the Secure Message Center for additional details regarding the one-time payment cancellation on your account. If you have questions regarding your account, please send us a secure message or contact Customer Service at (866)258-6572 Monday - Friday, 7:00 a.m. - 7:00 p.m. and Saturday from 8:00 a.m. - 5:00 p.m. CST.\r\nThank you for being the best part of loanDepot.\r\nNotifications of Error, Requests for Information, or Qualified Written Requests concerning your loan must be directed to:\r\nloanDepot\r\nP.O. Box 251027\r\nPlano, TX 75025\r\nNOTE: THIS E-MAIL ADDRESS IS NOT MONITORED PLEASE DO NOT REPLY TO THIS MESSAGE.\r\nNMLS#174457 - NMLS Consumer Access Site | 6561 Irvine Center Drive Irvine, CA 92618 | loanDepot.com, LLC";

            public const string AutopaySetupTitle = "A New Message Regarding Your Monthly Autopay Setup";
            public const string AutopaySetupEmailContentBody = "Please log in to your loanDepot account and access the Secure Message Center for additional details regarding the monthly Autopay draft scheduled for your account. If you have questions regarding your account, please send us a secure message or contact Customer Service at (866)258-6572 Monday - Friday, 7:00 a.m. - 7:00 p.m. and Saturday from 8:00 a.m. - 5:00 p.m. CST.\r\nThank you for being the best part of loanDepot.\r\nNotifications of Error, Requests for Information, or Qualified Written Requests concerning your loan must be directed to:\r\nloanDepot\r\nP.O. Box 251027\r\nPlano, TX 75025\r\nNOTE: THIS E-MAIL ADDRESS IS NOT MONITORED PLEASE DO NOT REPLY TO THIS MESSAGE.\r\nNMLS#174457 - NMLS Consumer Access Site | 6561 Irvine Center Drive Irvine, CA 92618 | loanDepot.com, LLC";

            public const string AutopayDeleteTitle = "A New Message Regarding Your Autopay Cancellation";
            public const string AutopayDeleteEmailContentBody = "Please log in to your loanDepot account and access the Secure Message Center for additional details regarding the Autopay cancellation on your account. If you have questions regarding your account, please send us a secure message or contact Customer Service at (866)258-6572 Monday - Friday, 7:00 a.m. - 7:00 p.m. and Saturday from 8:00 a.m. - 5:00 p.m. CST.\r\nThank you for being the best part of loanDepot.\r\nNotifications of Error, Requests for Information, or Qualified Written Requests concerning your loan must be directed to:\r\nloanDepot\r\nP.O. Box 251027\r\nPlano, TX 75025\r\nNOTE: THIS E-MAIL ADDRESS IS NOT MONITORED PLEASE DO NOT REPLY TO THIS MESSAGE.\r\nNMLS#174457 - NMLS Consumer Access Site | 6561 Irvine Center Drive Irvine, CA 92618 | loanDepot.com, LLC\r\n     ";
        }

        public struct AutopayPaymentFrequency
        {
            public const string Monthly = "Monthly";

            public const string Biweekly = "Biweekly";
        }

        public static IList<string> Statements = new ReadOnlyCollection<string>(new List<string>
        {
           "All Document Types",
            "Monthly Statement",
            "Escrow Statement",
            "Letters",
            "Tax Documents"
        });

        public static IList<string> Products = new ReadOnlyCollection<string>(new List<string>
        {
        "Solar",
        "Home Maintenance",
        "Home Renovation"
        });

        public static IList<string> TaxDocs = new ReadOnlyCollection<string>(new List<string>
                {
                    "IRS 1098",
                    "IRS 1099 INT"
                });

        public static IList<string> OtherTypes = new ReadOnlyCollection<string>(new List<string>
        {
        "Monthly Statement",
        "Escrow Statement"
        });

        public static IList<string> PayoffQuoteChannels = new ReadOnlyCollection<string>(new List<string>
        {
            "Fax",
            "Secure Messages",
            "US Postal Mail"
        });

        public static IList<string> Letters = new ReadOnlyCollection<string>(new List<string>
                {
                    "Hazard Change Notice",
                    "Loss Payee Address",
                    "Property Vacancy Notification Letter",
                    "Voluntary HO6 Coverage",
                    "Escrow Waiver",
                    "GoodbyeLetter",
                    "Payoff Statement",
                    "HELOC 30 Day Suspension Notice",
                    "HELOC Borrower Suspension Request Form",
                    "HELOC Borrower Suspension Request Receipt Confirmation",
                    "HELOC Borrower ReOpen Request Form",
                    "Initial ARM Change Notice",
                    "ARM Change Notice",
                    "Servicing Transfer Letter",
                    "Loss Payee Address Change",
                    "Change of Risk",
                    "MIP Cancellation Denial",
                    "PMI BPO-APPR Appeal Req",
                    "MIP FHA Cancellation Ltr",
                    "MIP Change of Mortgagee",
                    "PMI Auto Term Notice",
                    "404 Notice",
                    "Letter Retraction",
                    "HUD AVOIDING FC COVER LTR",
                    "Borrower Advocate Request",
                    "Forbearance Agreement",
                    "LM Request Acknowledgement",
                    "Facially Complete LM Request",
                    "FRB Cancellation Letter",
                    "FRB Expiring Letter",
                    "FRB Kept Letter",
                    "Late Charge Letter",
                    "SCRA Notice",
                    "HUD Counseling Notice",
                    "HUD Face to Face Notice",
                    "Homeownership Counseling",
                    "LM Solicitation",
                    "LM Bankruptcy Solicitation",
                    "REPAYMENT PLAN OFFER LETTER",
                    "Delinquency Impairment Letter",
                    "Return payment",
                    "ACH",
                    "Partial Payments",
                    "Late Charge Waived",
                    "Paid in Full Letter",
                    "Payment History",
                    "DF014 - ACH VERIFICATION DRFT",
                    "Auto Draft NSF Letter",
                    "Cancelled ACH",
                    "BiWeekly Email",
                    "Borrower Correspondence",
                    "Payment Setup Document",
                    "RETURN CHECK LETTER",
                    "Payment Cancellation Document",
                    "10 Day Payment Reminder",
                    "CEASE AND DESIST",
                    "Welcome Letter",
                    "Attorney Representation Letter",
                    "401K",
                    "RETURN SHORT PMT.",
                    "CASH LTR - Returned Item",
                    "2nd Notice PMT to New Servicer",
                    "Name or Address Change",
                    "Auto Draft Conf Email",
                    "CBR Letter",
                    "Year End Letter"
    });

        public struct PaymentSetupDataColumns
        {
            public const string PaymentId = "Id";

            public const string ConfirmationNumber = "ConfirmationNumber";

            public const string PaymentStatus = "PaymentStatus";

            public const string PaymentDate = "PaymentDate";

            public const string PaymentProfileId = "PaymentProfileId";

            public const string Channel = "Channel";

            public const string UserId = "UserId";

            public const string LoanPaymentTypeFrequency = "LoanPaymentTypeFrequency";

            public const string TotalDraftAmount = "TotalDraftAmount";

            public const string MonthlyPaymentAmount = "MonthlyPaymentAmount";

            public const string AuthorizedBy = "AuthorizedBy";

            public const string UserName = "UserName";

            public const string LoanNumber = "LoanNumber";

            public const string MaskedBankAccountNumber = "MaskedBankAccountNumber";




        }

        #region MSP

        public struct MSPFrameNames
        {
            public const string APP_Frame = "ifr_APP";
            public const string DIR_Frame = "ifr_DIR";
        }

        public struct MSPScreenNames
        {
            public const string DLQ1 = "DLQ1";
            public const string SAF1 = "SAF1";
            public const string ELC3 = "ELC3";
            public const string PMT2 = "PMT2";
            public const string PMTU = "PMTU";
            public const string DENI = "DENI";
        }

        public struct ProcessStopCodes
        {
            public const string CodeExclamationMark = "!";
            public const string Code1 = "1";
            public const string Code2 = "2";
            public const string Code3 = "3";
            public const string Code4 = "4";
            public const string Code5 = "5";
            public const string Code6 = "6";
            public const string Code7 = "7";
            public const string Code8 = "8";
            public const string Code9 = "9";
        }

        public struct MFAByPassCode
        {
            public const string ByPassMFACode = "022025";
        }

        #endregion MSP
    }
}