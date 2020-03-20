using System;
using System.Collections.Generic;
using Mirco.Models;

namespace Mirco.Interfaces
{
    public interface IUserService
    {
        List<UserModel> GetAll();

    }
}
