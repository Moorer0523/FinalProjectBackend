using Microsoft.EntityFrameworkCore;
using anonymous_chats_backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using anonymous_chats_backend;
using System;
using Microsoft.AspNetCore.Diagnostics;
using anonymous_chats_backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AnonymousDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAngularApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200", 
            "https://anonymouschatsfrontend-secondary.z13.web.core.windows.net/", 
            "https://dev-2lj715snuhzz1p1e.us.auth0.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();

    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://dev-2lj715snuhzz1p1e.us.auth0.com/";
    options.Audience = "https://anonymousChatsApi";
});

//Adding custom swagger ui to bypass token automation, you can grab a token from this page: https://manage.auth0.com/dashboard/us/dev-2lj715snuhzz1p1e/apis/66ff1fc6ae0d2ba4dc9469aa/test

builder.Services.AddCustomSwagger();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AnonymousDbContext>();
    dbContext.Database.Migrate();  // Applies any pending migrations to the database
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalAngularApp");
app.UseCors("AllowLocalAngularApp");

app.MapHub<ChatHub>("/ChatHub");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(errorApp => 
{ 
    errorApp.Run(async context => 
    { 
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        if (exception != null)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(exception, "Unhandled exception occurred.");
        } 
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred. Please try again later."); 
    }); 
});

app.MapControllers();

app.Run();
