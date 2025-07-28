using FeliGavilanApiCourse;
using FeliGavilanApiCourse.Data;
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

genresEndPoints.MapPost("/", Create);
genresEndPoints.MapGet("/", GetGenres)
    .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));
genresEndPoints.MapGet("/{id:int}", GetById);
genresEndPoints.MapPut("/{id:int}", Update);
genresEndPoints.MapDelete("/{id:int}", Delete);

app.Run();

static async Task<Ok<List<Genre>>> GetGenres(IGenresRepository repository)
{
    var genres = await repository.GetAll();
    return TypedResults.Ok(genres);

}

static async Task<Results<Ok<Genre>, NotFound>> GetById(int id, IGenresRepository repository)
{
    var genre = await repository.GetById(id);

    if (genre is null) return TypedResults.NotFound();

    return TypedResults.Ok(genre);
}

static async Task<Created<Genre>> Create(Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore)
{
    var id = await repository.Create(genre);

    await outputCacheStore.EvictByTagAsync("genres-get", default);

    return TypedResults.Created($"/genres/{id}", genre);
}

static async Task<Results<NotFound, NoContent>> Update(int id, Genre genre, IGenresRepository repository, IOutputCacheStore outputCacheStore)
{
    var exists = await repository.Exists(id);

    if (!exists) return TypedResults.NotFound();

    genre.Id = id;

    await repository.Update(genre);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return TypedResults.NoContent();
}

static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository repository, IOutputCacheStore outputCacheStore)
{
    var exists = await repository.Exists(id);

    if (!exists) return TypedResults.NotFound();

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return TypedResults.NoContent();
}

