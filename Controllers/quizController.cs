using Microsoft.AspNetCore.Mvc;
namespace ITPE3200FAM.Controllers;
using Microsoft.EntityFrameworkCore;
using ITPE3200FAM.Models;
using ITPE3200FAM.DAL;

    public class QuizController : Controller
    {
        private readonly QuizDbContext _context;

        public QuizController(QuizDbContext context)
        {
            _context = context;
        }

        // GET: /Quiz
        public async Task<IActionResult> Index()
        {
            var quizzes = await _context.Quizzes.ToListAsync();
            return View(quizzes);
        }

        // GET: /Quiz/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.QuizId == id);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // GET: /Quiz/Create
       [HttpGet]
        public IActionResult Create()
        {
            var quiz = new Quiz();
            // For å vise ett tomt spørsmål med to svaralternativer som start
            quiz.Questions.Add(new Question
            {
                AnswerOptions = new List<AnswerOption>
                {
                    new AnswerOption(),
                    new AnswerOption()
                }
            });
            return View(quiz);
        }

        // POST: /Quiz/Create
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                // Legger til quiz med tilhørende spørsmål og svaralternativer
                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: /Quiz/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.QuizId == id);

            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // POST: /Quiz/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _context.Quizzes.Update(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quiz);
        }

        // GET: /Quiz/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();

            return View(quiz);
        }

        // POST: /Quiz/DeleteConfirmed/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }

    