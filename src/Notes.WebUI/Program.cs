using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Notes.Application.Notes;
using Notes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// MVC + FluentValidation + AutoMapper
builder.Services.AddControllersWithViews()
    .AddViewOptions(o => o.HtmlHelperOptions.ClientValidationEnabled = true);

builder.Services.AddValidatorsFromAssembly(typeof(CreateOrUpdateNoteValidator).Assembly);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Notes}/{action=Index}/{id?}");

app.Run();
