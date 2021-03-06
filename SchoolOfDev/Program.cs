using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolOfDev.Helpers;
using SchoolOfDev.Middleware;
using SchoolOfDev.Middlewares;
using SchoolOfDev.Profiles;
using SchoolOfDev.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<INoteService, NoteService>();

var mapperConfig = MapperConfig.GetMapperConfig();
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
