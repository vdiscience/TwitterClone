using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using TwitterCloneBackend.DDD;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers();

//👇 new code
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    //options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
//    //options.Audience = Configuration["Auth0:Audience"];
//    //https://localhost:7028;http://localhost:5028
//    options.Authority = $"https://localhost:7028";
//    options.Audience = "http://localhost:5028";
//});
//👆 new code



builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ContractResolver =
            new CamelCasePropertyNamesContractResolver());

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<DataContext>
(opt => opt.UseInMemoryDatabase("TwitterClone"));
//(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Twitter Clone", Version = "v1" });

    //👇 new code
    //var securitySchema = new OpenApiSecurityScheme
    //{
    //    Description = "Using the Authorization header with the Bearer scheme.",
    //    Name = "Authorization",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.Http,
    //    Scheme = "bearer",
    //    Reference = new OpenApiReference
    //    {
    //        Type = ReferenceType.SecurityScheme,
    //        Id = "Bearer"
    //    }
    //};

    //c.AddSecurityDefinition("Bearer", securitySchema);

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //      {
    //          { securitySchema, new[] { "Bearer" } }
    //      });
    //👆 new code
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();  //👈 new code
app.UseAuthorization();

app.MapControllers();

app.Run();
