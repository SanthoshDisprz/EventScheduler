using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.DataAccess;
using DisprzTraining.Data;
// using Appointments;
using DisprzTraining.Models;
using MimeKit;
// using System.Net.Mail;
using MailKit.Net.Smtp;

namespace DisprzTraining.Business
{
    public class AppointmentsBL : IAppointmentsBL
    {
        private readonly IAppointmentsDAL _appointmentsDAL;
        public AppointmentsBL(IAppointmentsDAL appointmentsDAL)
        {
            _appointmentsDAL = appointmentsDAL;
        }
        //In GET call, convert ISO time to user's local time
        public List<Appointment> ConvertToLocalTime(List<Appointment> appointmentList, int timeZoneOffset)
        {
            List<Appointment> appointmentInLocalTimeStamp = new List<Appointment>();
            foreach (var appointment in appointmentList)
            {
                var updatedAppointment = new Appointment();
                updatedAppointment.Id = appointment.Id;
                updatedAppointment.Title = appointment.Title;
                updatedAppointment.StartTime = appointment.StartTime.Value.AddMinutes(-timeZoneOffset);
                updatedAppointment.EndTime = appointment.EndTime.Value.AddMinutes(-timeZoneOffset);
                updatedAppointment.Description = appointment.Description;
                updatedAppointment.CreatedBy = appointment.CreatedBy;
                updatedAppointment.GuestsList = appointment.GuestsList;
                updatedAppointment.Location = appointment.Location;
                appointmentInLocalTimeStamp.Add(updatedAppointment);

            }
            return appointmentInLocalTimeStamp;
        }

        public List<Appointment> GetAppointments(DateTime? startTime, DateTime? endTime, int timeZoneOffset)
        {
            if (startTime > endTime) throw new Exception("Start time should be lesser than End time");

            else if (startTime == null || endTime == null) throw new Exception("Both Start time and End time are mandatory for getting appointments");
            else if (startTime.Value == endTime.Value) throw new Exception("Start time and End time should not be equal");

            var appointmentsList = _appointmentsDAL.GetAppointments(startTime.Value, endTime.Value);
            var appointmentsInLocalTime = ConvertToLocalTime(appointmentsList, timeZoneOffset);
            return appointmentsInLocalTime;
        }

        public bool AppointmentConflictCheck(AddAppointment appointment)
        {
            var allAppointments = _appointmentsDAL.GetAllAppointments();

            bool isConflict = false;
            if (allAppointments.Any())
            {
                foreach (var meet in allAppointments)
                {
                    if ((meet.StartTime < appointment.EndTime) && (appointment.StartTime < meet.EndTime))
                    {
                        isConflict = true;
                    }
                }
            }

            return isConflict;
        }
        public bool IsValidMailId(string emailId)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(emailId);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public bool SendEmail(List<string> guestsList, string? title, DateTime? startTime, DateTime? endTime)
        {
            bool isMailSent = false;
            foreach (var guestEmail in guestsList)
            {
                bool isValidMailId = IsValidMailId(guestEmail);
                if (isValidMailId)
                {
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("santhoshtirumeni@gmail.com"));
                    email.To.Add(MailboxAddress.Parse(guestEmail));
                    email.Subject = title;
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = $"You have been invited to an appointment. Appointment Starts at {startTime.ToString()}(UTC) and Ends at {endTime.ToString()}(UTC)" };
                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate("santhoshtirumeni@gmail.com", "kbrgjzcbejuvtrhu");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    isMailSent = true;
                }
            }
            return isMailSent;
        }
        public bool CreateAppointment(AddAppointment appointment)
        {
            var currentTime = DateTime.UtcNow.AddMinutes(-1);
            if (appointment.StartTime > appointment.EndTime) throw new Exception("End time should be greater than Start time");
            else if (appointment.StartTime < currentTime) throw new Exception("Cannot create appointment for past time");
            else if (appointment.StartTime == null || appointment.EndTime == null) throw new Exception("Both Start time and End time are mandatory for creating appointments");
            else if (appointment.StartTime.Value == appointment.EndTime.Value) throw new Exception("Appointment Start time and End time should not be same");
            var isConflict = AppointmentConflictCheck(appointment);
            if (isConflict)
            {
                return false;
            }
            if (appointment.GuestsList != null)
            {
                SendEmail(appointment.GuestsList, appointment.Title, appointment.StartTime, appointment.EndTime);
            }
            return _appointmentsDAL.CreateAppointment(appointment);
        }

        public bool DeleteAppointment(Guid id)
        {
            bool isValidAppointmentId = false;
            var appointment = _appointmentsDAL.GetAppointmentById(id);
            var currentDateAndTime = DateTime.UtcNow;
            var appointmentStartDateAndTime = appointment?.StartTime;
            if (currentDateAndTime > appointmentStartDateAndTime)
            {
                throw new Exception("Cannot Delete Older Appointments");
            }
            if (appointment != null)
            {
                _appointmentsDAL.DeleteAppointment(appointment);
                isValidAppointmentId = true;
            }
            return isValidAppointmentId;
        }
        public bool UpdateAppointmentConflictCheck(Guid id, AddAppointment appointment)
        {
            var allAppointments = _appointmentsDAL.GetAllAppointments();
            if (allAppointments.Any())
            {
                foreach (var meet in allAppointments)
                {
                    if (id == meet.Id) { continue; }
                    if ((meet.StartTime < appointment.EndTime) && (appointment.StartTime < meet.EndTime))
                    {
                        return true;
                    }

                }
            }
            return false;

        }
        public bool UpdateAppointment(Guid id, AddAppointment appointment)
        {
            var currentDateAndTime = DateTime.UtcNow.AddMinutes(-1);
            var appointmentToBeUpdated = _appointmentsDAL.GetAppointmentById(id);
            if (appointmentToBeUpdated == null)
            {
                return false;
            }

            else if (appointmentToBeUpdated.StartTime.Value < currentDateAndTime) throw new Exception("Cannot Update Past Appointment");
            else if (appointment.StartTime < currentDateAndTime)
            {
                throw new Exception("Cannot Update Appointment to Past time");
            }
            else if (appointment.StartTime > appointment.EndTime) throw new Exception("End time should be greater than Start time");
            else if (appointment.StartTime == null || appointment.EndTime == null) throw new Exception("Both Start time and End time are mandatory for updating appointments");
            else if (appointment.StartTime.Value == appointment.EndTime.Value) throw new Exception("Appointment Start time and End time should not be same");
            if (appointment.GuestsList != null)
            {
                SendEmail(appointment.GuestsList, appointment.Title, appointment.StartTime, appointment.EndTime);
            }
            return _appointmentsDAL.UpdateAppointment(appointmentToBeUpdated, appointment);

        }
        public PaginatedResponse GetAppointmentsByTitle(string? title, int pageNumber, int pageSize, int timeZoneOffset)
        {
            if (title == null)
            {
                throw new Exception("Title cannot be null");
            }
            var result = _appointmentsDAL.GetAppointmentsByTitle(title);
            if (result.Any())
            {
                var paginatedResult = result.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
                var appointmentsInLocalTime = ConvertToLocalTime(paginatedResult, timeZoneOffset);
                var totalPages = (int)Math.Ceiling(result.Count() / (double)pageSize);
                var currentPage = pageNumber;
                var paginatedResponse = new PaginatedResponse()
                {
                    TotalPages = totalPages,
                    Appointments = appointmentsInLocalTime

                };

                return paginatedResponse;
            }
            return new PaginatedResponse();

        }


    }
}








