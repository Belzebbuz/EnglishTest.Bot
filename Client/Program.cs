global using Deployf.Botf;
global using Microsoft.EntityFrameworkCore;
global using Domain.Models;
using Infrastructure;
using Infrastructure.Context;
using Client.Services;

BotfProgram.StartBot(args, onConfigure: (services, config) =>
{
	services.AddInfrastructure();
	services.AddScoped<IBotUserService, AuthorizeService>();
}, onRun: (app, conf) =>
{
	app.InitDatabaseAsync<ApplicationDbContext>();
});