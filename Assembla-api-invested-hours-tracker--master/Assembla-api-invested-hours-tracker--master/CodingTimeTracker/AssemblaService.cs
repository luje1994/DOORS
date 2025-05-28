using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;

namespace CodingTimeTracker
{
    class AssemblaService
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;

        private const string Error = "HTTP Status: {0} – Reason: {1}";

        public AssemblaService(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public async Task<bool> AddAssemblaTicketComments(int ticketNumber, string spaceName, string ticketComment)
        {
            var comment = new TicketComment
            {
                comment = ticketComment,
            };

            var finalResult = await SetterClient(comment, new Uri($"https://api.assembla.com/v1/spaces/{spaceName}/tickets/{ticketNumber}/ticket_comments.json"));

            if (finalResult.IsSuccessStatusCode)
            {
                MessageBox.Show(@"Ticket Comment Has been Posted", @"Success", MessageBoxButtons.OK);
                return true;
            }

            MessageBox.Show(string.Format(Error, finalResult.StatusCode, finalResult.ReasonPhrase), @"Sorry",
                MessageBoxButtons.OK);
            return false;
        }

        public async Task<bool> AddAssemblaTasks(string description, int ticketId, string spaceId, DateTime beginAt, DateTime endAt, float hours)
        {
            var task = new UserTask
            {
                Tasks = new Task
                {
                    description = description,
                    space_id = spaceId,
                    ticket_id = ticketId,
                    hours = hours,
                    begin_at = beginAt,
                    end_at = endAt
                }
            };

            var finalResult = await SetterClient(task, new Uri("http://api.assembla.com/v1/tasks.json"));

            if (finalResult.IsSuccessStatusCode)
            {
                MessageBox.Show(@"Task Has been Posted", @"Success",
                    MessageBoxButtons.OK);
                return true;
            }

            MessageBox.Show(string.Format(Error, finalResult.StatusCode, finalResult.ReasonPhrase), @"Sorry",
                MessageBoxButtons.OK);
            return false;
        }

        public async Task<List<Space>> GetAssemblaSpaces()
        {
            var response = await GetterClient("https://api.assembla.com/v1/spaces.json");

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<List<Space>>().Result;

            MessageBox.Show(string.Format(Error, response.StatusCode, response.ReasonPhrase), @"Sorry",
                MessageBoxButtons.OK);
            return null;
        }

        public async Task<List<Ticket>> GetAssemblaTickets(string spaceName)
        {
            if (string.IsNullOrEmpty(spaceName))
            {
                MessageBox.Show(@"SpaceName Can't Be Blank", @"Sorry",
                    MessageBoxButtons.OKCancel);
                return null;
            }

            var response = await GetterClient($"https://api.assembla.com/v1/spaces/{spaceName}/tickets.json");

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<List<Ticket>>().Result;

            MessageBox.Show(string.Format(Error, response.StatusCode, response.ReasonPhrase), @"Sorry",
                MessageBoxButtons.OK);
            return null;
        }

        public async Task<User> GetAssemblaName()
        {
            var response = await GetterClient("https://api.assembla.com/v1/user.json");

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<User>().Result;

            MessageBox.Show(string.Format(Error, response.StatusCode, response.ReasonPhrase), @"Sorry",
                MessageBoxButtons.OK);
            return null;
        }

        private async Task<HttpResponseMessage> SetterClient(object obj, Uri path)
        {
            DataContractJsonSerializer jsonSer;

            if (obj is UserTask)
            {
                obj = (UserTask)obj;
                jsonSer = new DataContractJsonSerializer(typeof(UserTask));
            }
            else if (obj is TicketComment)
            {
                obj = (TicketComment)obj;
                jsonSer = new DataContractJsonSerializer(typeof(TicketComment));
            }
            else
                throw new InvalidCastException();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
                client.DefaultRequestHeaders.Add("X-Api-Secret", _apiSecret);

                var ms = new MemoryStream();
                jsonSer.WriteObject(ms, obj);
                ms.Position = 0;

                var sr = new StreamReader(ms);

                var theContent = new StringContent(sr.ReadToEnd(), Encoding.UTF8, "application/json");

                return await client.PostAsync(path, theContent);
            }
        }

        private async Task<HttpResponseMessage> GetterClient(string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(path);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
                client.DefaultRequestHeaders.Add("X-Api-Secret", _apiSecret);

                return await client.GetAsync("?api_key=123");
            }
        }
    }
}
