using LeThanhNhan_2280602169.Models;

namespace LeThanhNhan_2280602169.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }
}
