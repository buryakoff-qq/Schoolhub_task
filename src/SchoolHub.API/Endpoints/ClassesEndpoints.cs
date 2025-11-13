using SchoolHub.Application.DTOs;
using SchoolHub.Application.Services;

namespace SchoolHub.API.Endpoints;

public static class ClassesEndpoints
{
    public static IEndpointRouteBuilder MapClassesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/classes", async (SchoolClassService service, CancellationToken ct) =>
        {
            return Results.Ok(await service.GetAllAsync(ct));
        });

        endpoints.MapGet("/classes/{id:guid}", async (Guid id, SchoolClassService service, CancellationToken ct) =>
        {
            return Results.Ok(await service.GetByIdAsync(id, ct));
        });

        endpoints.MapPost("/classes", async (
            SchoolClassCreateRequest req,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                var result = await service.CreateAsync(req.Name, req.Teacher, ct);
                return Results.Created($"/classes/{result.Id}", result);
            }
            catch (InvalidOperationException e)
            {
                return Results.NotFound(e.Message);
            }
        });

        endpoints.MapPut("/classes/{id:guid}", async (
            Guid id,
            SchoolClassUpdateRequest req,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                var result = await service.UpdateAsync(id, req.Name, req.Teacher, ct);
                return Results.Ok(result);
            }
            catch (InvalidOperationException e)
            {
                return Results.NotFound(e.Message);
            }
        });

        endpoints.MapDelete("/classes/{id:guid}", async (
            Guid id,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                await service.DeleteAsync(id, ct);
                return Results.NoContent();
            }
            catch (InvalidOperationException e)
            {
                return Results.NotFound(e.Message);
            }
        });

        endpoints.MapPost("/classes/{classId:guid}/assign/{studentId:guid}", async (
            Guid classId,
            Guid studentId,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                await service.AssignAsync(classId, studentId, ct);
                return Results.Ok();
            }
            catch (InvalidOperationException e)
            {
                return e.Message == "Class Not Found" ? Results.NotFound(e.Message) : Results.BadRequest(e.Message);
            }
        });

        endpoints.MapDelete("/classes/{classId:guid}/unassign/{studentId:guid}", async (
            Guid classId,
            Guid studentId,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                await service.UnassignAsync(classId, studentId, ct);
                return Results.NoContent();
            }
            catch (InvalidOperationException e)
            {
                return Results.NotFound(e.Message);
            }
        });
        
        return endpoints;
    }
}