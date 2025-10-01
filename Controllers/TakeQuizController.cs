using Microsoft.EntityFrameworkCore;
using MyShop.Models;
 
namespace MyShop.Controllers
{
    public class TakeQuizController : Controller
    {
        private readonly ItemDbContext _context;
 
        public TakeQuizController(ItemDbContext context)
        {
            _context = context;
        }
 
        // GET: /TakeQuiz/Take/5
        [HttpGet]
        public async Task<IActionResult> Take(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.QuizId == id);
 
            if (quiz == null) return NotFound();
 
            return View(quiz);
        }
 
        // POST: /TakeQuiz/Submit
        [HttpPost]
        public async Task<IActionResult> Submit(int quizId, string userName, Dictionary<int, int> answers)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);
 
            if (quiz == null) return NotFound();
 
            int score = 0;
            foreach (var q in quiz.Questions)
            {
                if (answers.ContainsKey(q.QuestionId))
                {
                    var selectedOption = q.AnswerOptions.FirstOrDefault(o => o.AnswerOptionId == answers[q.QuestionId]);
                    if (selectedOption != null && selectedOption.IsCorrect)
                    {
                        score++;
                    }
                }
            }
 
            var result = new UserQuizResult
            {
                UserName = userName,
                Quiz = quiz,
                Score = score
            };
 
            return View("Result", result);
        }
 
        // GET: /TakeQuiz/Result (kun for redirecting)
        [HttpGet]
        public IActionResult Result(UserQuizResult result)
        {
            return View(result);
        }
    }
}