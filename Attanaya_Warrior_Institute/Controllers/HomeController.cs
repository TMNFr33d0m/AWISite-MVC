using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Attanaya_Warrior_Institute.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Attanaya_Warrior_Institute.Controllers
{
    public class HomeController : Controller
    {

        #region View Code

        public ActionResult Index()
        {

            string ip = Request?.UserHostAddress;

            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip), nameof(Index), ip);
                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Index)), nameof(Index), ip);

            if (ip != null && Session.IsNewSession)
            {

                string hostName = "";

                // Try to resolve hostname from IP. This can go wrong if the hostname is hidden or unavailable for whatever reason, and throws an un-necessary error if it does, so we handle it gracefully. 
                try
                {
                    IPHostEntry hostAddressConversion = Dns.GetHostEntry(ip);
                    hostName = hostAddressConversion.HostName;
                }
                catch (Exception e)
                {
                    LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "An error occurred while trying to resolve the hostname of the client. The error was {0}", e), nameof(Index), ip);
                    hostName = "not found";
                }

                // Try to write the record to the DB. This shouldn't go wrong, and will return the error page with the error code for debugging.
                try
                {
                    WebsiteVisitorUtil visitorUtil = new WebsiteVisitorUtil(Utility.ConnectionString);
                    visitorUtil.WriteVisitorRecord(ip, hostName);
                }
                catch (Exception e)
                {
                    LoggingModel.LogCriticalException(e, nameof(Index), ip);
                    return View("Error");
                }
            }

            return View();
        }

        public ActionResult ClassBookingReceipt(ClassBookingReceiptModel model)
        {
            return View(model);
        }

        public ActionResult Faq()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Faq), ip);
                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Faq)), nameof(Faq), ip);
             

            return View();

        }

        public ActionResult Rules()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Rules), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Rules)), nameof(Rules), ip);

            return View();
        }

        public ActionResult Byog()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Byog), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Byog)), nameof(Byog), ip);

            return View();
        }

        public ActionResult AzSecforLaws()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Byog), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(AzSecforLaws)), nameof(AzSecforLaws), ip);

            return View();
        }

        public ActionResult Instructors()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Instructors));

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Instructors)), nameof(Instructors), ip);

            return View("InstructorPartnership");
        }

        public ActionResult Hours()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Hours));

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Hours)), nameof(Hours), ip);

            return View();
        }

        public ActionResult About()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Reservation), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Reservation)), nameof(Reservation), ip);

            return View();
        }

        public ActionResult Broken()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Reservation), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Reservation)), nameof(Reservation), ip);

            return View();
        }

        public ActionResult Classes()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Classes), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Classes)), nameof(Classes), ip);

            EduClassViewModel model = new EduClassViewModel();

            return View(model);
        }

        public ActionResult GiftCertificate(PackageDealModel model)
        {

            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Classes), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(GiftCertificate)), nameof(GiftCertificate), ip);

            if (model == null)
            {
                model = new PackageDealModel();
            }

            return View(model);
        }

        public ActionResult Memberships()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Faq), ip);
                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Memberships)), nameof(Memberships), ip);


            return View();

        }

        public HttpStatusCodeResult CreateMembership(string subscriptionID, string planId)
        {
            SubscriptionModel Sub = new SubscriptionModel
            {
                AccountID = Guid.Parse(User.Identity.GetUserId()),
                SubscriptionID = subscriptionID,
                SubscriptionType = planId,
                SubscriptionStartDate = Utility.GetCurrentTucsonTime()
            };

            try
            {
                var res = Sub.CreateNewSubscription(Sub);
                return new HttpStatusCodeResult(res.StatusCode);
            }
            catch (Exception ex)
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture,  "A problem occurred while creating a membership in the DB. AccountID {0}, SubID: {1}, SubType: {2}, SubDate {3} Message: {4} ", Sub.AccountID, Sub.SubscriptionID, Sub.SubscriptionType, Sub.SubscriptionStartDate, ex), nameof(CreateMembership));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }            
        }

        public ActionResult PackageReceipt(PackageDealModel model)
        {

            return View("PackageReceipt", model);
        }

        [Authorize]
        public ActionResult InternalUI()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            //ToDo: Add a role check to ensure only authorized employees can access InternalUI - right now anyone with an account can. 
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to the internal dashboard. Reason: UNAUTHORIZED IP", ip),
                    nameof(Dashboard), ip);

                return View("AccessDenied");
            }

            DashboardViewModel model = new DashboardViewModel();

            model.EBookings = EnhancedBookingStub.GetEStubbedReservationsAndClasses();
            model.ClosedDates = ClosedDates.GetClosedDates();
            model.Hours = HoursOfOperation.GetHoursOfOperation();
            model.Rooms = Rooms.GetAvailableRoomsInList();
            
            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(InternalUI)), nameof(InternalUI), ip);
            return View(model);
        }

        public PartialViewResult InternalUISchedulePartial()
        {
            var Model = new DashboardViewModel(); 

                Model.EBookings = EnhancedBookingStub.GetEStubbedReservationsAndClasses();
                Model.ClosedDates = ClosedDates.GetClosedDates();
                Model.Hours = HoursOfOperation.GetHoursOfOperation();
                Model.Rooms = Rooms.GetAvailableRoomsInList();

            return PartialView(Model);
        }

        [Authorize]
        public ActionResult ManageClasses(
            string instructorID = null,
            string classtitle = null,
            string startDateTime = null,
            string endDateTime = null,
            string description = null,
            int? mainRoom = null,
            int? secondaryRoom = null,
            string externalBookingLink = null,
            int? externalBookingSource = null,
            decimal? costPerStudent = null,
            int? maxAttendees = null
            )
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(ManageClasses), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(ManageClasses)), nameof(ManageClasses), ip);


            EduClass model = new EduClass
            {
                InstructorId = string.IsNullOrWhiteSpace(instructorID) ? Instructor.GetInstructorFromAccountId(Guid.Parse(User.Identity.GetUserId())).InstructorId : Guid.Parse(instructorID),
                ClassTitle = classtitle,
                StartDateTime = string.IsNullOrWhiteSpace(startDateTime) ? DateTime.Now.AddDays(1).Date.AddHours(DateTime.Now.AddHours(1).Hour) : DateTime.Parse(startDateTime, CultureInfo.CurrentCulture),
                EndDateTime = string.IsNullOrWhiteSpace(endDateTime) ? DateTime.Now.AddDays(1).Date.AddHours(DateTime.Now.AddHours(2).Hour) : DateTime.Parse(endDateTime, CultureInfo.CurrentCulture),
                Description = description,
                MainRoom = mainRoom != null ? (int)mainRoom : 0,
                SecondaryRoom = secondaryRoom != null ? (int)secondaryRoom : 0,
                ExternalBookingSource = externalBookingSource.HasValue ? Convert.ToBoolean(externalBookingSource, CultureInfo.InvariantCulture) : Convert.ToBoolean(bool.FalseString, CultureInfo.InvariantCulture),
                ExternalBookingLink = externalBookingLink,
                CostPerStudent = costPerStudent.HasValue ? (decimal)costPerStudent : 0,
                MaxAttendees = maxAttendees.HasValue ? (int)maxAttendees : 0
            };


            return View(model);
        }


        [Authorize]
        public ActionResult EditClasses(string EduClassId)
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(ManageClasses), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(ManageClasses)), nameof(ManageClasses), ip);

            var model = EduClass.GetEduClassById(Guid.Parse(EduClassId));

            return View("ManageClasses", model);
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Dashboard), ip);

                return View("AccessDenied");
            }

            EduClassViewModel model = new EduClassViewModel(Instructor.GetInstructorFromAccountId(Guid.Parse(User.Identity.GetUserId())));

            if (model.EduClasses.Count() == 0)
            {
                 model = new EduClassViewModel(Students.GetStudentIdFromAccountId(Guid.Parse(User.Identity.GetUserId())));
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Dashboard)), nameof(Dashboard), ip);
            return View(model);
        }

        [Authorize]
        public ActionResult BookClass(Guid EduClassID, Guid AccountID)
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Dashboard), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(BookClass)), nameof(BookClass), ip);

            ClassBookingViewModel model = new ClassBookingViewModel();

            try
            {
                var StudentID = Students.GetStudentIdFromAccountId(AccountID);

                model.Student = Students.GetStudentByStudentID(StudentID);
                model.EduClass = EduClass.GetEduClassById(EduClassID);
                model.Booking = new StudentBookings
                {
                    ReservationID = Guid.NewGuid(),
                    EduClassID = EduClassID,
                    StudentID = StudentID,
                    TimeStamp = DateTime.Now
                };
            }
            catch (Exception)
            {
                throw;
            }

            return View("ClassBooking", model);
        }

        // We make the app pass the information back to itself on refresh, so we preserve the data in the fields...
        public ActionResult Reservation(
            string reservationName = null,
            string reservationPhone = null,
            string reservationEmail = null,
            string reservationDate = null,
            string selectedReservationTime = null,
            int? reservationGuestCount = null,
            int? reservationDurationHrs = null,
            int? selectedReservationRoom = null,
            int? desiredExperience = null,
            string discountCode = null
            )
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Reservation), ip);

                return View("AccessDenied");
            }

            if (discountCode == null || discountCode == String.Empty)
            {
                if (DateTime.Now <  DateTime.Parse("2-1-2021 00:00:00.000", CultureInfo.InvariantCulture))
                {
                    discountCode = "protect";
                }
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Reservation)), nameof(Reservation), ip);

            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            Reservation bookingModel = new Reservation
            {
                ReservationName = reservationName,
                ReservationPhone = reservationPhone,
                ReservationEmail = reservationEmail,
                ReservationDate = reservationDate ?? currentTucsonDateTime.ToShortDateString(),
                SelectedReservationTime = selectedReservationTime.IsNullOrWhiteSpace() || selectedReservationTime.Contains("CLOSED") || selectedReservationTime.Contains("TODAY") || string.IsNullOrEmpty(selectedReservationTime) ? TimeSpan.Zero : TimeSpan.Parse(selectedReservationTime, CultureInfo.CurrentCulture),
                ReservationDurationHrs = reservationDurationHrs ?? 1,
                SelectedReservationRoom = selectedReservationRoom == null ? 0 : (int)selectedReservationRoom,
                DesiredExperience = desiredExperience == null ? 0 : (int)desiredExperience,
                DiscountCode = discountCode,
                AccountID = User.Identity.IsAuthenticated ? Guid.Parse(User.Identity.GetUserId()) : Guid.Empty

            };

            return View("Reservation", bookingModel);
        }

        public ActionResult TopSecret()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(TopSecret));

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(TopSecret)), nameof(TopSecret), ip);

            return View();
        }

        public ActionResult Waiver()
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(Faq), ip);
                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(Faq)), nameof(Faq), ip);


            return View();

        }


        #endregion

        #region Utility Code 


        [HttpGet]
        public HttpStatusCodeResult SaveStudentBooking(
        string eduClassID,
        string studentID,
        string firstName,
        string middleName,
        string lastName,
        string emailAddress,
        string phoneNumber,
        string classTitle,
        string classTimeStart,
        string classTimeEnd,
        int classRoom,
        int classRoomAlt,
        string maxAttendees,
        string totalMoney
        )
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            StudentBookings model = new StudentBookings
            {
                StudentID = Guid.Parse(studentID),
                ReservationID = Guid.NewGuid(),
                EduClassID = Guid.Parse(eduClassID),
                TimeStamp = Utility.GetCurrentTucsonTime()
            };

            EduClass eduClass = new EduClass();
             Students students = new Students();

            eduClass.EduClassId = model.EduClassID;
            eduClass.ClassTitle = classTitle;
            eduClass.CostPerStudent = decimal.Parse(totalMoney);
            eduClass.Description = "OMITTED FOR LENGTH";
            eduClass.EndDateTime = DateTime.Parse(classTimeEnd);
            eduClass.StartDateTime = DateTime.Parse(classTimeStart);
            eduClass.MainRoom = classRoom;
            eduClass.SecondaryRoom = classRoomAlt;
            eduClass.MaxAttendees = int.Parse(maxAttendees);

            students.StudentID = Guid.Parse(studentID);
            students.FirstName = firstName;
            students.MiddleName = middleName;
            students.LastName = lastName;
            students.EmailAddress = emailAddress;
            students.PhoneNumber = phoneNumber;

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "{0} {1} {2} (Student ID: {3}) submitted a booked {4} (Class ID: {5})",
                firstName,
                middleName,
                lastName,
                model.StudentID,
                eduClass.ClassTitle,
                eduClass.EduClassId
                ),
                    nameof(SaveStudentBooking), ip);

            StudentBookings.SaveBooking(model);
            StudentBookings.SendClassBookingConfirmationEmail(eduClass, students).ConfigureAwait(true);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult BookingReceiptAsync
        (
            string reservationName = null,
            string reservationPhone = null,
            string reservationEmail = null,
            string reservationDate = null,
            string selectedReservationTime = null,
            int? reservationGuestCount = null,
            int? reservationDurationHrs = null,
            int? selectedReservationRoom = null,
            int? desiredExperience = null,
            string discountCode = null,
            string totalMoney = null)
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(BookingReceiptAsync), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} accessed the {1} page.", ip, nameof(BookingReceiptAsync)), nameof(BookingReceiptAsync));

            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            Reservation bookingModel = new Reservation
            {
                ReservationName = reservationName,
                ReservationPhone = reservationPhone,
                ReservationEmail = reservationEmail,
                ReservationDate = reservationDate ?? currentTucsonDateTime.ToShortDateString(),
                ReservationGuestCount = reservationGuestCount ?? 1,
                SelectedReservationTime = selectedReservationTime.IsNullOrWhiteSpace() || selectedReservationTime.Contains("CLOSED") || selectedReservationTime.Contains("TODAY") ? TimeSpan.Zero : TimeSpan.Parse(selectedReservationTime, CultureInfo.CurrentCulture),
                ReservationDurationHrs = reservationDurationHrs ?? 1,
                SelectedReservationRoom = selectedReservationRoom == null ? 0 : (int)selectedReservationRoom,
                DesiredExperience = desiredExperience ?? 1,
                DiscountCode = discountCode,
                TotalMoney = totalMoney
            };

            _ = bookingModel.SendConfirmationEmail(bookingModel);

            return View("BookingReceipt", bookingModel);
        }

        [HttpGet]
        public ActionResult BookingReceiptAsyncInsertFailed
        (
            string reservationName = null,
            string reservationPhone = null,
            string reservationEmail = null,
            string reservationDate = null,
            string selectedReservationTime = null,
            int? reservationGuestCount = null,
            int? reservationDurationHrs = null,
            int? selectedReservationRoom = null,
            int? desiredExperience = null,
            string discountCode = null,
            string totalMoney = null)
        {
            string ip = Request?.UserHostAddress;
            string[] bannedIPs = ConfigurationManager.AppSettings["bannedIP"].Split(';');

            // Prevent banned IP addresses from loading site...
            if (bannedIPs.Contains(ip))
            {
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User with IP address of {0} was denied access to website. Reason: BANNED IP", ip),
                    nameof(BookingReceiptAsyncInsertFailed), ip);

                return View("AccessDenied");
            }

            LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "PROBLEM: User with IP address of {0} accessed the {1} page in contingency mode.", ip, nameof(BookingReceiptAsyncInsertFailed)), nameof(BookingReceiptAsyncInsertFailed), ip);
            
            var currentTucsonDateTime = Utility.GetCurrentTucsonTime();

            Reservation bookingModel = new Reservation
            {
                ReservationName = reservationName,
                ReservationPhone = reservationPhone,
                ReservationEmail = reservationEmail,
                ReservationDate = reservationDate ?? currentTucsonDateTime.ToShortDateString(),
                ReservationGuestCount = reservationGuestCount ?? 1,
                SelectedReservationTime = selectedReservationTime.IsNullOrWhiteSpace() || selectedReservationTime.Contains("CLOSED") || selectedReservationTime.Contains("TODAY") ? TimeSpan.Zero : TimeSpan.Parse(selectedReservationTime, CultureInfo.CurrentCulture),
                ReservationDurationHrs = reservationDurationHrs ?? 1,
                SelectedReservationRoom = selectedReservationRoom == null ? 0 : (int)selectedReservationRoom,
                DesiredExperience = desiredExperience ?? 1,
                DiscountCode = discountCode,
                TotalMoney = totalMoney
            };

            try
            {
                SaveBooking(bookingModel);
                bookingModel.SendConfirmationEmail(bookingModel);
                bookingModel.SendNewBookingEmail(bookingModel);
                LoggingModel.LogMessage(string.Format(CultureInfo.CurrentCulture, "User {0} successfully saved the booking after entering contingency mode.", ip), nameof(BookingReceiptAsyncInsertFailed), ip);

            }
            catch (Exception e)
            {
                bookingModel.SendConfirmationEmail(bookingModel);
                bookingModel.SendNewBookingEmailWithFailureAlert(bookingModel);
                LoggingModel.LogCriticalException(e, nameof(BookingReceiptAsyncInsertFailed), ip);
            }
            
            return View("BookingReceipt", bookingModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public HttpStatusCodeResult SaveBooking(
            string reservationName,
            string reservationPhone,
            string reservationEmail,
            int reservationGuestCount,
            string reservationDate,
            string selectedReservationTime,
            int reservationDurationHrs,
            int selectedReservationRoom,
            int desiredExperience,
            string discountCode,
            string totalMoney
        )
        {

            var accountID = Guid.Empty;

            if (User.Identity.IsAuthenticated){
                accountID = Guid.Parse(User.Identity.GetUserId());
            }

            Reservation bookingModel = new Reservation
            {
                ReservationName = reservationName,
                ReservationPhone = reservationPhone,
                ReservationEmail = reservationEmail,
                ReservationGuestCount = reservationGuestCount,
                ReservationDate = reservationDate,
                SelectedReservationTime = TimeSpan.Parse(selectedReservationTime, CultureInfo.CurrentCulture),
                ReservationDurationHrs = reservationDurationHrs,
                SelectedReservationRoom = selectedReservationRoom,
                DesiredExperience = desiredExperience,
                DiscountCode = discountCode,
                TotalMoney = totalMoney,
                AccountID = accountID

            };

            try
            {
                bookingModel.SaveBooking(bookingModel);
                Discounts.UpdateDiscountCount(bookingModel.DiscountCode);
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch(Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(SaveBooking));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        public HttpStatusCodeResult SaveBooking(Reservation bookingModel)
        {
            try
            {
                bookingModel.AccountID = Guid.Empty; 

                if (User.Identity.IsAuthenticated)
                {
                    bookingModel.AccountID = Guid.Parse(User.Identity.GetUserId());
                }

                bookingModel?.SaveBooking(bookingModel);
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(SaveBooking));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }
        
        [HttpGet]
        public ActionResult GetDiscount (string discountCode)
        {
            var payload = Utility.GetDiscounts(discountCode);
            return Json(payload, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMaxHoursFromSelectedTime(string selectedDate, string selectedStartTime, int simulationRoomID)
        {
            if (selectedStartTime != "CLOSED")
            {
                var timeSlots = Models.Reservation.GetAvailableTimes(DateTime.Parse(selectedDate, CultureInfo.CurrentCulture), simulationRoomID);
                var activeTimeSlots = timeSlots.Where(slot => TimeSpan.Parse(slot.StartTimeSlot, CultureInfo.CurrentCulture) >= TimeSpan.Parse(selectedStartTime, CultureInfo.CurrentCulture));
                var activeTimeSlotCount = activeTimeSlots.Count();

                return Json(activeTimeSlotCount, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var activeTimeSlotCount = 0;
                return Json(activeTimeSlotCount, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetGiftCertificate(
            string PackageType, 
            string PurchaserFirstName, 
            string PurchaserMiddleName, 
            string PurchaserLastName, 
            string PurchaserEmailAddress, 
            string PurchaserPhoneNumber, 
            string RecipientName, 
            string RecipientPhone, 
            string RecipientEmail
            )
        {
            PackageDealModel model = new PackageDealModel
            {
                PackageType = int.Parse(PackageType, CultureInfo.InvariantCulture),
                PurchaserFirstName = PurchaserFirstName,
                PurchaserMiddleName = PurchaserMiddleName,
                PurchaserLastName = PurchaserLastName,
                PurchaserEmailAddress = PurchaserEmailAddress,
                PurchaserPhoneNumber = PurchaserPhoneNumber,
                RecipientName = RecipientName,
                RecipientPhone = RecipientPhone,
                RecipientEmail = RecipientEmail
            };
            
            var result = PackageDealModel.CreateNewGiftCertificate(model);

            return Json(result, JsonRequestBehavior.AllowGet); ;
        }

        [HttpGet]
        public ActionResult GetPackageFromId(int id)
        {
            return Json(JsonConvert.SerializeObject(Packages.GetPackageById(id)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetExperienceFromId(int id)
        {
            return Json(JsonConvert.SerializeObject(Experience.GetExperienceByID(id)), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public HttpStatusCodeResult CreateNewClass(
            string instructorID = null,
            string classtitle = null,
            string startDateTime = null,
            string endDateTime = null,
            string description = null,
            int? mainRoom = null,
            int? secondaryRoom = null,
            string externalBookingLink = null,
            int? externalBookingSource = null,
            decimal? costPerStudent = null,
            int? maxAttendees = null)
        {
            try
            {
                var Model = new EduClass
                {
                    InstructorId = Guid.Parse(instructorID),
                    ClassTitle = classtitle,
                    StartDateTime = DateTime.Parse(startDateTime, CultureInfo.CurrentCulture),
                    EndDateTime = DateTime.Parse(endDateTime, CultureInfo.CurrentCulture),
                    Description = description,
                    MainRoom = mainRoom != null ? (int)mainRoom : 0,
                    SecondaryRoom = secondaryRoom != null ? (int)secondaryRoom : 0,
                    ExternalBookingLink = externalBookingLink ?? "#",
                    ExternalBookingSource = Convert.ToBoolean(externalBookingSource, CultureInfo.CurrentCulture),
                    CostPerStudent = (decimal)costPerStudent,
                    MaxAttendees = (int)maxAttendees

                };
                
                var result = Model.CreateNewClass(Model); 

                return result;
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(CreateNewClass));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpStatusCodeResult UpdateClass(
           string InstructorID = null,
           string EduClassID = null,
           string classtitle = null,
           string startDateTime = null,
           string endDateTime = null,
           string description = null,
           int? mainRoom = null,
           int? secondaryRoom = null,
           string externalBookingLink = null,
           int? externalBookingSource = null,
           decimal? costPerStudent = null,
           int? maxAttendees = null)
        {
            try
            {
                var Model = new EduClass
                {
                    InstructorId = Guid.Parse(InstructorID),
                    EduClassId = Guid.Parse(EduClassID),
                    ClassTitle = classtitle,
                    StartDateTime = DateTime.Parse(startDateTime, CultureInfo.CurrentCulture),
                    EndDateTime = DateTime.Parse(endDateTime, CultureInfo.CurrentCulture),
                    Description = description,
                    MainRoom = mainRoom != null ? (int)mainRoom : 0,
                    SecondaryRoom = secondaryRoom != null ? (int)secondaryRoom : 0,
                    ExternalBookingLink = externalBookingLink ?? string.Empty,
                    ExternalBookingSource = externalBookingLink.Length > 2 ? true : false,
                    CostPerStudent = (decimal)costPerStudent,
                    MaxAttendees = (int)maxAttendees

                };

                var result = Model.UpdateClass(Model);

                return result;
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(UpdateClass));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        public HttpStatusCodeResult DeleteClass(string EduClassID = null)
        {
            try
            {
                EduClass.DeleteClass(EduClassID);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(DeleteClass));
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        public JsonResult CheckEduClassScheduleAvailability(DateTime CurrentClassStartTime, DateTime CurrentClassEndTime, int MainRoomID, int SecondaryRoomID, string eduClassId)
        {
            var result = EduClass.CheckScheduleAvailability(CurrentClassStartTime, CurrentClassEndTime, MainRoomID, SecondaryRoomID, eduClassId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCodeResult SaveWaiver(WaiverClass waiver)
        {
            var res = HttpStatusCode.OK;
            try
            {
                waiver.SignatureDate = Utility.GetCurrentTucsonTime();
                res = (HttpStatusCode)WaiverClass.SaveWaiver(waiver).StatusCode;
            }
            catch (Exception e)
            {
                LoggingModel.LogCriticalException(e, nameof(SaveWaiver));
                res = HttpStatusCode.BadRequest;
            }

            return new HttpStatusCodeResult(res);
        }

        #endregion

    }
}