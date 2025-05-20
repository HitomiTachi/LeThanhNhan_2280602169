using LeThanhNhan_2280602169.Models;
using LeThanhNhan_2280602169.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    // GET: Add product
    public IActionResult Add()
    {
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    // POST: Add product with image upload
    [HttpPost]
    public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
    {
        if (ModelState.IsValid)
        {
            if (imageUrl != null)
            {
                // Save main image
                product.ImageUrl = await SaveImage(imageUrl);
            }

            if (imageUrls != null && imageUrls.Count > 0)
            {
                product.ImageUrls = new List<string>();
                foreach (var file in imageUrls)
                {
                    product.ImageUrls.Add(await SaveImage(file));
                }
            }

            _productRepository.Add(product);
            return RedirectToAction("Index");
        }

        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View(product);
    }

    private async Task<string> SaveImage(IFormFile image)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var filePath = Path.Combine(uploadsFolder, image.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }
        return "/images/" + image.FileName;
    }

    // GET: Display all products
    public IActionResult Index()
    {
        var products = _productRepository.GetAll();
        return View(products);
    }

    // GET: Display a single product
    public IActionResult Display(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // GET: Update form
    public IActionResult Update(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

        return View(product);
    }

    // POST: Process update
    [HttpPost]
    public async Task<IActionResult> Update(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
    {
        if (ModelState.IsValid)
        {
            if (imageUrl != null)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }

            if (imageUrls != null && imageUrls.Count > 0)
            {
                product.ImageUrls = new List<string>();
                foreach (var file in imageUrls)
                {
                    product.ImageUrls.Add(await SaveImage(file));
                }
            }

            _productRepository.Update(product);
            return RedirectToAction("Index");
        }

        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

        return View(product);
    }

    // GET: Confirm delete
    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // POST: Confirmed deletion
    [HttpPost, ActionName("DeleteConfirmed")]
    public IActionResult DeleteConfirmed(int id)
    {
        _productRepository.Delete(id);
        return RedirectToAction("Index");
    }
}