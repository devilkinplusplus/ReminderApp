using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReminderApp.Abstractions;
using ReminderApp.Consts;
using ReminderApp.Entities;
using ReminderApp.Enums;
using ReminderApp.RequestParameters;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IOperations _operations;
        public MainController(IOperations operations)
        {
            _operations = operations;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateMessageParams param)
        {
            _operations.SendMessageAtTime(param.To, param.Content, param.SendAt, (MethodType)Enum.Parse(typeof(MethodType),param.Method));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Read()
        {
            return Ok(await _operations.GetAllTodosAsync());
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateMessageParams param)
        {
            await _operations.UpdateMessageAsync(param.Id,param.To,param.Content,param.SendAt, (MethodType)Enum.Parse(typeof(MethodType), param.Method));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _operations.DeleteAsync(id);
            return NoContent();
        }
    }
}
