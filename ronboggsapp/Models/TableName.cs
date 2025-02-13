using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ronboggsapp.Models
{
    public class TableName
    {
        // Contact List Query
        public static string ContactList = "SELECT u.id, CONCAT(u.firstName, ' ', u.lastName) AS FullName, u.phone AS Phone, u.email AS `From`, c.con_message AS Message, c.con_datetime AS Email_Date_Time, c.id as 'cid' FROM useraccounts AS u INNER JOIN tbl_contact AS c ON u.id = c.uid WHERE roleName = 'Contact' order by Email_Date_Time DESC;";
        // Contact List Query
        public static string AppointmentList = "SELECT u.id, CONCAT(u.firstName, ' ', u.lastName) AS FullName, u.phone AS 'Phone', u.email AS `ClientEmail`,z.AppointmentDateTime as 'ZoomAppointmentDateTime', z.applink as 'ZoomLink',z.Status as 'AppointmentType',z.IpAddress as 'IP',z.ClientLocation as 'Location',z.ClientLat as 'Latitude', z.ClientLon as 'Longitude',z.id as 'zid' FROM useraccounts AS u INNER JOIN tbl_zoomappointments AS z ON u.id = z.uid  WHERE z.Status = 'New Appointment' order by z.AppointmentDateTime DESC;";

        // Delete Zoom Meeting
        public static string Appointment_DELETE_BY_ID = "delete from tbl_zoomappointments ";
        public static string ContactList_DELETE_BY_ID = "delete from tbl_contact ";

    }
}
