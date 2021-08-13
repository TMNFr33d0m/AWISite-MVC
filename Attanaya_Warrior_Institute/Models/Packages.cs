using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IronBarCode;

namespace Attanaya_Warrior_Institute.Models
{
    public class Packages
    {
        #region
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public decimal PackagePrice { get; set; }
        #endregion  

        public static Packages GetPackageById(int packageId)
        {
            Packages package = new Packages();

            string queryString = "SELECT * FROM dbo.Packages WHERE PackageId = '" + packageId + "'";

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
                            package.PackageId = (int)reader["PackageId"];
                            package.PackageName = (string)reader["PackageName"];
                            package.PackageDescription = (string)reader["PackageDescription"];
                            package.PackagePrice = (decimal)reader["PackagePrice"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetPackageById));
                    throw;
                }
            }


            return package;
        }

        public static List<Packages> GetPackageSet()
        {
            List<Packages> packageSet = new List<Packages>();

            string queryString = "SELECT * FROM dbo.Packages";

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
                            Packages package = new Packages();

                            package.PackageId = (int)reader["PackageId"];
                             package.PackageName = (string)reader["PackageName"];
                            package.PackageDescription = (string)reader["PackageDescription"];
                            package.PackagePrice = (decimal)reader["PackagePrice"];

                            packageSet.Add(package);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingModel.LogCriticalException(ex, nameof(GetPackageById));
                    throw;
                }
            }


            return packageSet;
        }

        public static Dictionary<int, string> SelectedPackageHelper()
        {
            IEnumerable<Packages> records = GetPackageSet();

            return records.ToDictionary(record => record.PackageId, record => record.PackageName);

        }
        
        public static HttpStatusCodeResult CreateNewPackage(Packages model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var connection = new SqlConnection(Utility.ConnectionString))
            {
                string queryString1 = "INSERT INTO dbo.Packages(" +
                                      "[PackageId], " +
                                      "[PackageName], " +
                                      "[PackageDescription], " +
                                      "[PackagePrice], " +
                                      ") VALUES('" +
                                      model.PackageId + "','" +
                                      model.PackageName + "','" +
                                      model.PackageDescription + "','" +
                                      model.PackagePrice + "'"
                                      + ")";

                var command = new SqlCommand(queryString1, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(CreateNewPackage));
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
                // End using
            }
        }

        public static Image ConvertByteArrayToImage(Guid modelGiftCertificateNumber)
        {
            using (var ms = new MemoryStream(BarcodeWriter
                .CreateBarcode("https://ironsoftware.com/csharp/barcode", BarcodeWriterEncoding.Code128)
                .ToPngBinaryData()))
            {
                return Image.FromStream(ms);
            }
        }

    }
}