using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mirco.Interfaces;
using Mirco.Models;

namespace MircoService.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController( IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("Get")]
        public UserModel Get(int id)
        {
            return _userService.GetAll().FirstOrDefault(o => o.Id == id);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<UserModel> GetAll()
        {
            return _userService.GetAll();
        }
    }
}