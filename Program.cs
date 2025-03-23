using SenexPontosAPI.Business;
using SenexPontosAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Registro da injeção de dependência
builder.Services.AddScoped<IDapperHelper, DapperHelper>();

builder.Services.AddScoped<EmpresaManager>();
builder.Services.AddScoped<PessoaManager>();
builder.Services.AddScoped<ConsumoManager>();
builder.Services.AddScoped<PontosManager>();
builder.Services.AddScoped<AppManagers>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
