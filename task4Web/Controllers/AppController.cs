using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using task4Web.Models;
using System.Text.Json;
using System.Text;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SHA3.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace task4Web.Controllers
{

    public class AppController : Controller
    {
        public readonly HttpClient _httpClient;
     
        public AppController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://www.task4api.somee.com/");
        }
        public string encryptPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = Sha3.Sha3256().ComputeHash(bytes);
            string hashHex = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return hashHex;
        }
        public async void generateAutentication(string emailUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,  emailUser)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        }

        public async Task<IEnumerable<UsersModels>> listUsers()
        {
            var response = await _httpClient.GetAsync("api/Users/ListUsers");
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<IEnumerable<UsersModels>>(await response.Content.ReadAsStringAsync());    
            return new List<UsersModels>();
        }
  
        public async Task<IActionResult> CreateUser(UsersCreateAccountModel user)
        {
            if(ModelState.IsValid)
            {
                user.hashPassword = encryptPassword(user.hashPassword);
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/Users/CreateUser", content);
                if (response.IsSuccessStatusCode)
                    return View("LogIn");
                ModelState.AddModelError("Email", "[This email already exist]");
            }
            return View("CreateAccount");
        }
        public async Task<IActionResult> VerifyPassword(UsersLogInModel users)
        {
            if (ModelState.IsValid)
            {
                var responseEmail = await _httpClient.GetAsync("api/Users/ReturnHash?email="+ users.email);
                var userEmail = await responseEmail.Content.ReadAsStringAsync();
                if (responseEmail.IsSuccessStatusCode && userEmail != "")
                {
                    var hashStore = await responseEmail.Content.ReadAsStringAsync();
                    var hashCurrent = encryptPassword(users.password);
                    if (hashCurrent == hashStore)
                    {
                        var responseState = await _httpClient.GetAsync("api/Users/ValidateUserState?email=" + users.email);
                        var userBlocked = await responseState.Content.ReadAsStringAsync();
                        if (responseState.IsSuccessStatusCode && userBlocked == "false")
                        {
                            generateAutentication(users.email);
                            var contenidoState = new StringContent(JsonConvert.SerializeObject(users.email), Encoding.UTF8, "application/json");
                            await _httpClient.PostAsync("api/Users/UpdateUsers?email=", contenidoState);
                            return View("Admin", await listUsers());
                        }
                        ModelState.AddModelError("Email", "[This email is blocked]");
                    }
                    else
                        ModelState.AddModelError("Password", "[Wrong password]");
                }
                else
                    ModelState.AddModelError("Email", "[The email does not exist]");
              
            }
            return View("LogIn");
        }
        [Authorize]
        public async Task<IActionResult> Admin()
        {
            var response = await _httpClient.GetAsync("api/Users/ValidateUserState?email=" + User.FindFirst(ClaimTypes.Name)?.Value); 
            if (response.IsSuccessStatusCode)
            {
                var userDatabase = await response.Content.ReadAsStringAsync();
                if(userDatabase != "")
                {
                    if(userDatabase == "false")
                        return View("Admin", await listUsers());
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
            return View("LogIn"); 
        }
        public IActionResult CreateAccount()
        {
            return View(); 
        }
        public IActionResult LogIn()
        {

            return View();
        }
        
        public async Task<IActionResult> GoBack()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("LogIn");
        }



    }
}
