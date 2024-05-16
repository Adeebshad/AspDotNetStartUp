using Autofac.Extensions.DependencyInjection;
using Autofac;
using Basic.WebApi;
using System.Reflection;
using Serilog;
using Serilog.Events;

using Microsoft.EntityFrameworkCore;
using Basic.DataAccesEF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Autofac.Core;
using Microsoft.OpenApi.Models;

try {
    Log.Information("Application Starting ...");
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var assemblyName = Assembly.GetExecutingAssembly().FullName;

    // DbContext Setup for Entity Framework

    //builder.Services.AddDbContext<PeopleContext>(options =>
    //    options.UseSqlServer(connectionString));

    builder.Services.AddDbContext<ItemContext>(options =>
       options.UseSqlServer(connectionString));
    // JWT 



    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    });


    // Dependency add as Autofac
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());  // declear autofac as dependency Injection
    // All Module 
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>  // ki ki module add korbo
    {
        containerBuilder.RegisterModule(new WebModule());
        //containerBuilder.RegisterModule(new InfrastructureModule(connectionString,
        //        assemblyName));
    });

    // SeriLog Configuratoin

    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."

        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
    });

    builder.Services.AddAuthorization();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowOrigin");

    app.MapControllers();

    app.Run();
}
catch (Exception ex) {
    Log.Fatal(ex, "Application start-up failed");

} finally {
    Log.CloseAndFlush();

};
