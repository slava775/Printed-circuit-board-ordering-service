using Testing.Domain.DTOs.Categories;

namespace Testing.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllAsync();
    }
}
