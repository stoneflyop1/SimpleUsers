using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq;
using System;
using SimpleUsers.Core.Entities;

namespace SimpleUsers.Core
{
    #pragma warning disable 1591
    
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options):base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //https://stackoverflow.com/questions/503263/how-to-determine-if-a-type-implements-a-specific-generic-interface-type
            var typesToRegister = assembly.GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.GetInterfaces().Any(x => x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
            foreach (var type in typesToRegister)
            {
                // https://stackoverflow.com/questions/22864790/using-system-dynamic-in-roslyn
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }

    public class SampleData
    {
        public static void AddData(DbContext context)
        {
            var user = new User
            {
                UserName = "admin",
                PasswordHash = "1",
                Name = "管理员",
                Mobile = "15900000000",
                Email = "test@test.com"
            };
            context.Set<User>().Add(user);
            context.SaveChanges();
        }
    }
}