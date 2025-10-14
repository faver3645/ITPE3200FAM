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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var quizzes = await _repo.GetAll();
            return View(quizzes);
        }

        [HttpGet]
        public async Task<IActionResult> Take(int id)
        {
            var quiz = await _repo.GetQuizById(id);
            if (quiz == null)
                return NotFound();

            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int quizId, string userName, Dictionary<string, string> answers)
        {
            var quiz = await _repo.GetQuizById(quizId);
            if (quiz == null)
                return NotFound();

            // SERVER-SIDE VALIDATION
            bool hasErrors = false;

            if (string.IsNullOrWhiteSpace(userName))
            {
                ModelState.AddModelError("userName", "User name is required.");
                hasErrors = true;
            }

            var unansweredQuestions = new List<int>();
            foreach (var question in quiz.Questions)
            {
                if (!answers.ContainsKey(question.QuestionId.ToString()))
                    unansweredQuestions.Add(question.QuestionId);
            }

            if (unansweredQuestions.Any())
            {
                // legg bare til én samlet feil på toppen
                ModelState.AddModelError("", "Please answer all questions before submitting.");
                hasErrors = true;
            }

            // SEND SVAR TILBAKE TIL VIEW
            ViewData["UserName"] = userName;
            ViewData["Answers"] = answers;
            ViewData["Unanswered"] = unansweredQuestions;

            if (hasErrors)
                return View("Take", quiz);

            // BEREGN SCORE
            int score = 0;
            foreach (var question in quiz.Questions)
            {
                if (answers.TryGetValue(question.QuestionId.ToString(), out var selectedOptionIdStr))
                {
                    if (int.TryParse(selectedOptionIdStr, out int selectedOptionId))
                    {
                        var selectedOption = question.AnswerOptions.FirstOrDefault(o => o.AnswerOptionId == selectedOptionId);
                        if (selectedOption != null && selectedOption.IsCorrect)
                            score++;
                    }
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

        [HttpGet]
        public IActionResult Result(QuizResult result)
        {
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Attempts(int id)
        {
            var results = await _repo.GetResultsForQuizAsync(id);
            if (results == null || !results.Any())
                results = new List<QuizResult>();

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
