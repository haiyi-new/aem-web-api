using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MyWebApi.Models;
using MySql.Data.MySqlClient; // Add this namespace for MySqlConnection

namespace MyWebApi.Controllers
{
    [Route("[controller]")]
    public class DataSaveController : ControllerBase
    {
        private readonly string LoginUrl = "http://test-demo.aemenersol.com/api/Account/Login";
        private readonly string PlatformWellActualUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";
        private readonly string PlatformWellDummyUrl = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy";
        private readonly string Username = "user@aemenersol.com";
        private readonly string Password = "Test@123";
        private string ApiKey = "";

        private readonly MyDbContext dbContext;

        public DataSaveController(MyDbContext myDbContext)
        {
            dbContext = myDbContext;
        }

        private async Task<string> Login()
        {
            using var httpClient = new HttpClient();
            var loginData = new { username = Username, password = Password };
            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(LoginUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to login: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            return responseData.Trim('"');
        }

        private async Task<String> GetDataFromExternalApi(string url)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ApiKey);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to access data from {url}: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadAsStringAsync();

            return responseData;
        }

        [HttpGet("Actual")]
        public async Task<IActionResult> Actual()
        {
            try
            {
                ApiKey = await Login();

                if (string.IsNullOrEmpty(ApiKey))
                {
                    return BadRequest ("API key is null or empty. Failed to retrieve API key.");
                }

                var dashboardDataJsonRaw = await GetDataFromExternalApi(PlatformWellActualUrl);
                var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellActualResponse>>(dashboardDataJsonRaw);

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = $"Error saving data: {ex.Message}"});
            }
        }

        [HttpGet("Dummy")]
        public async Task<IActionResult> Dummy()
        {
            try
            {
                ApiKey = await Login();

                if (string.IsNullOrEmpty(ApiKey))
                {
                    return BadRequest ("API key is null or empty. Failed to retrieve API key.");
                }

                var dashboardDataJsonRaw = await GetDataFromExternalApi(PlatformWellDummyUrl);
                var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellDummyResponse>>(dashboardDataJsonRaw);

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = $"Error saving data: {ex.Message}"});
            }
        }
        
        [HttpGet("Actualx")]
        public async Task<IActionResult> Actualx()
        {
            ApiKey = await Login();

            if (string.IsNullOrEmpty(ApiKey))
            {
                return BadRequest("API key is null or empty. Failed to retrieve API key.");
            }

            var dashboardDataJsonRaw = await GetDataFromExternalApi(PlatformWellActualUrl);
            var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellActualResponse>>(dashboardDataJsonRaw);

            dbContext.PlatformWellActualResponse.AddRange(jsonObject);
            await dbContext.SaveChangesAsync();

            return Ok(jsonObject.First().Well.First());
        }

        [HttpGet("Dummyx")]
        public async Task<IActionResult> Dummyx()
        {
            ApiKey = await Login();

            if (string.IsNullOrEmpty(ApiKey))
            {
                return BadRequest("API key is null or empty. Failed to retrieve API key.");
            }

            var dashboardDataJsonRaw = await GetDataFromExternalApi(PlatformWellDummyUrl);
            var jsonObject = JsonConvert.DeserializeObject<List<PlatformWellDummyResponse>>(dashboardDataJsonRaw);

            dbContext.PlatformWellDummyResponse.AddRange(jsonObject);
            await dbContext.SaveChangesAsync();

            return Ok(jsonObject.First().Well.First());
        }

    }
}
