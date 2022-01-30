using OA.Base.Enums;
using OA.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Dtos.ServiceViewModel
{
    public class UserMangeRolesViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<EnumUserRole> Roles { get; set; }
    }
}
