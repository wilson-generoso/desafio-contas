using Desafio.Contas.Api.Mappings;
using Desafio.Contas.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Desafio.Contas.Application.xml"));
    options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Desafio.Contas.Domain.xml"));
});
builder.Services.AddApplication();
builder.Services.AddInfrastructureMongoDB(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorMiddleware>();

app.MapAccountEndpoints();
app.MapEntryEndpoints();

app.Run();
