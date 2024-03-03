using BegumYatch.Core.Models;
using BegumYatch.Core.Models.BerthRezervation;
using BegumYatch.Core.Models.Demands;
using BegumYatch.Core.Models.Demands.AdminPanel;
using BegumYatch.Core.Models.FileOperations;
using BegumYatch.Core.Models.Orders;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FuelPurchaseDemand> FuelPurchaseDemands { get; set; }
        public DbSet<ConciergeServiceDemand> ConciergeServiceDemands { get; set; }
        public DbSet<ExcursionDemand> ExcursionDemands { get; set; }
        public DbSet<SecurityServiceDemand> SecurityServiceDemands { get; set; }
        public DbSet<FlowerOrder> FlowerOrders { get; set; }
        public DbSet<ProvisionOrder> ProvisionOrders { get; set; }
        public DbSet<BerthRezervation> BerthRezervations { get; set; }
        public DbSet<TechnicalAssitanceandSparePartOrder> TechnicalAssitanceandSparePartOrders { get; set; }
        public DbSet<FileDetail> FileDetails { get; set; }
        public DbSet<CheckIn> CheckIn { get; set; }
        public DbSet<VipServiceDemand> VipDemand { get; set; }
        public DbSet<MailOtp> MailOtp { get; set; }
        public DbSet<AnsweredFuelPurchaseDemand> AnsweredFuelPurchaseDemands 
            { get; set; }


        public override int SaveChanges()
        {
            HandleUserDelete();
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {

                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleUserDelete();
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        private void HandleUserDelete()
        {
            var entities = ChangeTracker.Entries()
                                .Where(e => e.State == EntityState.Deleted);
            foreach (var entity in entities)
            {
                if (entity.Entity is AppUser)
                {
                    entity.State = EntityState.Modified;
                    var user = entity.Entity as AppUser;
                    user.IsDeleted = true;
                }
            }
        }
    }
}
