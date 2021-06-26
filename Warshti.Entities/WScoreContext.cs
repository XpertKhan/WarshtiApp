using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WScore.Entities;
using Warshti.Entities.Configurations;
using WScore.Entities.Identity;
using Warshti.Entities.WScore;
using Warshti.Entities.Car;
using Warshti.Entities.Maintenance;
using Warshti.Entities.Identity;
using Warshti.Entities.Entities;
using Warshti.Entities.Entities.Maintenance;
using Warshti.Entities.Entities.Car;

namespace Warshti.Entities
{
    public class WScoreContext : IdentityDbContext<User, Role, int,
        UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly IConfiguration _config;
        public WScoreContext (DbContextOptions<WScoreContext> options,
            IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("WStoreConnection"));
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ASP.NET Core Identity
            modelBuilder.ApplyConfiguration(new UserSettingConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());

            //modelBuilder.ApplyConfiguration(new AnnouncementConfiguration());
            //modelBuilder.ApplyConfiguration(new AnnouncementImageConfiguration());

            modelBuilder.ApplyConfiguration(new ColorConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new FaultConfiguration());
            modelBuilder.ApplyConfiguration(new ModelConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceFaultConfiguration());
            modelBuilder.ApplyConfiguration(new TransmissionConfiguration());

            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStepConfiguration());
            modelBuilder.ApplyConfiguration(new WorkShopInfoConfiguration());
            modelBuilder.ApplyConfiguration(new WorkShopImageConfiguration());

            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new UserPaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new FaqConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceImageConfiguration());
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementImage> AnnouncementImages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Fault> Faults { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceFault> ServiceFaults { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStep> OrderSteps { get; set; }
        public DbSet<WorkShopInfo> WorkShopInfos { get; set; }
        public DbSet<WorkShopImage> WorkShopImages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<UserPaymentMethod> UserPaymentMethods { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<DeviceToken> DeviceTokens { get; set; }
        public DbSet<ServiceImage> ServiceImages { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionImage> QuestionImages { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<CarInformation> CarsInformation { get; set; }

    }
}
