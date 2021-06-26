using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WScore.Helpers
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptions<SmtpSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public SmtpSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute("", subject, message, email);
        }

        //public Task Execute(string apiKey, string subject, string message, string email)
        //{
           

        //    var client = new SendGridClient(apiKey);
        //    var msg = new SendGridMessage()
        //    {
        //        From = new EmailAddress("salman1277@gmail.com", Options.SendGridUser),
        //        Subject = subject,
        //        PlainTextContent = message,
        //        HtmlContent = message
        //    };
        //    msg.AddTo(new EmailAddress(email));

        //    // Disable click tracking.
        //    // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        //    msg.SetClickTracking(false, false);

        //    return client.SendEmailAsync(msg);
        //}

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            try
            {
                var senderEmail = new MailAddress(Options.SmtpEmail);
                var recieverEmail = new MailAddress(email, "Reciever");
                var smtpClient = new SmtpClient();
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(senderEmail.Address, Options.SmtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Host = "win08.tmd.cloud";
                smtpClient.Port = 25;
                var mess = new MailMessage(senderEmail, recieverEmail)
                {

                    Subject = subject,
                    Body = message

                };
                await smtpClient.SendMailAsync(mess);
            }
            catch(Exception ex)
            {
                throw ex;
            }
          
          
            
             
        }
    }
}
