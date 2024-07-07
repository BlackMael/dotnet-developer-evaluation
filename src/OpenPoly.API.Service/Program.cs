using Carter;
using OpenPoly.Core;
using OpenPoly.Core.ClientAPI.OpenApiXML;
using OpenPoly.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICompanyService, CompanyService>();

// Add Kiota handlers to the dependency injection container
builder.Services.AddKiotaHandlers();

// Register the factory for the GitHub client
builder.Services.AddHttpClient<OpenApiClientFactory>((sp, client) =>
    {
        // Is appsetting is missing default to localhost which should end up failing
        var baseAddress = builder.Configuration.GetSection("XmlApi")["BaseAddress"] ?? "https://localhost/missing-app-setting";

        client.BaseAddress = new Uri(baseAddress);
    })
    .AttachKiotaHandlers(); // Attach the Kiota handlers to the http client, this is to enable all the Kiota features.

// Register the GitHub client
builder.Services.AddTransient(sp => sp.GetRequiredService<OpenApiClientFactory>().GetClient());

builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        => await Results.Problem().ExecuteAsync(context)
        )
    );

app.MapCarter();

app.Run();
