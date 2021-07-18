using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Connect_Data;
using TeamBee.Models;
using TeamBee.ViewModels;

namespace TeamBee.Function
{
    public class AdminThemNhanVien
    {

        public static User themNhanVien(UserViewModels user)
        {

            User new_user = new User
            {
                UserName = user.UserName,
                Password = user.Password,
                Email = user.Email,
                NameFirst = user.NameFirst,
                NameLast = user.NameLast,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Permission = user.status,
                Active = 0,
                Confirm = 1

            };
            return new_user;
        }
    }

}
