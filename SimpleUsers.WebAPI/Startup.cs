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
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using SimpleUsers.Core;
using SimpleUsers.Core.Services;
using SimpleUsers.WebAPI.Providers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SimpleUsers.WebAPI
{
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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UserContext>(options => options.UseSqlite(connectionString));            
            
            var tokenValidationParameters = TokenUtil.Create();
            //https://forums.asp.net/t/2105147.aspx?Authorization+using+cookies+for+views+and+bearer+tokens+for+json+results
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<DbContext, UserContext>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                AddXmlComments(c);
            });
        }

        private void AddXmlComments(SwaggerGenOptions c)
		{
			var app = PlatformServices.Default.Application;
            var xm1 = System.IO.Path.Combine(app.ApplicationBasePath, "SimpleUsers.WebAPI.xml");
			c.IncludeXmlComments(xm1);
			var xml2 = System.IO.Path.Combine(app.ApplicationBasePath, "SimpleUsers.Core.xml");
			c.IncludeXmlComments(xml2);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db
            // Loading sample data.
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DbContext>();
                bool hasCreated = dbContext.Database.EnsureCreated();
                if (hasCreated)
                {
                    SampleData.AddData(dbContext);
                }
            }
        }
    }
}
