using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL
{
    public class QuizRepository
    {
        private readonly QuizDbContext _db;

        public QuizRepository(QuizDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Quiz>> GetAllAsync()
        {
            return await _db.Quizzes.ToListAsync();
        }

        public async Task<Quiz?> GetQuizByIdAsync(int id)
        {
            return await _db.Quizzes.FindAsync(id);
        }

        public async Task CreateAsync(Quiz quiz)
        {
            _db.Quizzes.Add(quiz);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Quiz quiz)
        {
            _db.Quizzes.Update(quiz);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var quiz = await _db.Quizzes.FindAsync(id);
            if (quiz == null)
                return false;

            _db.Quizzes.Remove(quiz);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
