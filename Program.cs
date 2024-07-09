using InventoryManagement.Data;
using InventoryManagement.Data.Repositories;
using InventoryManagement.Behaviors;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão não pode ser nula ou vazia.");
}

builder.Services.AddTransient<IProductRepository>(provider => 
    new ProductRepository(connectionString));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<InventoryManagement.Middleware.CqrsLoggingMiddleware>(); 
app.MapControllers();
app.Run();
