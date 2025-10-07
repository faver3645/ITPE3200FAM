using MyShop.Models;

namespace MyShop.DAL
{
    public interface QQuizRepository
    {
        Task<IEnumerable<Quiz>> GetALL();
        Task<Item?> GetItemById(int id);
        Task Create(Quiz item);
        Task Update(Quiz item);
        Task<bool> Delete(int id);
    }
}
