using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq;
using System;

namespace SimpleUsers.Core
{
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
}