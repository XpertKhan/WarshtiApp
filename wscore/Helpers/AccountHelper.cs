using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace WScore.Helpers
{
    public class AccountHelper
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration config;

        public AccountHelper(IEmailService emailService, IConfiguration _config)
        {
            _emailService = emailService;
            config = _config;
        }

        public static string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public async Task SendVerificationEmail(User account)
        {
            string origin = config["IsLocal"].ToString() == "True" ? config["LocalDomain"].ToString() : config["LiveDomain"].ToString();
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/api/accounts/verify-email?token={account.ResetToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

           await _emailService.SendEmailAsync(
                email: account.Email,
                subject: "Sign-up Verification API - Verify Email",
                message: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }
    }
}
