using ITPE3200FAM.Models;

namespace ITPE3200FAM.DAL;

public interface IQuizRepository
{
	Task<IEnumerable<Quiz>> GetAll();
    Task<Quiz?> GetQuizById(int id);
	Task Create(Quiz quiz);
    Task Update(Quiz quiz);
    Task<bool> Delete(int id);
}