using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Services;
using Schoolhub.Infrastructure.Persistence;
using Schoolhub.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("SchoolHubDb");
});

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ISchoolClassRepository, SchoolClassRepository>();

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<SchoolClassService>();

var app = builder.Build();

app.MapGet("/students", async (StudentService service, CancellationToken ct) =>
{
    return Results.Ok(await service.GetAllAsync(ct));
});

app.MapGet("/students/{id:guid}", async (Guid id, StudentService service, CancellationToken ct) =>
{
    return Results.Ok(await service.GetByIdAsync(id, ct));
});

app.MapGet("/students/by-id/{studentId}", async (
    string studentId,
    StudentService service,
    CancellationToken ct) =>
{
    var student = await service.GetByStudentIdAsync(studentId, ct);
    return Results.Ok(student);
});

app.MapPost("/students", async (
    StudentCreateRequest req,
    StudentService service,
    CancellationToken ct) =>
{
    var result = await service.CreateAsync(
        req.StudentId,
        req.FirstName,
        req.LastName,
        req.BirthDate,
        req.City,
        req.Street,
        req.PostalCode,
        ct);

    return Results.Created($"/students/{result.Id}", result);
});

app.MapPut("/students/{id:guid}", async (
    Guid id,
    StudentUpdateRequest req,
    StudentService service,
    CancellationToken ct) =>
{
    var result = await service.UpdateAsync(
        id,
        req.StudentId,
        req.FirstName,
        req.LastName,
        req.BirthDate,
        req.City,
        req.Street,
        req.PostalCode,
        ct);

    return Results.Ok(result);
});

app.MapDelete("/students/{id:guid}", async (
    Guid id,
    StudentService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
});

app.MapGet("/classes", async (SchoolClassService service, CancellationToken ct) =>
{
    return Results.Ok(await service.GetAllAsync(ct));
});

app.MapGet("/classes/{id:guid}", async (Guid id, SchoolClassService service, CancellationToken ct) =>
{
    return Results.Ok(await service.GetByIdAsync(id, ct));
});

app.MapPost("/classes", async (
    SchoolClassCreateRequest req,
    SchoolClassService service,
    CancellationToken ct) =>
{
    var result = await service.CreateAsync(req.Name, req.Teacher, ct);
    return Results.Created($"/classes/{result.Id}", result);
});

app.MapPut("/classes/{id:guid}", async (
    Guid id,
    SchoolClassUpdateRequest req,
    SchoolClassService service,
    CancellationToken ct) =>
{
    var result = await service.UpdateAsync(id, req.Name, req.Teacher, ct);
    return Results.Ok(result);
});

app.MapDelete("/classes/{id:guid}", async (
    Guid id,
    SchoolClassService service,
    CancellationToken ct) =>
{
    await service.DeleteAsync(id, ct);
    return Results.NoContent();
});

app.MapPost("/classes/{classId:guid}/assign/{studentId:guid}", async (
    Guid classId,
    Guid studentId,
    SchoolClassService service,
    CancellationToken ct) =>
{
    await service.AssignAsync(classId, studentId, ct);
    return Results.Ok();
});

app.MapDelete("/classes/{classId:guid}/unassign/{studentId:guid}", async (
    Guid classId,
    Guid studentId,
    SchoolClassService service,
    CancellationToken ct) =>
{
    await service.UnassignAsync(classId, studentId, ct);
    return Results.NoContent();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();
