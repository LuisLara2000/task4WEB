using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using task4Web.Controllers;
using task4Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace task4Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public ExtraController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpDelete("recibir-valor")]
        public async void RecibirValor([FromBody] string valor)
        {
            var appController = new AppController(_httpClientFactory);
            //var response = await appController._httpClient.DeleteAsync("api/Users/DeleteUsers?ids=" + valor);
           
        }
        /*
        [HttpPut("BlockUsers")]
        public async void BlockUsers([FromBody] string users)
        {
            var appController = new AppController(_httpClientFactory);
            var json = JsonConvert.SerializeObject(users);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await appController._httpClient.PutAsync("api/Users/BlockUsers",contenido);
            if (response.IsSuccessStatusCode)
            {
                appController.Admin();
            }

        }
*/    }
}
