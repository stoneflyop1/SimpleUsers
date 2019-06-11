using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using SimpleUsers.Core;
using SimpleUsers.Core.Services;
using SimpleUsers.WebAPI.Providers;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;

namespace SimpleUsers.WebAPI
{
    #pragma warning disable 1591
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // EF数据库配置
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (connectionString == null)
            {
                throw new Exception("ConnectionString is null...");
            }
            services.AddDbContext<UserContext>(options => options.UseMySql(connectionString));
            
            // 添加认证，此处使用Bearer的Jwt Token
            //https://forums.asp.net/t/2105147.aspx?Authorization+using+cookies+for+views+and+bearer+tokens+for+json+results            
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = TokenUtil.Create();
            });

            // MVC设置，此处使用camelCase的Json格式
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options => 
                options.SerializerSettings.ContractResolver = 
                    new CamelCasePropertyNamesContractResolver());
            
            // 添加Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                AddXmlComments(c);
            });


            // 添加自定义服务接口及实现
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<DbContext, UserContext>();
        }

        private void AddXmlComments(SwaggerGenOptions c)
		{
			var appPath = System.AppContext.BaseDirectory; //PlatformServices.Default.Application;
            var xm1 = System.IO.Path.Combine(appPath, "SimpleUsers.WebAPI.xml");
			c.IncludeXmlComments(xm1);
			var xml2 = System.IO.Path.Combine(appPath, "SimpleUsers.Core.xml");
			c.IncludeXmlComments(xml2);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // 使用认证
            app.UseAuthentication();
            // 使用MVC
            app.UseMvc();

            // 使用Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // // 程序启动时，添加可能的示例数据
            // // https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db
            // using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            // {
            //    var dbContext = serviceScope.ServiceProvider.GetService<DbContext>();
            //    bool hasCreated = dbContext.Database.EnsureCreated();
            //    if (hasCreated) // 仅在第一次创建数据库时添加样例数据
            //    {
            //        SampleData.AddData(dbContext);
            //    }
            // }
        }
    }
}
