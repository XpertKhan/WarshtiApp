using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Helpers
{
    public interface IEmailService
    { 
        SmtpSenderOptions Options { get; }

        Task Execute(string apiKey, string subject, string message, string email);
        Task SendEmailAsync(string email, string subject, string message);
    }
}
