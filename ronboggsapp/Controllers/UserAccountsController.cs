using Org.BouncyCastle.Cms;
using ronboggsapp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;

namespace ronboggsapp.Controllers
{
    public class UserAccountsController : Controller
    {
        private DataTable dtRecords = new DataTable();
        private MYSQL_Connection_Server MYSQL;
        public ActionResult Index()
        {
            Response.Cache.SetNoStore();
            Session["CurrentPage"] = "Dashboard";
            if (Session["Username"] != null) 
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View();
            }
        }
        public ActionResult AppContactList()
        {
            Response.Cache.SetNoStore();
            if (Session["Username"] != null)
            {
                Session["CurrentPage"] = "Inbox";
                MYSQL = new MYSQL_Connection_Server();
                try
                {
                    dtRecords = MYSQL.ExecuteQuery(TableName.ContactList.ToString());
                    ViewBag.ContactList = dtRecords;
                    return View();
                }
                catch (Exception ex)
                {
                    dtRecords = null;
                    ViewBag.Appointments = dtRecords;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            try
            {
                MYSQL = new MYSQL_Connection_Server();
                if (MYSQL.ValidateUser(username, password))
                {
                    Session["Username"] = username;
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                    return View();
                }
            }
            catch(Exception ex) 
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        public ActionResult Dashboard()
        {
            Response.Cache.SetNoStore();
            if (Session["Username"] != null)
            {
                Session["CurrentPage"] = "Dashboard";
                MYSQL = new MYSQL_Connection_Server();
                try
                {

                    
                    dtRecords = MYSQL.ExecuteQuery(TableName.AppointmentList.ToString());
                    ViewBag.AppointmentList = dtRecords;
                    dtRecords = MYSQL.ExecuteQuery(TableName.ContactList.ToString());
                    ViewBag.ContactList = dtRecords;
                    return View();
                }
                catch (Exception ex)
                {
                    dtRecords = null;
                    ViewBag.Appointments = dtRecords;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Logout()
        {
            Session["Username"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        // Delete
        [HttpGet]
        public ActionResult ZOOM_MEETING_Delete(string id)
        {
            Response.Cache.SetNoStore();
            if (Session["Username"] != null)
            {
                Session["CurrentPage"] = "Dashboard";
                MYSQL = new MYSQL_Connection_Server();
                try
                {
                    MYSQL.ExecuteNoQuery(TableName.Appointment_DELETE_BY_ID + " where id="+Convert.ToInt32(id)+"");
                    return RedirectToAction("Dashboard");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // Delete
        [HttpGet]
        public ActionResult CONTACT_Delete(string id)
        {
            Response.Cache.SetNoStore();
            if (Session["Username"] != null)
            {
                Session["CurrentPage"] = "Inbox";
                MYSQL = new MYSQL_Connection_Server();
                try
                {
                    MYSQL.ExecuteNoQuery(TableName.ContactList_DELETE_BY_ID + " where id=" + Convert.ToInt32(id) + "");
                    return RedirectToAction("AppContactList");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("AppContactList");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}