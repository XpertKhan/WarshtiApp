using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Warshti.Entities;
using Warshti.Entities.Car;
using Warshti.Entities.Maintenance;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Panel.Data
{
    public class DataSeeder
    {
        private readonly WScoreContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DataSeeder(WScoreContext context,
            IWebHostEnvironment env,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            if (!_userManager.Users.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/users-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<User>>(json);
                foreach (var user in temp)
                {
                    await _userManager.CreateAsync(new User
                    {
                        Created = DateTime.Now,
                        Email = user.Email,
                        UserName = user.Email,
                        IsActive = user.IsActive,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        VerificationToken = RandomTokenString(),
                        UserTypeId = user.UserTypeId
                    }, "password");
                }
            }

            if (!_roleManager.Roles.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/roles-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Role>>(json);
                foreach (var role in temp)
                {
                    await _roleManager.CreateAsync(new Role
                    {
                        Created = DateTime.Now,
                        Name = role.Name,
                    });
                }
            }

            if (!_context.Faults.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/faults-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Fault>>(json);

                _context.AddRange(temp);
            }

            if (!_context.Colors.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/colors-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Color>>(json);

                _context.AddRange(temp);
            }

            if (!_context.Models.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/models-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Model>>(json);

                _context.AddRange(temp);
            }

            if (!_context.Companies.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/companies-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Company>>(json);

                _context.AddRange(temp);
            }

            if (!_context.Transmissions.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/transmissions-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Transmission>>(json);

                _context.AddRange(temp);
            }

            if (!_context.Departments.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/departments-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Department>>(json);

                _context.AddRange(temp);
            }

            if (!_context.Languages.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/languages-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<Language>>(json);

                _context.AddRange(temp);
            }

            if (!_context.PaymentMethods.Any())
            {
                var path = Path.Combine(_env.ContentRootPath, "Data/SeedData/paymentmethods-data.json");
                var json = File.ReadAllText(path);
                var temp = JsonConvert.DeserializeObject<IEnumerable<PaymentMethod>>(json);

                _context.AddRange(temp);
            }

            _context.SaveChanges();
        }

        private static string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
