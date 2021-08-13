using IronBarCode;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Attanaya_Warrior_Institute.Models
{
    public class PackageDealModel
    {
        #region

            [Key]
            [Required]
            [HiddenInput]
            // This is generated automatically at the model level just before SQL insert. It is returned to the client as a success artifact. 
            public Guid PackageDealId { get; set; }

            [Required]
            [DisplayName("Package Type")]
            public int PackageType { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
            [DisplayName("First Name")]
            public string PurchaserFirstName { get; set; }

            [DisplayName("Middle Name")]
            public string PurchaserMiddleName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
            [DisplayName("Last Name")]
            public string PurchaserLastName { get; set; }

            [Required]
            [EmailAddress]
            [DisplayName("Email Address")]
            public string PurchaserEmailAddress { get; set; }

            [Required]
            [Phone]
            [DisplayName("Phone Number")]
            public string PurchaserPhoneNumber { get; set; }

            [Required]
            [HiddenInput]
            public DateTime PurchaseDate { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
            [DisplayName("Full Name")]
            public string RecipientName { get; set; }

            [Required]
            [Phone]
            [DisplayName("Phone Number")]
            public string RecipientPhone { get; set; }

            [Required]
            [EmailAddress]
            [DisplayName("Email Address")]
            public string RecipientEmail { get; set; }

            [HiddenInput]
            public DateTime? RedemptionDate { get; set; }

            [HiddenInput]
            public string PackageDescription { get; set; }

        #endregion

        /// <summary>
        /// Get a gift certificate using the GUID (As scanned from a barcode or entered manually)
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static PackageDealModel GetPackageDealByGuid(Guid guid)
        {
            PackageDealModel PackageDeal = new PackageDealModel();

            string queryString = "SELECT * FROM dbo.GiftCertificates WHERE PackageDealId = '" + guid + "';";

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
                            PackageDeal.PackageDealId = (Guid) reader["PackageDealId"];
                            PackageDeal.PackageType = (int) reader["PackageType"];
                            PackageDeal.PurchaserFirstName = (string) reader["PurchaserFirstName"];
                            PackageDeal.PurchaserMiddleName = (string) reader["PurchaserMiddleName"];
                            PackageDeal.PurchaserLastName = (string) reader["PurchaserLastName"];
                            PackageDeal.PurchaserEmailAddress = (string) reader["PurchaserEmailAddress"];
                            PackageDeal.PurchaserPhoneNumber = (string) reader["PurchaserPhoneNumber"];
                            PackageDeal.PurchaseDate = (DateTime) reader["PurchaseDate"];
                            PackageDeal.RecipientName = (string) reader["RecipientName"];
                            PackageDeal.RecipientPhone = (string) reader["RecipientPhone"];
                            PackageDeal.RecipientEmail = (string) reader["RecipientEmail"];
                            PackageDeal.RedemptionDate = (DateTime?) reader["RedemptionDate"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetPackageDealByGuid));
                    throw;
                }
            }


            return PackageDeal; 
        }

        /// <summary>
        /// Create a new instance of a gift certificate. Returns a Guid if successful or a guid.empty if failure (from empty model or SQL error, check logs if empty guid.) 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Guid CreateNewGiftCertificate(PackageDealModel model)
        {
            if (model == null)
            {
                return Guid.Empty;
            }

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                // Generate Gift Certificate Number
                model.PackageDealId = Guid.NewGuid();
                model.PurchaseDate = Utility.GetCurrentTucsonTime();

                string queryString1 = "INSERT INTO dbo.GiftCertificates(" +
                                      "[PackageDealId], " +
                                      "[PackageType], " +
                                      "[PurchaserFirstName], " +
                                      "[PurchaserMiddleName], " +
                                      "[PurchaserLastName], " +
                                      "[PurchaserEmailAddress], " +
                                      "[PurchaserPhoneNumber], " +
                                      "[PurchaseDate], " +
                                      "[RecipientName], " +
                                      "[RecipientPhone], " +
                                      "[RecipientEmail],  " +
                                      "[RedemptionDate]" +
                                      ") VALUES('" +
                                      model.PackageDealId + "','" +
                                      model.PackageType + "','" +
                                      model.PurchaserFirstName + "','" +
                                      model.PurchaserMiddleName + "','" +
                                      model.PurchaserLastName + "','" +
                                      model.PurchaserEmailAddress + "','" +
                                      model.PurchaserPhoneNumber + "','" +
                                      model.PurchaseDate + "','" +
                                      model.RecipientName + "','" +
                                      model.RecipientPhone + "','" +
                                      model.RecipientEmail + "','" +
                                      model.RedemptionDate + "'"
                                      + ")";

                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,
                        "{0} ({1}) has purchased a gift certificate for {2} ({3}). ",
                        model.PurchaserFirstName + " " + model.PurchaserMiddleName + " " + model.PurchaserLastName,
                        model.PurchaserEmailAddress,
                        model.RecipientName,
                        model.RecipientEmail), nameof(CreateNewGiftCertificate));
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(CreateNewGiftCertificate));
                    return Guid.Empty;
                }

                return model.PackageDealId; 

                // End using
            }
        }

        public async System.Threading.Tasks.Task<HttpStatusCode> SendPackageEmailToRecipient(PackageDealModel model)
        {

            const string subject = "Your AWI Pre-Purchased Package";
            if (model != null)
            {
                string plainTextContent = @"Congratulations!
                                    " + model.RecipientName +
                                          @", we know you'll love the experience in store for you at AWI!"
                                          + "Your email service does not support HTML Rendered barcodes. Please call (520) 222-6621 to schedule your reservation. REF ID: " + model.PackageDealId 
                                          + @"We're located at 1802 W Grant Rd, #107, between I-10 and Silverbell, right next to Sherwin-Williams.
                                    If you are bringing your own gun, please make sure you are not bringing any ammo!
                                    We do not allow live ammunition in our facility, for safety reasons. Please bring your weapon in the locked-open position with NO AMMUNITION in your weapon or on your person.
                                    Your time starts at your scheduled time slot! Please do not be late. We cannot compensate you for time lost, and we cannot extend your session in order to be fair to others who may have booked the slot after you. Any time missed is time lost.
                                    NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date.      
                                    GROUPON CUSTOMERS: Please bring proof of purchase with you to your appointment. Failure to provide proof of purchase may result in our inability to provide service. 
                                    Please do not replay to this email. We will miss your message!                                
                                    ";


                string htmlContent = " <strong>Congratulations!</strong>"
                                     + "<br /> <br />"
                                     + model.RecipientName +
                                     @", we know you'll love the experience in store for you at AWI! Here's your package information!"
                                     + "<br /> <br /> "
                                     + "<img id=\"barcode\" src=\"data: image / png; base64,@(" + Convert.ToBase64String(BarcodeWriter.CreateBarcode(model.PackageDealId.ToString(), BarcodeWriterEncoding.Code128).ResizeTo(300, 100).ToPngBinaryData()) + ")\" />< br />" + model.PackageDealId.ToString()
                                     + "<br /> <br />"
                                     + model.PackageType
                                     + "<br /> <br />"
                                     + model.PackageDescription
                                     + "<br /> <br />"
                                     + "HOW TO BOOK: <br />"
                                     + "Visit our website and book a session. In the \"Discount Code\" field, use the code \"GC\", This will tell the system that you are booking with a pre-pruchased package and allow you to present your barcoded ID number (above) at the time of arrival. "
                                     + "<br /> "
                                     + "<br /> <br />"
                                     + @"We're located at 1802 W Grant Rd, #107, between I-10 and Silverbell, right next to Sherwin-Williams. <a href=""https://goo.gl/maps/C8Cgi7iDp6gfDnyA9"">Google Maps </a> 
                                <br /> <br /> 
                                <strong> If you are bringing your own gun, please make sure you are not bringing any ammo! </strong> 
                                <br /> We do not allow live ammunition in our facility, for safety reasons. Please bring your weapon in the locked-open position with NO AMMUNITION in your weapon or on your person. <br /> 
                                <br /> 
                                <strong> Your time starts at your scheduled time slot! </strong> Please do not be late. We cannot compensate you for time lost, and we cannot extend your session in order to be fair to others who may have booked the slot after you. Any time missed is time lost. <br />
                                <br />                               
                                <strong> NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! </strong> - With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date.
                                <strong> GROUPON CUSTOMERS: Please bring proof of purchase with you to your appointment. Failure to provide proof of purchase may result in our inability to provide service. </strong> <br /> 
                                <br />
                                Please do not reply to this email. It is unmonitored. We will miss your message!   
                              ";


                var response = Utility.SendReservationMessage(model.RecipientEmail, subject, plainTextContent, htmlContent, null, null);

                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User {0} has been sent a confirmation email to {1}. ", model.RecipientName, model.RecipientEmail), nameof(SendPackageEmailToRecipient));

                return response.StatusCode;
            }

            return HttpStatusCode.BadRequest;
        }

        public async System.Threading.Tasks.Task<HttpStatusCode> SendPackageEmailToPurchaser(PackageDealModel model)
        {
            const string subject = "Your AWI Pre-Purchased Package";
            if (model != null)
            {
                string plainTextContent = @"Thanks
                                    " + model.PurchaserFirstName +
                                          @"for purchasing a simulation experience package. We know you'll love the experience in store at AWI!"
                                          + "There is a slight problem. It does not appear your email service supports HTML Emails. We require HTML for rendered barcodes. But all is not lost!"
                                          + "Please call (520) 222-6621 to schedule your reservation. REF ID: " + model.PackageDealId
                                          + "Please do not reply to this email. We will miss your message!";

                string htmlContent = " <strong>Congratulations!</strong>"
                                     + "<br /> <br />"
                                     + model.RecipientName +
                                     @", we know you'll love the experience in store for you at AWI! Here's your package information!"
                                     + "<br /> <br />"
                                     + "<img id=\"barcode\" src=\"data: image / png; base64,@(" + Convert.ToBase64String(BarcodeWriter.CreateBarcode(model.PackageDealId.ToString(), BarcodeWriterEncoding.Code128).ResizeTo(300, 100).ToPngBinaryData()) + ")\" />< br />" + model.PackageDealId.ToString()
                                     + "<br /> <br />"
                                     + model.PackageType
                                     + "<br /> <br />"
                                     + model.PackageDescription
                                     + "<br /> <br />"
                                     + "HOW TO BOOK: <br />"
                                     + "Visit our website and book a session. In the \"Discount Code\" field, use the code \"GC\", This will tell the system that you are booking with a pre-pruchased package and allow you to present your barcoded ID number (above) at the time of arrival. "
                                     + "<br /> "
                                     + "<br /> <br />"
                                     + @"We're located at 1802 W Grant Rd, #107, between I-10 and Silverbell, right next to Sherwin-Williams. <a href=""https://goo.gl/maps/C8Cgi7iDp6gfDnyA9"">Google Maps </a> 
                                        <br /> <br /> 
                                        <strong> If you are bringing your own gun, please make sure you are not bringing any ammo! </strong> 
                                        <br /> We do not allow live ammunition in our facility, for safety reasons. Please bring your weapon in the locked-open position with NO AMMUNITION in your weapon or on your person. <br /> 
                                        <br /> 
                                        <strong> Your time starts at your scheduled time slot! </strong> Please do not be late. We cannot compensate you for time lost, and we cannot extend your session in order to be fair to others who may have booked the slot after you. Any time missed is time lost. <br />
                                        <br />                               
                                        <strong> NO REFUNDS, NO RESCHEDULES INSIDE 24 HOURS! </strong> - With 24 hours notice on a future reservation, we can attempt to accommodate rescheduling to any date in the future of the reservation date.
                                        <strong> GROUPON CUSTOMERS: Please bring proof of purchase with you to your appointment. Failure to provide proof of purchase may result in our inability to provide service. </strong> <br /> 
                                        <br />
                                        Please do not reply to this email. It is unmonitored. We will miss your message!   
                                      ";


               var response = Utility.SendReservationMessage(model.PurchaserEmailAddress, subject, plainTextContent, htmlContent, null, null);

                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User {0} has been sent a confirmation email to {1}. ", model.RecipientName, model.RecipientEmail), nameof(SendPackageEmailToRecipient));

                return response.StatusCode;
            }

            return HttpStatusCode.BadRequest;
        }

    }
}