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
        public int UserId { get; set; }
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
                    UserId = loginData.Data.Id;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Data.Token);
                }
            }
            catch (AggregateException)
            {
                Console.WriteLine("Internet is not working");
            }

        }
        public async Task<byte[]> GetFolder(int id)
        {
            var response = await client.GetAsync($"/DownloadFolder/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
                return null;
        }
        public async Task<List<FolderList>> GetFoldersByUserId()
        {
            var getRes =await client.GetAsync($"Folder/GetFoldersByAppUserId/{UserId}");
            if (getRes.IsSuccessStatusCode)
            {
                var fileData = JsonConvert.DeserializeObject<GenericMultiple<FolderList>>(await getRes.Content.ReadAsStringAsync());
                return fileData.Data;
            }
            else
                return null;
        }

        public async Task<List<FolderList>> GetSubFoldersByFolderId(int folderId)
        {
            var getRes = await client.GetAsync($"Folder/GetSubFoldersByFolderId/{folderId}");
            if (getRes.IsSuccessStatusCode)
            {
                var folderData = JsonConvert.DeserializeObject<GenericMultiple<FolderList>>(await getRes.Content.ReadAsStringAsync());
                return folderData.Data;
            }
            else
                return null;
        }


        public async Task<string> EditAsync(string folderName,int id)
        {
            var jsonData = JsonConvert.SerializeObject(new { Id=id, FolderName=folderName });
            StringContent content = new StringContent(jsonData);
            var response = await client.PutAsync($"Folder/{id}",content);
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var errorMessage = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());
                return errorMessage.Message.First();
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

      
        public async Task<string> AddAsync(int? id, string folderName)
        {
            var jsonData = JsonConvert.SerializeObject(new { FolderName = folderName });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = new HttpResponseMessage();
            if (id != null)
            {
                response = await client.PostAsync($"Folder/{id}", content);
            }
            else
            {
                response = await client.PostAsync($"Folder", content);
            }

            if (response.IsSuccessStatusCode)
                return "";
            else
            {
                var sea = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());
                return sea.Message.First();
            }
        }
    }
}
