using ReminderApp.Abstractions;
using ReminderApp.Enums;
using System.Net.Mail;
using System.Net;
using System.Text;
using Telegram.Bot.Types;

namespace ReminderApp.Concretes
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ITodoService _todoService;
        public MailService(IConfiguration configuration, ITodoService todoService)
        {
            _configuration = configuration;
            _todoService = todoService;
        }

        public async Task SendMessageAsync(string to, string content)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = true;
            mail.Subject = content;
            mail.To.Add(to);
            mail.From = new(_configuration["Mail:Username"], "Reminder", Encoding.UTF8);

            //Send this mail
            SmtpClient smtpClient = new();
            smtpClient.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Host = _configuration["Mail:Host"];

            await smtpClient.SendMailAsync(mail);

            await _todoService.AddAsync(new()
            {
                To = to,
                Content = content,
                SendAt = DateTime.Now,
                Method = MethodType.Email.ToString(),
            });
        }
    }
}
