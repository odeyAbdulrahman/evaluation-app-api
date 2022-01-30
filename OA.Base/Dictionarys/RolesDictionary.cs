using OA.Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Base.Dictionarys
{
    public class RolesDictionary : IRolesDictionary
    {
        private readonly Dictionary<string, long> Roles = new Dictionary<string, long>(6);
        public RolesDictionary AddRoles(EnumTypeRole type)
        {
            if(type == EnumTypeRole.All)
            {
                if (Roles.Count() < 6)
                {
                    AddAdmin();
                    AddStaff();
                }
            }
            if (type == EnumTypeRole.Admins)
            {
                if (Roles.Count() < 5)
                    AddAdmin();
            } else
            {
                if (Roles.Count() < 1)
                    AddStaff();
            }
            return this;
        }
        public List<KeyValuePair<string, long>> GetRoles()
        {
            return Roles.ToList();
        }
        public string GetRoleKey(long value)
        {
            return Roles.FirstOrDefault(x => x.Value == value).Key;
        }
        public long GetRoleValue(string key)
        {
            return Roles.FirstOrDefault(x => x.Key == key).Value;
        }
        private void AddAdmin()
        {
            Roles.Add(nameof(EnumUserRole.User), (long)EnumUserRole.User);
            Roles.Add(nameof(EnumUserRole.Editor), (long)EnumUserRole.Editor);
            Roles.Add(nameof(EnumUserRole.Customer), (long)EnumUserRole.Customer);
            Roles.Add(nameof(EnumUserRole.Admin), (long)EnumUserRole.Admin);
            Roles.Add(nameof(EnumUserRole.GeneralDirector), (long)EnumUserRole.GeneralDirector);
        }
        private void AddStaff()
        {
            Roles.Add(nameof(EnumUserRole.Employee), (long)EnumUserRole.Employee);
        }
    }
}
