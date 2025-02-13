using Org.BouncyCastle.Cms;
using ronboggsapp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Org.BouncyCastle.Utilities.Net;
using System.Security.Cryptography;
using System.Data;
using System.Globalization;
using System.Web.Helpers;

namespace ronboggsapp.Controllers
{
    public class HomeController : Controller
    {
        private MYSQL_Connection_Server MYSQL;
        public ActionResult Index()
        {

            Session["CurrentPage"] = "Home";
            try
            {
                ViewBag.msg = "Success";
            }
            catch (Exception ex)
            {
                ViewBag.msg = "Failed:" + ex.Message;
            }
            return View();
        }
        public ActionResult About()
        {
            Session["CurrentPage"] = "About";
            ViewBag.Message = "About";

            return View();
        }
        public ActionResult Programs()
        {
            Session["CurrentPage"] = "Programs";
            ViewBag.Message = "Program";

            return View();
        }
        public ActionResult Contact()
        {
            Session["CurrentPage"] = "Contact";
            ViewBag.Message = "contact";

            return View();
        }
        public ActionResult Life_Coaching()
        {
            Session["CurrentPage"] = "Home";
            ViewBag.Message = "Life_Coaching";

            return View();
        }
        public ActionResult Personal_Growth()
        {
            Session["CurrentPage"] = "Home";
            ViewBag.Message = "Personal_Growth";

            return View();
        }
        public ActionResult Business_Coaching()
        {
            Session["CurrentPage"] = "Home";
            ViewBag.Message = "Business Coaching";

            return View();
        }
        [HttpPost]
        public JsonResult SendEmail(string cEmail, string cPhone, string cMessage, string cFullName)
        {
            try
            {
                // Email Setup
                EmailService.Contacting(cEmail, cPhone, cMessage, cFullName);
                MYSQL = new MYSQL_Connection_Server();
                MYSQL.AddContact(cFullName, cEmail, cPhone, cMessage);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                //ViewBag.Message = "Your message has been sent successfully!";
            }
            catch (Exception ex)
            {
                // Return error message
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // Zoom meeting API
        public ActionResult ZoomMeeting()
        {
            return View();
        }
        string msg = "";
        public async Task<JsonResult> Zoom_Create(string cEmail, string cPhone, string cDate, string cTime, string cFullName)
        {
            MYSQL = new MYSQL_Connection_Server();
            DataTable dt = MYSQL.GetUsers(cEmail, cPhone);
            string ipAddress;
            IpInfo ipInfo = new IpInfo();
            bool success = false;
            ZoomMeetingLink zoom = new ZoomMeetingLink();
            try
            {
                cDate = cDate.Replace("Zoom Meeting", "");

                var culture = new CultureInfo("en-us");
                var dat = DateTime.Parse(cDate, culture).ToString("MM/dd/yyyy");
                DateTime selectedDateTime = Convert.ToDateTime(cDate + " " + cTime); // 12:30 AM

                TimeZoneInfo serverTimeZone = TimeZoneInfo.Local;
                string jsonResponse = await zoom.CreateMeeting("Complementry Meeting", selectedDateTime, serverTimeZone.ToString(), 30);
                ZoomMeetingResponse meeting = JsonConvert.DeserializeObject<ZoomMeetingResponse>(jsonResponse);
                if (meeting != null) 
                {
                    try
                    {


                        ipInfo = IpInfo.IP_DETAIL(Request.ServerVariables["HTTP_USER_AGENT"], out ipAddress);
                        string current = ipInfo.City + " " + ipInfo.Region + ", " + ipInfo.Country + ". Postal code:" + ipInfo.Postal;
                        int uid = 0;
                        // Get User uid from db if exist then 
                        if (dt.Rows.Count > 0)
                        {
                            uid = Convert.ToInt32(dt.Rows[0][0].ToString());
                        }
                        else
                        {
                            MYSQL.Create(cFullName, cEmail, cPhone, "Client");
                            dt = MYSQL.GetUsers(cEmail, cPhone);
                            uid = Convert.ToInt32(dt.Rows[0][0].ToString());
                        }
                        string[] location = ipInfo.Loc.Split(',');
                        MYSQL.Create(uid, meeting.start_time.ToString(), meeting.join_url.ToString(), ipInfo.Ip, current, location[0].ToString(), location[1].ToString());
                        //  EmailService.SendingMail(string recipientEmail, string fullName,string Phone, ZoomMeetingResponse meeting);
                        //ZoomMeetingResponse meeting = new ZoomMeetingResponse();
                        //meeting.start_time = selectedDateTime;
                        //meeting.join_url = "";
                        bool WithoutJoining_URL = false;
                        EmailService.SendingMail(cEmail, cFullName, cPhone, meeting, WithoutJoining_URL);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                success = true;
            }
            return Json(success, JsonRequestBehavior.AllowGet);
        }
        // Creating Zoom Meeting Link
        public async Task<JsonResult> ZoomMeeting_Create(string cEmail, string cPhone, string cDate, string cTime, string cFullName, bool IS_NEW_ZOOM_MEETING)
        {
            MYSQL = new MYSQL_Connection_Server();
            DataTable dt = MYSQL.GetUsers(cEmail, cPhone);
            string ipAddress;
            IpInfo ipInfo = new IpInfo();
            ZoomMeetingLink zoom = new ZoomMeetingLink("", "", "", "", 0);
            try
            {
                var culture = new CultureInfo("en-us");
                var dat = DateTime.Parse(cDate, culture).ToString("MM/dd/yyyy");
                DateTime selectedDateTime = Convert.ToDateTime(cDate + " " + cTime); // 12:30 AM
                string userTimeZone = "Asia/Karachi"; // User's timezone

                TimeZoneInfo serverTimeZone = TimeZoneInfo.Local;
                string jsonResponse = await zoom.CreateMeeting("Zoom Meeting", selectedDateTime, serverTimeZone.ToString(), 30);
                ZoomMeetingResponse meeting = JsonConvert.DeserializeObject<ZoomMeetingResponse>(jsonResponse);
                if (meeting != null)
                {
                    try
                    {
                     
                        ipInfo = IpInfo.IP_DETAIL(Request.ServerVariables["HTTP_USER_AGENT"], out ipAddress);
                        string current = ipInfo.City + " " + ipInfo.Region + ", " + ipInfo.Country + ". Postal code:" + ipInfo.Postal;
                        int uid = 0;
                        if (dt.Rows.Count > 0)
                        {
                            // Get User uid from db if exist then 
                            uid = Convert.ToInt32(dt.Rows[0][0].ToString());
                        }
                        else
                        {
                            MYSQL.Create(cFullName, cEmail, cPhone, "Client");
                            dt = MYSQL.GetUsers(cEmail, cPhone);
                            uid = Convert.ToInt32(dt.Rows[0][0].ToString());
                        }
                        string[] location = ipInfo.Loc.Split(',');
                        MYSQL.Create(uid, meeting.start_time.ToString(), meeting.join_url, ipInfo.Ip, current, location[0].ToString(), location[1].ToString());
                        //Console.WriteLine($"Meeting Topic: {meeting.topic}");
                        //Console.WriteLine($"Meeting ID: {meeting.id}");
                        //Console.WriteLine($"Start Time: {meeting.start_time}");
                        //Console.WriteLine($"Join URL: {meeting.join_url}");
                        //Console.WriteLine($"Host Email: {meeting.host_email}");
                        //EmailService.SendingMail("", "", "", meeting);
                        msg = "The appointment will take place by Video meeting online, the link will show here.";
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
