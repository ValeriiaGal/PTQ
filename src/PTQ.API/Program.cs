using PTQ.Application.DTOs;
using PTQ.Application.Interfaces;
using PTQ.Application.Services;
using PTQ.Repositories;
using PTQ.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("UniversityDatabase");

builder.Services.AddTransient<IQuizRepository>(_ => new QuizRepository(connStr!));
builder.Services.AddTransient<IPotatoTeacherRepository>(_ => new PotatoTeacherRepository(connStr!));
builder.Services.AddTransient<IQuizService>(_ =>
    new QuizService(
        new QuizRepository(connStr!),
        new PotatoTeacherRepository(connStr!),
        connStr!
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/quizzes", async (IQuizService service) =>
{
    var result = await service.GetAllQuizzesAsync();
    return Results.Ok(result);
});

app.MapGet("/api/quizzes/{id}", async (int id, IQuizService service) =>
{
    var result = await service.GetQuizByIdAsync(id);
    return result == null ? Results.NotFound() : Results.Ok(result);
});

app.MapPost("/api/quizzes", async (CreateQuizDTO dto, IQuizService service) =>
{
    try
    {
        var created = await service.CreateQuizAsync(dto);
        return Results.Created($"/api/quizzes/{created.Id}", created);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();