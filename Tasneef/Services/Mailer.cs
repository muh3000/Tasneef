using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tasneef.Data;
using System.Net.Mail;
using System.Net;

namespace Tasneef.Utilities
{
    public class Mailer : IEmailSender
    {
        private readonly ILogger<Mailer> _logger;

        private readonly ApplicationDbContext _context;
        public Mailer(ILogger<Mailer> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task SendEmailAsync(string ToAddress, string emailSubject, string emailBody)
        {
            //var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var emailConfig = _context.EmailConfigurations.FirstOrDefault(e => e.Id == "1");
            if (emailConfig != null)
            {
                //string fromEmail = "muh3000@gmail.com";
                //string password = "Muh@207086";
                //string smtpServer = "smtp.gmail.com";
                //var client = new SendGridClient(apiKey);
                //var from = new EmailAddress("mebcowebedi@gmail.com");
                //var to = new EmailAddress(ToAddress);
                ////var plainTextContent = "and easy to do anywhere, even with C#";
                //var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, emailBody, emailBody);

                try
                {

                    string toEmail = string.IsNullOrEmpty(ToAddress)
                                     ? emailConfig.FromEmail
                                     : ToAddress;
                    MailMessage mail = new MailMessage()
                    {
                        From = new MailAddress(emailConfig.FromEmail, emailConfig.SenderName)
                    };
                    mail.To.Add(new MailAddress(toEmail));
                    //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                    mail.Subject = "مكتب التصنيف - " + emailSubject;
                    mail.Body = emailBody;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;

                    using (SmtpClient smtp = new SmtpClient(emailConfig.SMTPServer))
                    {
                        smtp.Port = emailConfig.SMTPPort;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(emailConfig.SMTPUername, emailConfig.SMTPPassword);
                        smtp.EnableSsl = emailConfig.EnableSSL;
                        
                        await smtp.SendMailAsync(mail);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Email Sender Failed. " + ex.Message);
                }
            }




        }
    }
}
