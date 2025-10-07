using MyShop.Models;

namespace MyShop.DAL
{
    public interface QQuizRepository
    {
        Task<IEnumerable<Quiz>> GetALL();
        Task<Quiz?> GetItemById(int id);
        Task Create(Quiz item);
        Task Update(Quiz item);
        Task<bool> Delete(int id);
    }
}
