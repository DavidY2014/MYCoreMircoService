using System;
using System.Collections.Generic;
using Mirco.Interfaces;
using Mirco.Models;

namespace Mirco.Implments
{
    public class UserService:IUserService
    {
        #region  datainit
        private List<UserModel> _userList = new List<UserModel>
            {
                new UserModel(){
                    Id=1,
                    Name="Jack",
                    Age=10
                },
                new UserModel(){
                    Id=2,
                    Name="Tom",
                    Age=20
                },
                new UserModel(){
                    Id=3,
                    Name="Lucy",
                    Age=30
                }
            };



        #endregion

        public List<UserModel> GetAll()
        {
            return _userList;
        }

    }
}
