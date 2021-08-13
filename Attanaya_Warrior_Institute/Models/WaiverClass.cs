using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Attanaya_Warrior_Institute.Models
{
    public class WaiverClass
    {
        public Guid WaiverId { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantPhone { get; set; }
        public string ParticipantEmail { get; set; }
        public string EContactName { get; set; }
        public string EContactPhone { get; set; }
        public string EContactRelationship { get; set; }
        public DateTime SignatureDate { get; set; }
        public string ParentGuradian { get; set; }
        public string ParentGuardianRelationship { get; set; }
        public bool ParentGuardianHasAgreed { get; set; }

        public static HttpStatusCodeResult SaveWaiver(WaiverClass waiver)
        {
            if (waiver == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var connection = new SqlConnection(Utility.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("CreateNewWaiverRecord", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ParticipantName", SqlDbType.NVarChar).Value = waiver.ParticipantName;
                        cmd.Parameters.Add("@ParticipantPhone", SqlDbType.NVarChar).Value = waiver.ParticipantPhone;
                        cmd.Parameters.Add("@ParticipantEmail", SqlDbType.NVarChar).Value = waiver.ParticipantEmail;
                        cmd.Parameters.Add("@EContactName", SqlDbType.NVarChar).Value = waiver.EContactName;
                        cmd.Parameters.Add("@EContactPhone", SqlDbType.NVarChar).Value = waiver.EContactPhone;
                        cmd.Parameters.Add("@EContactRelationship", SqlDbType.NVarChar).Value = waiver.EContactRelationship;
                        cmd.Parameters.Add("@SignatureDate", SqlDbType.NVarChar).Value = waiver.SignatureDate;
                        cmd.Parameters.Add("@ParentGuradian", SqlDbType.NVarChar).Value = waiver.ParentGuradian;
                        cmd.Parameters.Add("@ParentGuardianRelationship", SqlDbType.NVarChar).Value = waiver.ParentGuardianRelationship;

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                            "{0} ({1}) has signed the waiver release form. ",
                            waiver.ParticipantName,
                            waiver.ParticipantEmail), nameof(SaveWaiver));
                    }
                }
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(SaveWaiver));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }
    }

}