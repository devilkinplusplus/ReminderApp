using Hangfire;
using Moq;
using ReminderApp.Abstractions;
using ReminderApp.Concretes;
using ReminderApp.Entities;
using ReminderApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReminderApp.Test.TestMethods
{
    public class OperationsTests
    {
        [Fact]
        public async Task UpdateMessageAsync_ShouldUpdateTodoWithNewValues()
        {
            // Arrange
            int id = 1;
            string to = "new-recipient@example.com";
            string content = "new-message-content";
            DateTime sendAt = DateTime.Now.AddDays(1);
            MethodType method = MethodType.Email;

            var mockTodoService = new Mock<ITodoService>();
            var mockTodo = new Mock<Todo>();
            mockTodoService.Setup(s => s.GetAsync(id)).ReturnsAsync(mockTodo.Object);

            var service = new Operations(mockTodoService.Object);

            // Act
            await service.UpdateMessageAsync(id, to, content, sendAt, method);

            // Assert
            mockTodoService.Verify(s => s.UpdateAsync(mockTodo.Object), Times.Once);
        }


    }
}
