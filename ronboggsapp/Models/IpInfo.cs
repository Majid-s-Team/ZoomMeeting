using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ronboggsapp.Models
{
    public class IpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }

        public static string GetUserCountryByIp(string ip, out string ipaddress)
        {
            IpInfo ipInfo = new IpInfo();
            string info = "";
            try
            {
                info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
                ipaddress = ipInfo.Ip;
            }
            catch (Exception ex)
            {
                //ipInfo.Country = null;
                //ipaddress = "";
                info = "Error " + ex.Message;
                ipaddress = "";
            }

            return info;
        }
        public static IpInfo IP_DETAIL(string ip, out string ipaddress)
        {
            IpInfo ipInfo = new IpInfo();
            string info = "";
            try
            {
                info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
                ipaddress = ipInfo.Ip;
                return ipInfo;
            }
            catch (Exception ex)
            {
                ipaddress = ipInfo.Ip;
                ipInfo = null;
                return ipInfo;

            }

        }
    }
}
