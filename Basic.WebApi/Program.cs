using Autofac.Extensions.DependencyInjection;
using Autofac;
using Basic.WebApi;
using System.Reflection;
using Serilog;
using Serilog.Events;

using Microsoft.EntityFrameworkCore;
using Basic.DataAccesEF;

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
    builder.Services.AddSwaggerGen();

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
}
catch (Exception ex) {
    Log.Fatal(ex, "Application start-up failed");

} finally {
    Log.CloseAndFlush();

};
