using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleUsers.Core.Entities;

namespace SimpleUsers.Core.Data
{
    #pragma warning disable 1591
    
    public class UserMap : IEntityTypeConfiguration<User>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.UserName).IsRequired();
            builder.HasIndex(b => b.UserName).IsUnique(true);
            builder.Property(b => b.PasswordHash).IsRequired();
        }
    }
}