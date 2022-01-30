using OA.Api.Common.HttpClientsBase;
using OA.Base.Dictionarys;
using OA.Base.Helpers.DateTimes;
using OA.Base.Helpers.GenerateRandoms;
using OA.Base.Helpers.StringCiphers;
using OA.Data.Models;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OA.Api.UnitOfWork
{
    public interface IUnitOfWork
    {
        // -------------------- Start Base Service -------------------- //
        IRolesDictionary RolesDictionaryService { get; }
        ICustomDateTime DateTimeService { get; }
        IGenerateRandom GenerateRandomService { get; }
        IStringCipher StringCipherService { get; }
        IHttpClientBase HttpClientService { get; }
        // -------------------- End Base Service -------------------- //

        ITransaction Transaction { get; }
        IRole RoleService { get; }
        IUserRole UserRoleService { get; }
        IUserAuth UserAuthService { get; }
        IUserManage UserManageService { get; }

        ICrud<Evaluation> EvaluationService { get; }
        ICrud<Department> DepartmentService { get; }
        ICrud<DepartmentEmployee> DepartmentEmployeeService { get; }
    }
}
