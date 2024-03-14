using BegumYatch.Core.Models.AdminPanel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BegumYatch.Repository.Configurations
{
    public class TempConfiguration : IEntityTypeConfiguration<Temp>
    {
        public void Configure(EntityTypeBuilder<Temp> builder)
        {
            builder.HasNoKey();
        }
    }
}
