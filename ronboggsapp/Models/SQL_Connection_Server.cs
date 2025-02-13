using System;
using System.Configuration;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Web.WebPages;
using System.Data.SqlClient;
namespace ronboggsapp.Models
{
    // MYSQL SERVER MODEL
    public class SQL_Connection_Server
    {
        private string SQL_CONNECTION = ConfigurationManager.ConnectionStrings["SQL_CONNECTION"].ConnectionString;
        private SqlConnection SqlConnection = new SqlConnection();
        private SqlCommand SqlCommand = new SqlCommand();
        string QUERY = "";
        public SQL_Connection_Server()
        {
            SqlConnection.ConnectionString = SQL_CONNECTION;
            SqlCommand.Connection = SqlConnection;
        }
        public void ExecuteNoQuery(string query)
        {
            if (SqlConnection.State == System.Data.ConnectionState.Open)
                SqlConnection.Close();
            else
                SqlConnection.Open();

            SqlCommand.CommandText = query;
            SqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(query, SqlConnection);
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
            SqlCommand.CommandText = this.QUERY;
            SqlCommand.Parameters.AddWithValue("@username", username);
            SqlDataAdapter adapter = new SqlDataAdapter(SqlCommand);
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
