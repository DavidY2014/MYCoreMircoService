using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mirco.Interfaces;
using Mirco.Models;
using MircoService.Models;

namespace MircoService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        public HomeController(ILogger<HomeController> logger,IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            //单体变分布式，也就是通过url远程调用api方法
            //base.ViewBag.Users = _userService.GetAll();
            string url = "http://localhost:5726/api/user/getall";

            #region 发现consul实例

            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500");
                c.Datacenter = "dcl";
            });
            //获取信息
            var response = client.Agent.Services().Result.Response;
            var serviceDictionary = response.Where(s => s.Value.Service.Equals("")).ToArray();
            #endregion

            #region 负载均衡策略
            int iSeed = 0;
            AgentService agentService = null;
            {
                //平均随机分配
                //agentService = serviceDictionary[new Random(DateTime.Now.Millisecond+ iSeed++).Next(0,
                //    serviceDictionary.Length)].Value;

                //轮询分配
                agentService = serviceDictionary[iSeed++ % serviceDictionary.Length].Value;

                //根据权重来分配
                foreach (var pair in serviceDictionary)
                {
                    int count = int.Parse(pair.Value.Tags?[0]);
                    
                }
            
            }


            #endregion

            string content = InvokeApi(url);
            base.ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<UserModel>>(content);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        #region 远程url调用的封装

        public static string InvokeApi(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(message).Result;
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }


        #endregion



    }
}
