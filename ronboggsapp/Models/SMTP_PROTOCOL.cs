using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ronboggsapp.Models
{
    public class SMTP_PROTOCOL
    {
        public static string SMTP       = ConfigurationManager.AppSettings["SmtpServer"];
        public static string SENDER     = ConfigurationManager.AppSettings["SmtpUsername"];
        public static string PASSWORD   = ConfigurationManager.AppSettings["SmtpPassword"];
        public static int PORT          = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);  

    }
}
