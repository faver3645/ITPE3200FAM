using Microsoft.EntityFrameworkCore;
using ITPE3200FAM.Models;

namespace ITPE3200FAM.DAL
{
    public class QuizRepository : IQuizRepository
    {
        private readonly QuizDbContext _db;

        public QuizRepository(QuizDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Quiz>> GetAll()
        {
            return await _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.AnswerOptions)
                .ToListAsync();
        }

        public async Task<Quiz?> GetQuizById(int id)
        {
            return await _db.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.AnswerOptions)
                .FirstOrDefaultAsync(q => q.QuizId == id);
        }

        public async Task Create(Quiz quiz)
        {
            _db.Quizzes.Add(quiz);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Quiz quiz)
        {
            _db.Quizzes.Update(quiz);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
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
