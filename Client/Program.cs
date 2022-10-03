global using Deployf.Botf;
global using Microsoft.EntityFrameworkCore;
global using Domain.Models;
using Infrastructure;
using Infrastructure.Context;

BotfProgram.StartBot(args, onConfigure: (services, config) =>
{
	services.AddInfrastructure();
}, onRun: (app, conf) =>
{
	app.InitDatabaseAsync<ApplicationDbContext>();
});