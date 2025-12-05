using AventStack.ExtentReports;
using LD_AutomationFramework.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;

namespace LD_AutomationFramework.Utilities
{
    public class DBconnect
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string dbConnectionString { get; set; }
        ExtentTest test { get; set; }
        public string qaSqlConnection
        {
            get { return UtilAdditions.SetSqlConnectionString("QASqlConnection"); }
        }
        public string sgSqlConnection
        {
            get { return UtilAdditions.SetSqlConnectionString("SGSqlConnection"); }
        }

        public DBconnect(ExtentTest test, string dbNameForConnection)
        {
            this.test = test;
            if (ConfigSettings.Environment.Equals("QA"))
                dbConnectionString = qaSqlConnection.Replace("DATABASE", dbNameForConnection);
            else if (ConfigSettings.Environment.Equals("SG"))
                dbConnectionString = sgSqlConnection.Replace("DATABASE", dbNameForConnection);
            else
                dbConnectionString = qaSqlConnection.Replace("DATABASE", dbNameForConnection);
        }

        /// <summary>
        /// Method to execute database commands
        /// </summary>
        /// <param name="command">For example, ALTER DATABASE {databaseName} SET OFFLINE WITH ROLLBACK IMMEDIATE</param>
        /// <param name="databaseName">{databaseName}</param>
        /// <param name="parameters">Sql Parameters</param>
        /// <returns>Number of rows affected</returns>
        public int ExecuteCommand(string command, [Optional] string databaseName, SqlParameter[] parameters = null)
        {
            int numberofRowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                try
                {
                    using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                    {
                        try
                        {
                            sqlCommand.CommandType = CommandType.Text;
                            if (parameters != null)
                            {
                                if (parameters.Any())
                                {
                                    sqlCommand.Parameters.AddRange(parameters);
                                }
                            }
                            sqlCommand.Connection.Open();
                            numberofRowsAffected = sqlCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Failed while executing following command: " + command + "." + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Failed while eestablishing SQL connection: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return numberofRowsAffected;
        }

        /// <summary>
        /// Method to execute stored procedures
        /// </summary>
        /// <param name="procedureName">Stored procedure name</param>
        /// <param name="databaseName">{databaseName}</param>
        /// <param name="parameters">Sql Parameters</param>
        /// <returns>Number of rows affected</returns>
        public int ExecuteProcedure(string procedureName, [Optional] string databaseName, SqlParameter[] parameters = null)
        {
            int numberofRowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                try
                {
                    using (SqlCommand sqlCommand = new SqlCommand(procedureName, connection))
                    {
                        try
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            if (parameters != null)
                            {
                                if (parameters.Any())
                                {
                                    sqlCommand.Parameters.AddRange(parameters);
                                }
                            }
                            sqlCommand.Connection.Open();
                            numberofRowsAffected = sqlCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Failed while executing following stored procedure: " + procedureName + "." + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Failed while eestablishing SQL connection: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return numberofRowsAffected;
        }

        /// <summary>
        /// Method to execute database queries
        /// </summary>
        /// <param name="queryString">Select * from tablename</param>
        /// <param name="databaseName">{databaseName}</param>
        /// <param name="parameters">Sql Parameters</param>
        /// <returns>Data table</returns>
        public DataTable ExecuteQuery(string queryString, [Optional] string databaseName)
        {
            var table = new DataTable();

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                try
                {
                    using (SqlCommand sqlCommand = new SqlCommand(queryString, connection))
                    {

                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.Connection.Open();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            try
                            {
                                table.Load(reader);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Failed while executing following query: " + queryString + "." + ex.Message);
                            }
                            finally
                            {
                                reader.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Failed while eestablishing SQL connection: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return table;
        }

        /// <summary>
        /// Method to delete user profile to clear loans associated with an account in Customer Portal
        /// </summary>
        /// <param name="userProfileId">integer value</param>
        public void DeleteUserProfile(int userProfileId)
        {
            using (SqlConnection connection = new SqlConnection(dbConnectionString.Replace(Constants.DBNames.MelloServETL, Constants.DBNames.MelloServ)))
            {
                string[] queries = { "DELETE FROM [LDProfileDomain].[UserProperty] WHERE UserProfileID=@UserProfileID", "DELETE FROM MelloServ.LDProfileDomain.UserProfilePropCollection WHERE UserProfileID=@UserProfileID", "DELETE FROM MelloServ.LDProfileDomain.UserDevice WHERE UserProfileID=@UserProfileID", "DELETE FROM MelloServ.LDProfileDomain.UserTopicSubscriptions WHERE UserProfileID=@UserProfileID" };
                foreach (string query in queries)
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserProfileID", userProfileId);

                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Method to fetch the user profile from database based on corresponding email address
        /// </summary>
        /// <param name="email">test1@gmail.com</param>
        /// <returns></returns>
        public int GetUserProfileId(string email)
        {
            string query = "select top 1 UserProfileID from [LDProfileDomain].[UserProfile] where Email='" + email + "'";
            int userProfileId = 0;
            using (SqlConnection conn = new SqlConnection(dbConnectionString.Replace(Constants.DBNames.MelloServETL, Constants.DBNames.MelloServ)))
            {

                SqlCommand oCmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userProfileId = Convert.ToInt32(reader["UserProfileID"]);
                    }

                    conn.Close();
                }
            }
            return userProfileId;
        }

        /// <summary>
        /// Method to fetch the last payment date from database based on corresponding email address
        /// </summary>
        /// <param name="loanNumber">9221470603</param>
        /// <returns></returns>
        public string GetLastPaymentDate(string loanNumber)
        {
            string query = $"SELECT TOP (1) [LoanTransactionID],[transaction_date] FROM [MelloServETL].[dbo].[LoanTransaction] where loan_number = '{loanNumber}' order by transaction_date desc";
            string lastTransactionDate = string.Empty;
            using (SqlConnection conn = new SqlConnection(dbConnectionString.Replace(Constants.DBNames.MelloServETL, Constants.DBNames.MelloServ)))
            {
                SqlCommand oCmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = oCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastTransactionDate = reader["transaction_date"].ToString();
                    }
                    conn.Close();
                }
            }
            return lastTransactionDate;
        }

        /// <summary>
        /// To update the Borrower Email ID to loan level table 
        /// </summary>
        /// <param name="loanLevelData"> LoanLevelDetails</param>
        public void UpdateBorrowerEmailID(string loanNumber, string emailID)
        {
            string queryToUpdateBorrowerEmailID = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.UpdateBorrowerEmailID).Replace("LN_NUMBER", loanNumber).Replace("EMAIL_ID", emailID);
            var dBconnect = new DBconnect(test, Constants.DBNames.MelloServ);
            dBconnect.ExecuteQuery(queryToUpdateBorrowerEmailID).ToString();
            test.Log(Status.Info, $"Updated the borrower_email to {emailID} for loan_number: {loanNumber}");
        }

        /// <summary>
        /// To get the Borrower Phone Number from Telephone table
        /// </summary>
        /// <param name="loanNumber">Loan Number</param>
        /// <param name="phoneId">Phone ID (e.g., '1')</param>
        /// <returns>Phone Number as string</returns>
        public Dictionary<string, string> GetBorrowerPhoneNumber(string loanNumber, string phoneId = "1")
        {
            string queryToGetPhoneNumber = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetPhoneNumberWithPhoneId1)
                .Replace("LN_NUMBER", loanNumber)
                .Replace("PHONE_ID", phoneId);

            var dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
            var result = dBconnect.ExecuteQuery(queryToGetPhoneNumber);

            Dictionary<string, string> phoneNumberDetails = new Dictionary<string, string>();
            if (result.Rows.Count > 0)
            {
                phoneNumberDetails.Add("phone_number", result.Rows[0]["phone_number"].ToString());
                phoneNumberDetails.Add("phone_code", result.Rows[0]["phone_code"].ToString());
                phoneNumberDetails.Add("TelelphoneID", result.Rows[0]["TelelphoneID"].ToString());
            }
            test.Log(Status.Info, $"Fetched phone_number: {phoneNumberDetails["phone_number"]} for loan_number: {loanNumber}, phone_id: {phoneId}, TelelphoneID: {phoneNumberDetails["TelelphoneID"]}");
            return phoneNumberDetails;
        }

        /// <summary>
        /// To update the Borrower Phone Number in Telephone table
        /// </summary>
        /// <param name="loanNumber">Loan Number</param>
        /// <param name="phoneId">Phone ID (e.g., '1')</param>
        /// <param name="phoneNumber">New Phone Number</param>
        public void UpdateBorrowerPhoneNumber(string loanNumber, string phoneNumber, string phoneCode, string telePhoneId, string phoneId = "1")
        {
            string queryToUpdatePhoneNumber = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.UpdatePhoneNumberWithPhoneId1)
                .Replace("LN_NUMBER", loanNumber)
                .Replace("PHN_NUMBER", (phoneNumber.Equals("")) ? null : phoneNumber)
                .Replace("PHN_CODE", (phoneCode.Equals("")) ? null : phoneCode)
                .Replace("TELEPHONE_ID", (telePhoneId.Equals("")) ? null : telePhoneId);

            var dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
            dBconnect.ExecuteQuery(queryToUpdatePhoneNumber).ToString();
            test.Log(Status.Info, $"Updated phone_number to {phoneNumber} for loan_number: {loanNumber}, phone_id: {phoneId} , phone_code: {phoneCode} , TELEPHONE_ID: {telePhoneId}");
        }

        /// <summary>
        /// Inserts a loan record into LoanFlag table for FlagId 35
        /// </summary>
        /// <param name="loanNumber">Loan Number</param>
        /// <param name="flagId">Flag ID (e.g., '35param name="userName">User performing the insert</param>/// <param name="flagId">Flag ID (e.g., '35')</param>
        public void InsertLoanIntoLoanFlagTable(string loanNumber, string flagId, string userName)
        {
            string queryToInsertLoanFlag = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.InsertLoanIntoLoanFlagTableForFlagID35)
                .Replace("LN_NUMBER", loanNumber)
                .Replace("FLAG_ID", flagId)
                .Replace("USER_NAME", userName);

            var dBconnect = new DBconnect(test, Constants.DBNames.MelloServ);
            dBconnect.ExecuteQuery(queryToInsertLoanFlag).ToString();
            test.Log(Status.Info, $"Inserted LoanFlag record for loan_number: {loanNumber}, flag_id: {flagId}, user: {userName}");
        }

        /// <summary>
        /// Deletes a loan record from LoanFlag table for FlagId 35
        /// </summary>
        /// <param name="loanNumber">Loan Number</param>
        /// <param name="flagId">Flag ID (e.g., '35')</param>
        public void DeleteLoanFromLoanFlagTable(string loanNumber, string flagId)
        {
            string queryToDeleteLoanFlag = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.DeleteLoanFromLoanFlagTableForFlagID35)
                .Replace("LN_NUMBER", loanNumber)
                .Replace("FLAG_ID", flagId);

            var dBconnect = new DBconnect(test, Constants.DBNames.MelloServ);
            dBconnect.ExecuteQuery(queryToDeleteLoanFlag).ToString();
            test.Log(Status.Info, $"Deleted LoanFlag record for loan_number: {loanNumber}, flag_id: {flagId}");
        }
    }
}
