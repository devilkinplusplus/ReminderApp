using Microsoft.Extensions.Configuration;
using Moq;
using ReminderApp.Abstractions;
using ReminderApp.Concretes;
using ReminderApp.Consts;
using ReminderApp.Entities;
using ReminderApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ReminderApp.Test.TestMethods
{
    public class ServicesTests
    {
        [Fact]
        public async Task SendMessageAsync_ShouldSendTextMessage_AndAddToTodoService()
        {
            // Arrange
            string to = TelegramApiInformations.ChatId;
            string content = "message content";
            DateTime sendAt = DateTime.Now;
            var mockTodoService = new Mock<ITodoService>();
            var telegramService = new TelegramService(mockTodoService.Object);

            // Act
            await telegramService.SendMessageAsync(to, content);

            // Assert
            mockTodoService.Verify(s => s.AddAsync(It.Is<Todo>(item =>
                item.To == to &&
                item.Content == content &&
                item.SendAt >= sendAt &&
                item.Method == MethodType.Telegram.ToString())), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendEmail_AndAddToTodoService()
        {
            // Arrange
            string to = "recipient@example.com";
            string content = "message content";
            
            var mockTodoService = new Mock<ITodoService>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockConfiguration.Setup(c => c["Mail:Username"]).Returns("rufullayevilkin66@gmail.com");
            mockConfiguration.Setup(c => c["Mail:Password"]).Returns("ufklqnxiqzqkvzei");
            mockConfiguration.Setup(c => c["Mail:Host"]).Returns("smtp.gmail.com");

            var emailService = new MailService(mockConfiguration.Object,mockTodoService.Object);

            // Act
            await emailService.SendMessageAsync(to, content);

            // Assert
            mockTodoService.Verify(s => s.AddAsync(It.Is<Todo>(item =>
                item.To == to &&
                item.Content == content &&
                item.SendAt >= It.IsAny<DateTime>() &&
                item.Method == MethodType.Email.ToString())), Times.Once);
        }

    }
}
