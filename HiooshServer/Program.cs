﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HiooshServer.Data;
using HiooshServer.Services;
var builder = WebApplication.CreateBuilder(args);

/*
builder.Services.AddDbContext<HiooshServerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HiooshServerContext") ?? throw new InvalidOperationException("Connection string 'HiooshServerContext' not found.")));
*/

// indection
builder.Services.AddSingleton<IRatingsService, RatingService>();

builder.Services.AddSingleton<IContactsService, ContactService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(40);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseCors("Allow All");

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseCors("Allow All");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ratings}/{action=Index}/{id?}");

app.Run();
