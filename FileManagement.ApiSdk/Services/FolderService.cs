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
        public async Task<bool> RemoveAsync(string userName, string password, int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiConstants.ApiUrl);

            var userData = new UserLogin { UserName = userName, Password = password };

            var loginJson = JsonConvert.SerializeObject(userData);
            StringContent loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            var loginRes = await client.PostAsync("User", loginContent);

            //LoginRoot loginToken = JsonConvert.DeserializeObject<LoginRoot>(await loginRes.Content.ReadAsStringAsync());
            if (loginRes.IsSuccessStatusCode)
            {
                var loginInfo = JsonConvert.DeserializeObject<GenericData<LoginData>>(await loginRes.Content.ReadAsStringAsync());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginInfo.Data.Token);
                var response = await client.DeleteAsync($"Folder/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> AddAsync(string userName, string password, string folderName)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ApiConstants.ApiUrl);

            var userData = new UserLogin { UserName = userName, Password = password };

            var loginJson = JsonConvert.SerializeObject(userData);
            StringContent loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            var loginRes = await client.PostAsync("User", loginContent);

            //LoginRoot loginToken = JsonConvert.DeserializeObject<LoginRoot>(await loginRes.Content.ReadAsStringAsync());
            if (loginRes.IsSuccessStatusCode)
            {
                var loginInfo = JsonConvert.DeserializeObject<GenericData<LoginData>>(await loginRes.Content.ReadAsStringAsync());
                var jsonData = JsonConvert.SerializeObject(new { FolderName = folderName });
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",loginInfo.Data.Token);
                var response = await client.PostAsync("Folder", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        
    }
}
