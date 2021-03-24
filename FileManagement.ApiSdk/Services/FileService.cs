using FileManagement.ApiSdk.RequestClasses;
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
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri(ApiConstants.ApiUrl) };
        public FileService(string username, string password)
        {
            var userLogin = new UserLogin { UserName = username, Password = password };
            var loginJson = JsonConvert.SerializeObject(userLogin);
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

        public async Task<bool> RemoveFile(int id)
        {
            var deleteRes = await client.DeleteAsync($"File/{id}");
            if (deleteRes.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<FileList>> GetFiles(int folderId)
        {
            var response = await client.GetAsync($"File/GetFiles/{folderId}");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<FileList>>(await response.Content.ReadAsStringAsync());
            }
            else
                return null;
        }

        public async Task<byte[]> GetSingleFile(int id)
        {
            var response = await client.GetAsync($"/File/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
                return null;
        }

        public async Task<string> EditFile(string name,int id)
        {
            var jsonData = JsonConvert.SerializeObject(new { Id = id, FileName = name });
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var editRes = await client.PostAsync($"File/{id}",content);
            if (editRes.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var errorRes = JsonConvert.DeserializeObject<ErrorResponse>(await editRes.Content.ReadAsStringAsync());
                return errorRes.Message.First();
            }
        }

        public async Task<string> UploadFile(int folderId, string filePath)
        {
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

            content.Add(byteContent, "formFiles", Path.GetFileName(filePath));

            var uploadRes = await client.PostAsync($"File/UploadFile/{folderId}", content);
            if (uploadRes.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var errorRes = JsonConvert.DeserializeObject<ErrorResponse>(await uploadRes.Content.ReadAsStringAsync());
                return errorRes.Message.First();
            }
        }
    }
}
