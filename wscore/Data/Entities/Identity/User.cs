using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WScore.Data.Entities.Identity;
using WScore.Data.Entities.Maintenance;
using WScore.Data.Entities.WScore;

namespace WScore.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public int UserTypeId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public string VerificationToken { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }

        public virtual WorkShopInfo WorkShopInfo { get; set; }
        public virtual UserSetting UserSetting { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<WorkShopImage> WorkShopImages { get; set; }
        public virtual ICollection<Chat> SenderChats { get; set; }
        public virtual ICollection<Chat> ReceiverChats { get; set; }
        public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public User()
        {
            UserRoles = new HashSet<UserRole>();
            UserClaims = new HashSet<UserClaim>();
            UserLogins = new HashSet<UserLogin>();
            UserTokens = new HashSet<UserToken>();
            RefreshTokens = new HashSet<RefreshToken>();
            Announcements = new HashSet<Announcement>();
            Services = new HashSet<Service>();
            Orders = new HashSet<Order>();
            WorkShopImages = new HashSet<WorkShopImage>();
            SenderChats = new HashSet<Chat>();
            ReceiverChats = new HashSet<Chat>();
            UserPaymentMethods = new HashSet<UserPaymentMethod>();
            Notifications = new HashSet<Notification>();
        }
    }
}
