using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Utils;
using BR.Services.Interfaces;

namespace BR.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }


        public async Task SendAsync(string recipentMail, string subject, string body)
        {
            var message = new MimeMessage();
            try
            {
                message.From.Add(new MailboxAddress("Baku Reservation", _emailConfiguration.SmtpUsername));
                message.To.Add(new MailboxAddress("", recipentMail));
                message.Subject = subject;
                //message.Body = new TextPart(TextFormat.Html : TextFormat.Text)
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                using (var emailClient = new SmtpClient())
                {
                    
                    await emailClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                    await emailClient.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                    await emailClient.SendAsync(message);
                    await emailClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
