using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OA.Api.ConfigureServices;
using OA.Data;

namespace delivery.api.ConfigureServices
{
    public class DbService : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            // MySql Connection
            string sqlCon = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(sqlCon), poolSize: 128);
            //services.AddDbContextPool<AppDbContext>(options => options.UseMySql(sqlCon, ServerVersion.AutoDetect(sqlCon)), poolSize: 128);
        }
    }
}
