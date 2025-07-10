using FeliGavilanApiCourse;
using FeliGavilanApiCourse.Data;
using FeliGavilanApiCourse.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//SERVICES ZONE - BEGIN
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache();

builder.Services.AddScoped<IGenresRepository, GenresRepository>();

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

app.MapPost("/genres", async (Genre genre, IGenresRepository repository) =>
{
    var id = await repository.Create(genre);
    return Results.Created($"/genres{id}", genre);
});

app.MapGet("/genres", async (IGenresRepository repository) =>
{
    var genres = await repository.GetAll();
    return Results.Ok(genres);
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)));

app.MapGet("/genres/{id:int}", async (int id, IGenresRepository repository) =>
{
    var genre = await repository.GetById(id);

    if (genre is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(genre);
});

app.Run();

