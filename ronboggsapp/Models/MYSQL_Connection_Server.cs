using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using MySqlX.XDevAPI;
using System.Web.Helpers;
using System.Xml.Linq;
using System.Web.WebPages;
using System.Collections;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Web.Services.Description;
using System.Security.Cryptography;
namespace ronboggsapp.Models
{
    // MYSQL SERVER MODEL
    public class MYSQL_Connection_Server
    {
        //private string MYSQL_CONNECTION = ConfigurationManager.AppSettings["MYSQL_CONNECTION"];
        private string MYSQL_CONNECTION = ConfigurationManager.ConnectionStrings["MYSQL_CONNECTION"].ConnectionString;
        private MySqlConnection MySqlConnection = new MySqlConnection();
        private MySqlCommand MySqlCommand = new MySqlCommand();
        string QUERY = "";
        public MYSQL_Connection_Server()
        {
            MySqlConnection.ConnectionString = MYSQL_CONNECTION;
            MySqlCommand.Connection = MySqlConnection;
        }
        public void ExecuteNoQuery(string query)
        {
            if (MySqlConnection.State == System.Data.ConnectionState.Open)
                MySqlConnection.Close();
            else
                MySqlConnection.Open();

            MySqlCommand.CommandText = query;
            MySqlCommand.ExecuteNonQuery();
            MySqlConnection.Close();
        }
        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, MySqlConnection);
            adapter.Fill(dataTable);
            return dataTable;
        }
        // Contact Table
        public bool AddAppointment(string fullName, string Email, string Phone, string AppointmentDateTime, string applink, string Status, string IpAddress, string ClientLocation, string ClientLat, string ClientLon)
        {
            if (!Email.IsEmpty() && !AppointmentDateTime.IsEmpty() && !applink.IsEmpty())
            {
                int uid = 0;
                if (AlreadyExist(Email, Phone) == true)
                {
                    DataTable IsUser = GetUsers(Email, Phone);
                    uid = Convert.ToInt32(IsUser.Rows[0][0].ToString());
                    Create(uid, AppointmentDateTime, applink, IpAddress, ClientLocation, ClientLat, ClientLon);
                    return true;
                }
                else
                {
                    Create(fullName, Email, Phone, "Client");
                    DataTable IsUser = GetUsers(Email, Phone);
                    uid = Convert.ToInt32(IsUser.Rows[0][0].ToString());
                    Create(uid, AppointmentDateTime, applink, IpAddress, ClientLocation, ClientLat, ClientLon);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public void Create(int uid, string AppointmentDateTime, string applink, string IpAddress, string ClientLocation, string ClientLat, string ClientLon)
        {
            this.QUERY = "insert into tbl_zoomappointments(uid,AppointmentDateTime,applink,Status,IpAddress,ClientLocation,ClientLat,ClientLon)";
            this.QUERY += " values(" + uid + ",'" + AppointmentDateTime + "','" + applink + "','New Appointment','" + IpAddress + "','" + ClientLocation + "','" + ClientLat + "','" + ClientLon + "')";
            ExecuteNoQuery(this.QUERY);
        }
        public void Create(string fullName, string Email, string Phone, string RoleName)
        {
            string[] NameObj = fullName.Split(' ');
            string LastName = "";
            for (int i = 1; i < NameObj.Length; i++)
            {
                LastName += NameObj[i];
            }
            this.QUERY = "insert into useraccounts(firstName,lastName,email,phone,RoleName,createdDate,status)";
            this.QUERY += " values('" + NameObj[0] + "','" + LastName + "','" + Email + "','" + Phone + "','" + RoleName + "','" + DateTime.Now.ToShortDateString() + "',1)";
            ExecuteNoQuery(this.QUERY);
        }
        public void AddContact(string fullName, string Email, string Phone, string Message)
        {
            if (!fullName.IsEmpty() && !Email.IsEmpty() || !Phone.IsEmpty() && !Message.IsEmpty())
            {
                if (AlreadyExist(Email, Phone) == true)
                {
                    DataTable IsUser = GetUsers(Email, Phone);
                    int uid = Convert.ToInt32(IsUser.Rows[0][0].ToString());
                    this.QUERY = "insert into tbl_contact(uid,con_message,con_datetime)";
                    this.QUERY += " values(" + uid + ",'" + Message + "','" + DateTime.Now.ToShortDateString() + "')";
                    ExecuteNoQuery(this.QUERY);
                }
                else
                {
                    // New Contact Information Entry
                    Create(fullName, Email, Phone, "Contact");
                    // Adding Information in Contact Table
                    DataTable IsUser = GetUsers(Email, Phone);
                    if (IsUser.Rows.Count > 0)
                    {
                        int uid = Convert.ToInt32(IsUser.Rows[0][0].ToString());
                        this.QUERY = "insert into tbl_contact(uid,con_message,con_datetime)";
                        this.QUERY += " values(" + uid + ",'" + Message + "','" + DateTime.Now.ToShortDateString() + "')";
                        ExecuteNoQuery(this.QUERY);
                    }
                }
            }
        }
        public DataTable GetUsers()
        {
            this.QUERY = "SELECT * FROM useraccounts";
            return ExecuteQuery(this.QUERY);
        }
        public DataTable GetUsers(string Email, string Phone)
        {
            this.QUERY = "SELECT * FROM useraccounts where email='" + Email + "' or phone='" + Phone + "'";
            return ExecuteQuery(this.QUERY);
        }
        public bool AlreadyExist(string Email, string Phone)
        {
            DataTable dt = new DataTable();
            this.QUERY = "SELECT * FROM useraccounts WHERE email='" + Email + "' OR phone='" + Phone + "'";
            if (!Email.IsEmpty() || !Phone.IsEmpty())
            {
                dt = ExecuteQuery(this.QUERY);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        // LOGIN FUNCTION
        public bool ValidateUser(string username, string password)
        {
            bool isValidUser = false;
            this.QUERY = "SELECT password FROM useraccounts WHERE email=@username";
            MySqlCommand.CommandText = this.QUERY;
            MySqlCommand.Parameters.AddWithValue("@username", username);
            MySqlDataAdapter adapter = new MySqlDataAdapter(MySqlCommand);
            DataTable dtlogin = new DataTable();
            adapter.Fill(dtlogin);
            if (dtlogin.Rows.Count > 0) 
            {
                string storedPassword = dtlogin.Rows[0]["password"].ToString();
                // In production, compare hashed passwords using BCrypt
                if (password == storedPassword) // For testing purposes
                {
                    isValidUser = true;
                }
            }
            return isValidUser;
        }
    }
}
