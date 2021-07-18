using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBee.Models;
using TeamBee.ViewModels;

namespace TeamBee.Function
{
    public class AdminSuaNhanVien
    {
        public User suaNhanVien( User edituser, UserViewModels user)
        {
            edituser.UserName = user.UserName;
            edituser.Password = user.Password;
            edituser.Email = user.Email;
            edituser.NameFirst = user.NameFirst;
            edituser.NameLast = user.NameLast;
            edituser.Address = user.Address;
            edituser.PhoneNumber = user.PhoneNumber;
            edituser.Permission = user.status;
            return edituser;
        }
    }
}
