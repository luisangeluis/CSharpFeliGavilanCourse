using FeliGavilanApiCourse;
using FeliGavilanApiCourse.Data;
using FeliGavilanApiCourse.Endpoints;
using FeliGavilanApiCourse.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

var builder = WebApplication.CreateBuilder(args);

//SERVICES ZONE - BEGIN
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cache
builder.Services.AddOutputCache();

builder.Services.AddScoped<IGenresRepository, GenresRepository>();

builder.Services.AddAutoMapper(typeof(Program));

//SERVICES ZONE - END

var app = builder.Build();

app.UseOutputCache();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();


app.MapGet("/api/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

app.MapGroup("/genres").MapGenres();

app.Run();



