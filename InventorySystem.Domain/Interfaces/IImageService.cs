using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Domain.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveProductImageAsync(IFormFile file);
        Task DeleteImageAsync(string relativePath);
    }
}
