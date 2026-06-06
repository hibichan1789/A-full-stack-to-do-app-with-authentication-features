using Microsoft.EntityFrameworkCore;
using TodoAuthApi.Context;

var builder = WebApplication.CreateBuilder(args);


// PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyContext>(options => 
    options.UseNpgsql(connectionString)
);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();