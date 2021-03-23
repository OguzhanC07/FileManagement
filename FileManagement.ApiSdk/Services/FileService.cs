using FileManagement.ApiSdk.RequestClasses;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.ApiSdk.Services
{
    public class FileService : IFileService
    {
        private readonly HttpClient client = new HttpClient { BaseAddress=new Uri(ApiConstants.ApiUrl) };
        public async Task<bool> RemoveFile(string username,string password, int id)
        {
            var userLogin = new UserLogin { UserName=username,Password=password };
            var loginJson = JsonConvert.SerializeObject(userLogin);
            StringContent loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            var loginRes = await client.PostAsync("User", loginContent);
            if (loginRes.IsSuccessStatusCode)
            {
                var loginData = JsonConvert.DeserializeObject<GenericData<LoginData>>(await loginRes.Content.ReadAsStringAsync());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Data.Token);

                var deleteRes = await client.DeleteAsync($"File/{id}");
                if (deleteRes.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> UploadFile(string username, string password, int folderId,string filePath)
        {
            var userLogin = new UserLogin { UserName = username, Password = password };
            var loginJson = JsonConvert.SerializeObject(userLogin);
            StringContent loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");

            var loginRes = await client.PostAsync("User", loginContent);
            if (loginRes.IsSuccessStatusCode)
            {
                var loginData = JsonConvert.DeserializeObject<GenericData<LoginData>>(await loginRes.Content.ReadAsStringAsync());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Data.Token);

                MultipartFormDataContent content = new MultipartFormDataContent();
                byte[] buffer;
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                try
                {
                    int length = (int)fileStream.Length;
                    buffer = new byte[length];
                    int count;
                    int sum = 0;

                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                        sum += count;
                }
                finally
                {
                    fileStream.Close();
                }

                ByteArrayContent byteContent = new ByteArrayContent(buffer);

                content.Add(byteContent, "formFiles",Path.GetFileName(filePath));

                var uploadRes = await client.PostAsync($"File/UploadFile/{folderId}", content);
                if (uploadRes.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
