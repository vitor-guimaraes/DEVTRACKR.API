using DEVTRACKR.API.Persistence;
using DEVTRACKR.API.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DevTrackrCs");
builder.Services.AddDbContext<DevTrackRContext>(o => o.UseInMemoryDatabase("DevTrackrCs"));
// builder.Services.AddSingleton<DevTrackRContext>();

// builder.Services.AddDbContext<DevTrackRContext>(o => o.UseSqlServer(connectionString)); //RODAR COM MONGODB

builder.Services.AddScoped<IPackageRepository, PackageRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
