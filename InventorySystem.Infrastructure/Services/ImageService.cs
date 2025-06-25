using InventorySystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Infrastructure.Services
{
    public class ImageService : IImageService
    {

        #region DeleteImage
        public Task DeleteImageAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return Task.CompletedTask;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.Replace("/", "\\"));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region SaveProductImage
        public async Task<string> SaveProductImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
            var imagePath = Path.Combine(folderPath, imageName);

            Directory.CreateDirectory(folderPath);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"images/products/{imageName}";
        }


        #endregion



    }


}

