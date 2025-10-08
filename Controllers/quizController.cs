using Microsoft.AspNetCore.Mvc;
using ITPE3200FAM.Models;
using ITPE3200FAM.DAL;

namespace ITPE3200FAM.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository _repo;

        public QuizController(IQuizRepository repo)
        {
            _repo = repo;
        }

        // GET: /Quiz
        public async Task<IActionResult> Index()
        {
            var quizzes = await _repo.GetAll();
            return View(quizzes);
        }

        // GET: /Quiz/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var quiz = await _repo.GetQuizById(id);
            if (quiz == null)
                return NotFound();

            return View(quiz);
        }

        // GET: /Quiz/Create
        [HttpGet]
        public IActionResult Create()
        {
            var quiz = new Quiz
            {
                Questions = new List<Question>
                {
                    new Question
                    {
                        AnswerOptions = new List<AnswerOption>
                        {
                            new AnswerOption(),
                            new AnswerOption()
                        }
                    }
                }
            };
            return View(quiz);
        }

        // POST: /Quiz/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            if (!ModelState.IsValid)
                return View(quiz);

            await _repo.Create(quiz);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Quiz/Update/5
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var quiz = await _repo.GetQuizById(id);
            if (quiz == null)
                return NotFound();

            return View(quiz);
        }

        // POST: /Quiz/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Quiz quiz)
        {
            if (!ModelState.IsValid)
                return View(quiz);

            await _repo.Update(quiz);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Quiz/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _repo.GetQuizById(id);
            if (quiz == null)
                return NotFound();

            return View(quiz);
        }

        // POST: /Quiz/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleted = await _repo.Delete(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}