using Testing.Domain.Entities;

namespace Testing.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
    }
}
