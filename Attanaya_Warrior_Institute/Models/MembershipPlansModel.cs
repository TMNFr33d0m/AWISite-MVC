using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Attanaya_Warrior_Institute.Models
{
    public class MembershipPlansModel
    {
        public Guid RecordId { get; set; }
        public string PlanName { get; set; }
        public decimal PlanPricePerMonth { get; set; }
        public string PlanDescription { get; set; }
        public string PayPalPlanId { get; set; }
        public DateTime PlanCreationDate { get; set; }
        public int PlanPurchaseMaxCount { get; set; }
        public string PlanCardImagePath { get; set; }
        public int DisplayOrder { get; set; }
        public int MaxMonthlyUses { get; set; }
        public int MaxPersonsPerSession { get; set; }
         
        #region 

        public static List<MembershipPlansModel> GetAllMembershipPlans ()
        {

           List<MembershipPlansModel> membershipPlans = new List<MembershipPlansModel>();

            string queryString = "SELECT * FROM dbo.MembershipPlans ORDER BY DisplayOrder ASC";

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                var command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MembershipPlansModel membershipPlan = new MembershipPlansModel();

                            membershipPlan.RecordId = (Guid)reader["RecordId"];
                            membershipPlan.PlanName = (string)reader["PlanName"];
                            membershipPlan.PlanPricePerMonth = (decimal)reader["PlanPricePerMonth"];
                            membershipPlan.PlanDescription = (string)reader["PlanDescription"];
                            membershipPlan.PayPalPlanId = (string)reader["PayPalPlanId"];
                            membershipPlan.PlanCreationDate = (DateTime)reader["PlanCreationDate"];
                            membershipPlan.PlanPurchaseMaxCount = (int)reader["PlanPurchaseMaxCount"];
                            membershipPlan.PlanCardImagePath = (string)reader["PlanCardImagePath"];
                            membershipPlan.DisplayOrder = (int)reader["DisplayOrder"];
                            membershipPlan.MaxMonthlyUses = (int)reader["MaxMonthlyUses"];
                            membershipPlan.MaxPersonsPerSession = (int)reader["MaxPersonsPerSession"];

                            membershipPlans.Add(membershipPlan);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetAllMembershipPlans));
                    throw;
                }
            }

            return membershipPlans;
        }

        public HttpStatusCodeResult CreateNewMembershipPlan(MembershipPlansModel plan)
        {
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CreateNewClass", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PlanName", SqlDbType.NVarChar).Value = plan.PlanName;
                        cmd.Parameters.Add("@PlanPricePerMonth", SqlDbType.Decimal).Value = plan.PlanPricePerMonth;
                        cmd.Parameters.Add("@PlanDescription", SqlDbType.NVarChar).Value = plan.PlanDescription;
                        cmd.Parameters.Add("@PayPalPlanId", SqlDbType.NVarChar).Value = plan.PayPalPlanId;
                        cmd.Parameters.Add("@PlanCreationDate", SqlDbType.DateTime).Value = plan.PlanCreationDate;
                        cmd.Parameters.Add("@PlanPurchaseMaxCount", SqlDbType.Int).Value = plan.PlanPurchaseMaxCount;
                        cmd.Parameters.Add("@PlanCardImagePath", SqlDbType.VarChar).Value = plan.PlanCardImagePath;
                        cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int).Value = plan.DisplayOrder;
                        cmd.Parameters.Add("@MaxMonthlyUses", SqlDbType.Int).Value = plan.MaxMonthlyUses;
                        cmd.Parameters.Add("@MaxPersonsPerSession", SqlDbType.Int).Value = plan.MaxPersonsPerSession;

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                            "Admin has created a new subscription plan! {0} -  ",
                            plan.PlanName), nameof(CreateNewMembershipPlan));
                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(CreateNewMembershipPlan));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        public HttpStatusCodeResult UpdateNewMembershipPlan(MembershipPlansModel plan)
        {
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CreateNewClass", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PlanName", SqlDbType.NVarChar).Value = plan.PlanName;
                        cmd.Parameters.Add("@PlanPricePerMonth", SqlDbType.Decimal).Value = plan.PlanPricePerMonth;
                        cmd.Parameters.Add("@PlanDescription", SqlDbType.NVarChar).Value = plan.PlanDescription;
                        cmd.Parameters.Add("@PayPalPlanId", SqlDbType.NVarChar).Value = plan.PayPalPlanId;
                        cmd.Parameters.Add("@PlanCreationDate", SqlDbType.DateTime).Value = plan.PlanCreationDate;
                        cmd.Parameters.Add("@PlanPurchaseMaxCount", SqlDbType.Int).Value = plan.PlanPurchaseMaxCount;
                        cmd.Parameters.Add("@PlanCardImagePath", SqlDbType.VarChar).Value = plan.PlanCardImagePath;
                        cmd.Parameters.Add("@DisplayOrder", SqlDbType.Int).Value = plan.DisplayOrder;
                        cmd.Parameters.Add("@MaxMonthlyUses", SqlDbType.Int).Value = plan.MaxMonthlyUses;
                        cmd.Parameters.Add("@MaxPersonsPerSession", SqlDbType.Int).Value = plan.MaxPersonsPerSession;

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                            "Admin has created a new subscription plan! {0} -  ",
                            plan.PlanName), nameof(CreateNewMembershipPlan));
                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(CreateNewMembershipPlan));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        #endregion
    }

}