using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OA.Api.Common.HttpClientsBase;
using OA.Base.Dictionarys;
using OA.Base.Helpers.DateTimes;
using OA.Base.Helpers.GenerateRandoms;
using OA.Data;
using OA.Data.Models;
using OA.Repo;
using OA.Service.Interfaces;
using OA.Service.Services;

namespace OA.Api.UnitOfWork
{
    class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(AppDbContext db, UserManager<AspNetUser> userManager, RoleManager<IdentityRole> roleManager,
         IRepository<Evaluation> evaluationRepository, IRepository<Department> departmentRepository, IRepository<SubDepartment> subDepartmentRepository,
         IRepository<DepartmentEmployee> departmentEmployeeRepository, IConfiguration configuration)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            EvaluationRepository = evaluationRepository;
            DepartmentRepository = departmentRepository;
            SubDepartmentRepository = subDepartmentRepository;
            DepartmentEmployeeRepository = departmentEmployeeRepository;
            Db = db;
            Configuration = configuration;
        }
        readonly IConfiguration Configuration;
        readonly AppDbContext Db;
        //user manger db context 
        readonly UserManager<AspNetUser> UserManager;
        readonly RoleManager<IdentityRole> RoleManager;
        readonly IRepository<Evaluation> EvaluationRepository;
        readonly IRepository<Department> DepartmentRepository;
        readonly IRepository<SubDepartment> SubDepartmentRepository;
        readonly IRepository<DepartmentEmployee> DepartmentEmployeeRepository;

        // -------------------- Start Base Service -------------------- //
        private IRolesDictionary rolesDictionaryService;
        public IRolesDictionary RolesDictionaryService => rolesDictionaryService ??= new RolesDictionary();
        private ICustomDateTime dateTimeService;
        public ICustomDateTime DateTimeService => dateTimeService ??= new CustomDateTime();
        private IGenerateRandom generateRandomService;
        public IGenerateRandom GenerateRandomService => generateRandomService ??= new GenerateRandom();
        private IHttpClientBase httpClientService;
        public IHttpClientBase HttpClientService => httpClientService ??= new HttpClientBase(Configuration);
        // -------------------- End Base Service -------------------- //

        private ITransaction transaction;
        public ITransaction Transaction => transaction ??= new Transaction(Db);
        private IRole roleService;
        public IRole RoleService => roleService ??= new Role(RoleManager);
        private IUserRole userRoleService;
        public IUserRole UserRoleService => userRoleService ??= new UserRole(UserManager);
        private IUserAuth userAuthService;
        public IUserAuth UserAuthService => userAuthService ??= new UserAuth(UserManager);
        private IUserManage userManageService;
        public IUserManage UserManageService => userManageService ??= new UserManage(UserManager);
        private ICrud<Evaluation> evaluationService;
        public ICrud<Evaluation> EvaluationService => evaluationService ??= new EvaluationService(EvaluationRepository);
        
        private ICrud<Department> departmentService;
        public ICrud<Department> DepartmentService => departmentService ??= new DepartmentService(DepartmentRepository);
        
        private ICrud<SubDepartment> subDepartmentService;
        public ICrud<SubDepartment> SubDepartmentService => subDepartmentService ??= new SubDepartmentService(SubDepartmentRepository);
        
        private ICrud<DepartmentEmployee> departmentEmployeeService;
        public ICrud<DepartmentEmployee> DepartmentEmployeeService => departmentEmployeeService ??= new DepartmentEmployeeService(DepartmentEmployeeRepository);
    }
}
