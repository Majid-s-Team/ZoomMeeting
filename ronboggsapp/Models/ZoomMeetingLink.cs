using Newtonsoft.Json;
using Org.BouncyCastle.Cms;
using ronboggsapp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ronboggsapp.Models
{
    public class ZoomMeetingLink
    {
        private string BaseUrl          = ZoomConfiguration.ZOOM_PARAM_BaseUrl; 
        private string ClientId         = ZoomConfiguration.ZOOM_PARAM_ClientId;
        private string ClientSecret     = ZoomConfiguration.ZOOM_PARAM_ClientSecret;
        private string AccountId        = ZoomConfiguration.ZOOM_PARAM_AccountId;  
        private string Grant_type       = ZoomConfiguration.ZOOM_PARAM_grant_type; 
        private string Topic            = ZoomConfiguration.ZOOM_PARAM_topic;
        private string TokenUrl         = ZoomConfiguration.ZOOM_PARAM_TokenUrl;
        string _userId                  = ZoomConfiguration.ZOOM_ID;


        private string Email { get; set; }
        private string FullName { get; set; }
        private string Phone { get; set; }
        private string[] dtime = new string[2];
        private int duration { get; set; }
        public ZoomMeetingLink() { }
        public ZoomMeetingLink(string Email,string FullName,string Phone,string datetime,int duration) 
        {
            this.Email = Email;
            this.FullName = FullName;
            this.Phone = Phone;
            this.duration = duration;
            dtime = datetime.Split('_');
        }
        
        // Create zoom meeting link
        public async Task<string> CreateMeeting(string topic, DateTime userSelectedTime, string userTimeZone, int duration)
        {
            // Step 1: Get an access token
            var accessToken = await GetAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                // Handle error when unable to retrieve the access token
                return "Failed to retrieve access token";
            }
            // Step 2: Create a meeting
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var request = new
            {
                topic = Topic,
                type = 2, // Scheduled meeting
                start_time = userSelectedTime.ToString("yyyy-MM-ddTHH:mm:ssZ"), // Start time in UTC
                duration = duration, //60, // Duration in minutes
                timezone = userTimeZone,
                settings = new
                {
                    join_before_host = true,
                    mute_upon_entry = true,
                    auto_recording = "cloud"
                }
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var createMeetingUrl = $"{BaseUrl}/users/me/meetings";
            var response = await httpClient.PostAsync(createMeetingUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                // Parse the JSON response to get the meeting details
                // You can retrieve the join URL, meeting ID, and other information from the response
                return result;
            }
            // Handle error when unable to create the meeting
            return "Failed to create meeting";
        }
        // Create zoom meeting link
        public async Task<string> CreateMeeting()
        {
            // Step 1: Get an access token
            var accessToken = await GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                // Handle error when unable to retrieve the access token
                return "Failed to retrieve access token";
            }

            // Step 2: Create a meeting
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var request = new
            {
                topic = Topic,
                type = 2, // Scheduled meeting
                start_time = DateTime.UtcNow.AddMinutes(10).ToString("yyyy-MM-ddTHH:mm:ssZ"), // Start time in UTC
                duration = this.duration, //60, // Duration in minutes
                timezone = "America/Los_Angeles",
                settings = new
                {
                    join_before_host = true,
                    mute_upon_entry = true,
                    auto_recording = "cloud"
                }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var createMeetingUrl = $"{BaseUrl}/users/me/meetings";
            var response = await httpClient.PostAsync(createMeetingUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                // Parse the JSON response to get the meeting details
                // You can retrieve the join URL, meeting ID, and other information from the response
                return result;
            }

            // Handle error when unable to create the meeting
            return "Failed to create meeting";
        }
        // list Meeting for users
        public async Task<List<dynamic>> GetUserMeetingsAsync(string userId, string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"https://api.zoom.us/v2/users/{userId}/meetings");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            return jsonResponse.meetings.ToObject<List<dynamic>>();
        }
        //retrieve the user ID associated with the email address
        public async Task<string> GetUserIdByEmailAsync(string email, string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"https://api.zoom.us/v2/users/{email}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            return jsonResponse.id;
        }
        //Fetch Meeting Detail by emails
        public async Task FetchMeetingDetailsByEmailAsync(string email)
        {
            var accessToken = await GetAccessToken();
            // Step 1: Get User ID by Email
            var userId = await GetUserIdByEmailAsync(email, accessToken);
            Console.WriteLine($"User ID: {userId}");

            // Step 2: List Meetings for User
            var meetings = await GetUserMeetingsAsync(userId, accessToken);
            foreach (var meeting in meetings)
            {
                //Console.WriteLine($"Meeting ID: {meeting.id}, Topic: {meeting.topic}");
                string mid = meeting.id.ToString();
                var meetingDetails = await GetMeetingDetailsAsync(mid, accessToken);

                var participants = await GetMeetingParticipantsAsync(meeting.id.ToString(), accessToken);

                Console.WriteLine($"Meeting Details: {Newtonsoft.Json.JsonConvert.SerializeObject(meetingDetails)}");
            }

            // Step 3: Fetch Details of a Specific Meeting (Example: First Meeting)
            if (meetings.Count > 0)
            {
              //Console.WriteLine($"Meeting ID: {meeting[0].id}, Topic: {meeting.topic}");
            }
            else
            {
                Console.WriteLine("No meetings found for this user.");
            }
        }
        // Get Meeting Details
        public async Task<dynamic> GetMeetingDetailsAsync(string meetingId, string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync($"https://api.zoom.us/v2/meetings/{meetingId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject(content);
        }
        // Get Meeting Participant
        private async Task<List<dynamic>> GetMeetingParticipantsAsync(string meetingId, string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"https://api.zoom.us/v2/report/meetings/{meetingId}/participants");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            return jsonResponse.participants.ToObject<List<dynamic>>();
        }
        // Fetch Meeting Details With Participants By Email 
        public async Task FetchMeetingDetailsWithParticipantsByEmailAsync(string email)
        {
            var _accessToken = await GetAccessToken();
            // Step 1: Get User ID by Email
            var userId = await GetUserIdByEmailAsync(email, _accessToken);
            Console.WriteLine($"User ID: {userId}");

            // Step 2: List Meetings for User
            var meetings = await GetUserMeetingsAsync(userId, _accessToken);
            foreach (var meeting in meetings)
            {
                Console.WriteLine($"Meeting ID: {meeting.id}, Topic: {meeting.topic}");

                // Step 3: Fetch Meeting Details
                var meetingDetails = await GetMeetingDetailsAsync(meeting.uuid.ToString(), _accessToken);
                Console.WriteLine($"Meeting Details: {Newtonsoft.Json.JsonConvert.SerializeObject(meetingDetails)}");

                // Step 4: Fetch Participants for the Meeting
                try
                {
                    var participants = await GetMeetingParticipantsAsync(meeting.id.ToString(), _accessToken);
                    foreach (var participant in participants)
                    {
                        Console.WriteLine($"Participant Name: {participant.name}, Email: {participant.email}");
                    }
                }
                catch(Exception ex) 
                {

                }
            }
        }
        // Creating Token
        private async Task<string> GetAccessToken()
        {
            using (var httpClient = new HttpClient())
            {
                var clientId = ClientId;
                var clientSecret = ClientSecret;

                var authHeader = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestBody = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", Grant_type),
                    new KeyValuePair<string, string>("account_id", AccountId)  // Replace with your actual account_id
                };

                var content = new FormUrlEncodedContent(requestBody);
                var tokenUrl = TokenUrl;

                var response = await httpClient.PostAsync(tokenUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Token Response: {result}");  // Debugging log

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    return jsonResult.access_token;
                }
                else
                {
                    throw new Exception($"Error getting access token: {result}");
                }
            }
        }
        public async Task<List<dynamic>> GetUserMeetingsAsync()
        {
            var accessToken = await GetAccessToken();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"https://api.zoom.us/v2/users/{_userId}/meetings?type=past");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(content);
            return jsonResponse.meetings.ToObject<List<dynamic>>();
        }
        public async Task<List<dynamic>> GetMeetingParticipantsAsync(string meetingId)
        {
            var accessToken = await GetAccessToken();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"https://api.zoom.us/v2/report/meetings/{meetingId}/participants");
            //var response = await client.GetAsync($"https://api.zoom.us/v2/report/meetings/{meetingId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(content);
            return jsonResponse.participants.ToObject<List<dynamic>>();
        }
        public async Task DisplayUserMeetingsWithParticipants()
        {
            try
            {
                // Step 1: Fetch all meetings for the user
                var meetings = await GetUserMeetingsAsync();

                if (meetings.Count == 0)
                {
                    Console.WriteLine("No meetings found for this user.");
                    return;
                }
                foreach (var meeting in meetings)
                {
                    Console.WriteLine($"Meeting ID: {meeting.id}");
                    Console.WriteLine($"Topic: {meeting.topic}");
                    Console.WriteLine($"Start Time: {meeting.start_time}");
                    Console.WriteLine($"Duration: {meeting.duration} minutes");
                    Console.WriteLine($"Join URL: {meeting.join_url}");
                    try
                    {
                        // Step 2: Fetch participants for this meeting
                        var participants = await GetMeetingParticipantsAsync(meeting.id.ToString());
                        Console.WriteLine("Participants:");
                        foreach (var participant in participants)
                        {
                            Console.WriteLine($" - Name: {participant.name}, Email: {participant.email}");
                        }
                        Console.WriteLine("---------------------------");
                    }
                    catch(Exception ex) { }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error fetching meetings or participants: {e.Message}");
            }
        }
    }
}