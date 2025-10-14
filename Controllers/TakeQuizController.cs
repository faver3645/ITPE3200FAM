using Microsoft.AspNetCore.Mvc;
using ITPE3200FAM.Models;
using ITPE3200FAM.DAL;

namespace ITPE3200FAM.Controllers
{
    public class TakeQuizController : Controller
    {
        private readonly IQuizRepository _repo;
        private readonly ILogger<TakeQuizController> _logger;

        public TakeQuizController(IQuizRepository repo, ILogger<TakeQuizController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: /TakeQuiz/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var quizzes = await _repo.GetAll();
            if (quizzes == null)
            {
                _logger.LogError("[TakeQuizController] No quizzes found when executing _repo.GetAll()");
                return NotFound("Quiz list not found");
            }
            return View(quizzes);
        }

        // GET: /TakeQuiz/Take/5
        [HttpGet]
        public async Task<IActionResult> Take(int id)
        {
            var quiz = await _repo.GetQuizById(id);
            if (quiz == null)
            {
                _logger.LogError("[TakeQuizController] Quiz not found for QuizId {QuizId:0000}", id);
                return NotFound("Quiz not found");
            }

            return View(quiz);
        }

        // POST: /TakeQuiz/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int quizId, string userName, Dictionary<int, int> answers)
        {
            var quiz = await _repo.GetQuizById(quizId);
            if (quiz == null)
            {
                _logger.LogError("[TakeQuizController] Quiz not found for QuizId {QuizId:0000} during submission", quizId);
                return NotFound("Quiz not found");
            }

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

        // POST: /TakeQuiz/SaveAttempt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAttempt(int quizId, string userName, int score)
        {
            var quiz = await _repo.GetQuizById(quizId);
            if (quiz == null)
            {
                _logger.LogError("[TakeQuizController] Quiz not found for QuizId {QuizId:0000} when saving attempt", quizId);
                return NotFound("Quiz not found");
            }

            var result = new QuizResult
            {
                UserName = userName,
                QuizId = quiz.QuizId,
                Quiz = quiz,
                Score = score
            };

            try
            {
                await _repo.AddResultAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("[TakeQuizController] Failed to save attempt {@Result}, error: {Error}", result, ex.Message);
                return BadRequest("Failed to save attempt");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /TakeQuiz/Attempts/5
        [HttpGet]
        public async Task<IActionResult> Attempts(int id) // id = QuizId
        {
            IEnumerable<QuizResult> results;
            try
            {
                results = await _repo.GetResultsForQuizAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("[TakeQuizController] Failed to get attempts for QuizId {QuizId:0000}, error: {Error}", id, ex.Message);
                return BadRequest("Failed to load attempts");
            }

            if (results == null || !results.Any())
                results = new List<QuizResult>();

            return View(results);
        }

        // GET: /TakeQuiz/Result
        [HttpGet]
        public IActionResult Result(QuizResult result)
        {
            if (result == null)
            {
                _logger.LogWarning("[TakeQuizController] Result is null when accessing Result view");
                return BadRequest("Result not found");
            }
            return View(result);
        }
    }
}