using FileManagement.ApiSdk.RequestClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public class FolderService : IFolderService
    {
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri(ApiConstants.ApiUrl) };

        public FolderService(string username, string password)
        {

            var userData = new UserLogin { UserName = username, Password = password };

            var loginJson = JsonConvert.SerializeObject(userData);
            StringContent loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            try
            {
                var loginRes = client.PostAsync("User", loginContent).Result;
                if (loginRes.IsSuccessStatusCode)
                {
                    var loginData = JsonConvert.DeserializeObject<GenericData<LoginData>>(loginRes.Content.ReadAsStringAsync().Result);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Data.Token);
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Internet is not working");
            }

        }
        public async Task<bool> RemoveAsync(int id)
        {
            var response = await client.DeleteAsync($"Folder/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddAsync(string folderName)
        {
            var jsonData = JsonConvert.SerializeObject(new { FolderName = folderName });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Folder", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }


    }
}
