using DomnerTech.Backend.Application;
using DomnerTech.Backend.Infrastructure;
using DomnerTech.Backend.WorkerService;
using DomnerTech.Backend.WorkerService.Workers;

var builder = Host.CreateApplicationBuilder(args);

// Add application services
builder.Services.AddApplication();

// Configure AppSettings
var appSettings = builder.Configuration.Get<AppSettings>() ?? new AppSettings();

// Add infrastructure services
builder.Services.AddInfrastructure(appSettings);

// Register background workers
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<LeaveAccrualWorker>();
builder.Services.AddHostedService<LeaveCarryForwardWorker>();
builder.Services.AddHostedService<LeaveExpiryWorker>();
builder.Services.AddHostedService<LeaveNotificationWorker>();

var host = builder.Build();
host.Run();
