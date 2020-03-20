using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;

namespace MircoService.ServiceInstance.Utility
{
    /// <summary>
    /// consul服务注册
    /// </summary>
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://localhost:8500");
                c.Datacenter = "dcl";
            });
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 :
                int.Parse(configuration["weight"]);
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service"+Guid.NewGuid(),
                Name = "OrderServiceGroup",
                Address = ip,
                Port = port,//不同实例
                Tags = new string[] { weight.ToString()},
                Check = new AgentServiceCheck()
                { 
                    Interval = TimeSpan.FromSeconds(12),
                    HTTP = $"http://{ip}:{port}/Api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60),
                }


            });


        }


    }
}
