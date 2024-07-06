using System.Text;
using bibliotecaAPI.services;
using bibliotecaDataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString= builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AplicationDBContext>(options => 
                                options.UseSqlServer(connectionString));

builder.Services.AddSingleton<encrptationAndAuthentication>();
//Configuro para que pueda usar el jwt mientras se corre el proyecto
builder.Services.AddAuthentication(config =>{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} ).AddJwtBearer(config =>{
    config.RequireHttpsMetadata= false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false, //valida la app en cuanto a url si queremos usarla como una verificacion para que pueda entrar otra app externa
        ValidateAudience = false, //quienes pueden acceder
        ValidateLifetime= true,// valida el tiempo de vida del token
        ClockSkew= TimeSpan.Zero,// validar el tiempo
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>{
        app
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("NewPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
