using Microsoft.AspNetCore.Mvc;
using ITPE3200FAM.Models;
using ITPE3200FAM.DAL;

namespace ITPE3200FAM.Controllers
{
    public class TakeQuizController : Controller
    {
        private readonly IQuizRepository _repo;

        public TakeQuizController(IQuizRepository repo)
        {
            _repo = repo;
        }

        // GET: /TakeQuiz/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var quizzes = await _repo.GetAll();
            return View(quizzes);
        }

        // GET: /TakeQuiz/Take/5
        [HttpGet]
        public async Task<IActionResult> Take(int id)
        {
            var quiz = await _repo.GetQuizById(id);
            if (quiz == null)
                return NotFound();

            return View(quiz);
        }

        // POST: /TakeQuiz/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int quizId, string userName, Dictionary<int, int> answers)
        {
            var quiz = await _repo.GetQuizById(quizId);
            if (quiz == null)
                return NotFound();

            int score = 0;
            foreach (var question in quiz.Questions)
            {
                if (answers.ContainsKey(question.QuestionId))
                {
                    var selectedOption = question.AnswerOptions
                        .FirstOrDefault(o => o.AnswerOptionId == answers[question.QuestionId]);

                    if (selectedOption != null && selectedOption.IsCorrect)
                        score++;
                }
            }

            var result = new QuizResult
            {
                UserName = userName,
                QuizId = quiz.QuizId,
                Quiz = quiz,
                Score = score
            };

            return View("Result", result);
        }

        // GET: /TakeQuiz/Result
        [HttpGet]
        public IActionResult Result(QuizResult result)
        {
            return View(result);
        }

        // GET: /TakeQuiz/Attempts/5
        [HttpGet]
        public async Task<IActionResult> Attempts(int id) // id = QuizId
        {
            var results = await _repo.GetResultsForQuizAsync(id);

            if (results == null || !results.Any())
                results = new List<QuizResult>(); // alltid send en tom liste i stedet for null

            return View(results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttempt(int quizId, string userName, int score)
        {
            var quiz = await _repo.GetQuizById(quizId);
            if (quiz == null)
                return NotFound();

            var result = new QuizResult
            {
                UserName = userName,
                QuizId = quiz.QuizId,
                Quiz = quiz,
                Score = score
            };

            await _repo.AddResultAsync(result);

            return RedirectToAction(nameof(Index));
        }
    }
}