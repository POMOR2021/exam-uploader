using ImageUploaderApp.Models;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;

var builder = WebApplication.CreateBuilder(args);

// Подключение к PostgreSQL через строку подключения (env/config)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// S3 клиент
builder.Services.AddSingleton<IAmazonS3>(_ => {
    var config = builder.Configuration;
    return new AmazonS3Client(
        config["S3:AccessKey"],
        config["S3:SecretKey"],
        new AmazonS3Config {
            ServiceURL = config["S3:ServiceUrl"],
            ForcePathStyle = true
        }
    );
});
builder.Services.AddSingleton<S3ImageStorage>(sp => {
    var config = builder.Configuration;
    return new S3ImageStorage(
        sp.GetRequiredService<IAmazonS3>(),
        config["S3:BucketName"],
        config["S3:Region"]
    );
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
