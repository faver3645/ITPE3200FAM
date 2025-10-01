using Microsoft.AspNetCore.Mvc;        // For Controller, IActionResult, HttpPost, etc.
using Microsoft.EntityFrameworkCore;   // For Include(), ThenInclude()
using QuizApp.Models;                  // For QuizDbContext, Quiz, Question, AnswerOption, UserQuizResult
using System.Collections.Generic;      // For Dictionary<int, int>
using System.Linq;                     // For FirstOrDefault()
 
 
 
public class QuizController : Controller
{
    private readonly QuizDbContext _context;
 
    public QuizController(QuizDbContext context)
    {
        _context = context;
    }
 
    // List all quizzes
    public IActionResult Index() => View(_context.Quizzes.ToList());
 
    // Create quiz
    [HttpGet]
    public IActionResult Create() => View();
 
    [HttpPost]
    public IActionResult Create(Quiz quiz)
    {
        if (ModelState.IsValid)
        {
            _context.Quizzes.Add(quiz);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(quiz);
    }
 
    // Edit, Delete, Details: samme prinsipp
}