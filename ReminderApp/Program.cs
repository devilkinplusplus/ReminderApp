using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using ReminderApp.Abstractions;
using ReminderApp.Concretes;
using ReminderApp.Context;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("HangfireServer"));
});

builder.Services.AddTransient<IMailService,MailService>();
builder.Services.AddTransient<ITelegramService,TelegramService>();
builder.Services.AddScoped<ITodoService,TodoService>();
builder.Services.AddScoped<IOperations,Operations>();

builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireServer"));
});
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHangfireDashboard();

app.Run();
