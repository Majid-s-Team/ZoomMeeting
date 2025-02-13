using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ronboggsapp.Models
{
    public class ZoomConfiguration
    {
        public static string ZOOM_PARAM_BaseUrl         = ConfigurationManager.AppSettings["ZOOM_PARAM_BaseUrl"].ToString();
        public static string ZOOM_PARAM_ClientId        = ConfigurationManager.AppSettings["ZOOM_PARAM_ClientId"].ToString();
        public static string ZOOM_PARAM_ClientSecret    = ConfigurationManager.AppSettings["ZOOM_PARAM_ClientSecret"].ToString();
        public static string ZOOM_PARAM_AccountId       = ConfigurationManager.AppSettings["ZOOM_PARAM_AccountId"].ToString();
        public static string ZOOM_PARAM_grant_type      = ConfigurationManager.AppSettings["ZOOM_PARAM_grant_type"].ToString();
        public static string ZOOM_PARAM_topic           = ConfigurationManager.AppSettings["ZOOM_PARAM_topic"].ToString();
        public static string ZOOM_PARAM_TokenUrl        = ConfigurationManager.AppSettings["ZOOM_PARAM_TokenUrl"].ToString();

        public static string ZOOM_ID = ConfigurationManager.AppSettings["ZOOM_ID"].ToString();
    }
}