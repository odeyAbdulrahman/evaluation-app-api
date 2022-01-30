using OA.Base.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Base.Dictionarys
{
    public interface IRolesDictionary
    {
        RolesDictionary AddRoles(EnumTypeRole type);
        List<KeyValuePair<string, long>> GetRoles();
        string GetRoleKey(long value);
        long GetRoleValue(string key);
    }
}
