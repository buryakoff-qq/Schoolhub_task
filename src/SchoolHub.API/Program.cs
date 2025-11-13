using Scalar.AspNetCore;
using SchoolHub.API.Endpoints;
using SchoolHub.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSchoolHubServices();

var app = builder.Build();

app.MapStudentEndpoints();
app.MapClassesEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();