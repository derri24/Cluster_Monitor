using ClusterMonitor.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IExperimentService, ExperimentService>();
builder.Services.AddScoped<IClusterService, ClusterService>();
builder.Services.AddScoped<ICameraService, CameraService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();