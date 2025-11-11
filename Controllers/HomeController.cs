using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ImageUploaderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageUploaderApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _db;
    private readonly S3ImageStorage _storage;

    public HomeController(ILogger<HomeController> logger, AppDbContext db, S3ImageStorage storage)
    {
        _logger = logger;
        _db = db;
        _storage = storage;
    }

    public async Task<IActionResult> Index()
    {
        var images = await _db.Images.OrderByDescending(i => i.UploadDate).ToListAsync();
        return View(images);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return View();
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string url;
        using (var stream = file.OpenReadStream())
        {
            url = await _storage.UploadAsync(stream, fileName, file.ContentType);
        }
        var image = new ImageMeta
        {
            FileName = file.FileName,
            UploadDate = DateTime.UtcNow,
            Url = url
        };
        _db.Images.Add(image);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
