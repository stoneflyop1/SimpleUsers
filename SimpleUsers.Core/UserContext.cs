using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq;
using System;
using SimpleUsers.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SimpleUsers.Core
{
    #pragma warning disable 1591
    
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options):base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
            // https://stackoverflow.com/questions/59134406/unable-to-track-an-entity-of-type-because-primary-key-property-id-is-null
            var keysProperties = modelBuilder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }
        }
    }

    public static class SampleData
    {
        public static int AddData(DbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var user = new User
            {
                UserName = "admin",
                PasswordHash = "1",
                Name = "管理员",
                Mobile = "15900000000",
                Email = "test@test.com"
            };
            context.Set<User>().Add(user);
            return context.SaveChanges();
        }
    }
}