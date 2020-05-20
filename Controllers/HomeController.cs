using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClientUI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;
using Newtonsoft.Json;
using System.Text;

namespace ClientUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var apiClient = new HttpClient();
            //apiClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await apiClient.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            //var user = new RegisterModel
            //{
            //    Username = "quangphat",
            //    Password = "quangphat"
            //};
            // var apiClient = new HttpClient();
            //var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            //var response = await apiClient.PostAsync("http://localhost:5000/api/user/register", content);
            var token = await HttpContext.GetTokenAsync("access_token");
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(token);
            var response = await apiClient.GetAsync("http://localhost:5001/account/list");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            return SignOut("Cookies", "oidc");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
