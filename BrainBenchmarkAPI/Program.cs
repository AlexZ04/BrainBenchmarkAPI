using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DbContext>(options => options.UseNpgsql(connection));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var serviceScope = app.Services.CreateScope();

var context = serviceScope.ServiceProvider.GetService<DbContext>();

context?.Database.Migrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
