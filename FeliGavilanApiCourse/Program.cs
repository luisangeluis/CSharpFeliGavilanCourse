using FeliGavilanApiCourse;
using FeliGavilanApiCourse.Data;
using FeliGavilanApiCourse.Repositories;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

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

var genresEndPoints = app.MapGroup("/genres");

app.MapGet("/api/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

genresEndPoints.MapPost("/", async (Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore) =>
{
    var id = await repository.Create(genre);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.Created($"/genres{id}", genre);
});

genresEndPoints.MapGet("/", async (IGenresRepository repository) =>
{
    var genres = await repository.GetAll();
    return Results.Ok(genres);
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));

genresEndPoints.MapGet("/{id:int}", async (int id, IGenresRepository repository) =>
{
    var genre = await repository.GetById(id);

    if (genre is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(genre);
});

genresEndPoints.MapPut("/{id:int}", async (int id, Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore) =>
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return Results.NotFound();
    }

    genre.Id = id;

    await repository.Update(genre);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.NoContent();

});

genresEndPoints.MapDelete("/{id:int}", async (int id, IGenresRepository repository, IOutputCacheStore outputCacheStore) =>
{
    var exists = await repository.Exists(id);

    if (!exists) return Results.NotFound();

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.NoContent();
});

app.Run();

