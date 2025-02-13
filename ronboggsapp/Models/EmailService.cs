using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ronboggsapp.Models
{
   
    public class EmailService
    {
        public static void SendingMail(string recipientEmail, string fullName,string Phone, ZoomMeetingResponse meeting,bool WithoutJoining_URL)
        {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(EnvirontmentVariable.DecryptString(SMTP_PROTOCOL.SENDER));  // Your email
                mail.To.Add(recipientEmail); // Receiver Email
                mail.Subject = "Zoom Meeting Confirmation";

                // Email body with dynamic details
                mail.Body = $"<h2>Zoom Meeting Confirmation</h2>" +
                            $"<p>Dear {fullName},</p>" +
                            $"<p>Thank you for scheduling a meeting. Here are your meeting details:</p>" +
                            $"<ul>" +
                            $"<li><b>Date & Time:</b> {meeting.start_time}</li>" +
                            $"<li><b>Meeting Link:</b> <a href='"+ meeting.join_url + "'>Join Meeting</a></li>" +
                            $"</ul>" +
                            $"<p>Looking forward to our meeting!</p>" +
                            $"<br/><p>Best Regards,</p>" +
                            $"<p>Your Company Name</p>";
                mail.IsBodyHtml = true;

                // SMTP client configuration
                SmtpClient smtp = new SmtpClient(SMTP_PROTOCOL.SMTP, SMTP_PROTOCOL.PORT);
                smtp.Credentials = new NetworkCredential(EnvirontmentVariable.DecryptString(SMTP_PROTOCOL.SENDER), EnvirontmentVariable.DecryptString(SMTP_PROTOCOL.PASSWORD)); // Use an App Password for security
                smtp.EnableSsl = true;
                // Send email
                smtp.Send(mail);
        }
        public static void Contacting(string cEmail, string cPhone, string cMessage, string cFullName)
        {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(cEmail);
                mail.To.Add(EnvirontmentVariable.DecryptString(SMTP_PROTOCOL.SENDER));
                mail.Subject = "New Contact Form Submission";
                mail.Body = $"<strong>Full Name:</strong> {cFullName} <br/>" +
                            $"<strong>Email:</strong> {cEmail} <br/>" +
                            $"<strong>Phone:</strong> {cPhone} <br/>" +
                            $"<strong>Message:</strong> {cMessage}";
                mail.IsBodyHtml = true;

                // SMTP Configuration (Using Gmail)
                SmtpClient smtp = new SmtpClient(SMTP_PROTOCOL.SMTP, SMTP_PROTOCOL.PORT);
                smtp.Credentials = new NetworkCredential(EnvirontmentVariable.DecryptString(SMTP_PROTOCOL.SENDER), EnvirontmentVariable.DecryptString(SMTP_PROTOCOL.PASSWORD));  // Owner's Email Login
                smtp.EnableSsl = true;
                smtp.Send(mail);
        }
    }
}
