using Mono.WebAPI;
using Ninject;
using Ninject.Web.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var settings = new NinjectSettings();
// Unfortunately, in .NET Core projects, referenced NuGet assemblies are not copied to the output directory
// in a normal build which means that the automatic extension loading does not work _reliably_ and it is
// much more reasonable to not rely on that and load everything explicitly.
//settings.LoadExtensions = false;

var kernel = new AspNetCoreKernel(settings);

//kernel.Load(typeof(AspNetCoreHostConfiguration).Assembly);
kernel.Load(new ServiceModule());

builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory(kernel));

builder.Services.AddControllers();
builder.Services.AddSingleton<VehicleMakeController>();
builder.Services.AddSingleton<VehicleModelController>();
builder.Services.AddSingleton<VehicleOwnerController>();
builder.Services.AddSingleton<VehicleRegistrationController>();

var app = builder.Build();

//app.UseHttpsRedirection();
//app.UseRouting();
app.MapControllers();
app.Run();