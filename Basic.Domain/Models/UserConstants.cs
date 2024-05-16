using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Domain.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new()
        {
            new UserModel(){ Username="admin",EmailAddress ="admin@gmail.com",Password="admin",Role="Admin"},
            new UserModel(){ Username="user",EmailAddress ="user@gmail.com",Password="user",Role="User"}
        };
    }
}
