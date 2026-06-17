using GLMS.Data;
using GLMS_V3.API.Interfaces;
using GLMS_V3.API.Repositories;
using GLMS_V3.API.Services;
using GLMS_V3_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<
    IContractRepository,
    ContractRepository>();

builder.Services.AddScoped<
    IContractService,
    ContractService>();

builder.Services.AddScoped<
    IClientRepository,
    ClientRepository>();

builder.Services.AddScoped<
    IClientService,
    ClientService>();

builder.Services.AddScoped<
    IServiceRequestRepository,
    ServiceRequestRepository>();

builder.Services.AddScoped<
    IServiceRequestService,
    ServiceRequestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
