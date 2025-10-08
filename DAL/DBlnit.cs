using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ITPE3200FAM.Models;

namespace ITPE3200FAM.DAL
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<QuizDbContext>();

            // Slett og opprett databasen på nytt (kun for utvikling/testing)
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Sjekk om det allerede finnes data
            if (context.Quizzes.Any()) return;

            // Eksempeldata
            var quizzes = new List<Quiz>
            {
                new Quiz
                {
                    Title = "Matematikk",
                    Description = "Test kunnskap i grunnleggende matte",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Hva er 2 + 2?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "3", IsCorrect = false },
                                new Answer { Text = "4", IsCorrect = true },
                                new Answer { Text = "5", IsCorrect = false }
                            }
                        },
                        new Question
                        {
                            Text = "Hva er kvadratroten av 9?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "2", IsCorrect = false },
                                new Answer { Text = "3", IsCorrect = true },
                                new Answer { Text = "4", IsCorrect = false }
                            }
                        }
                    }
                },
                new Quiz
                {
                    Title = "Geografi",
                    Description = "Hvor godt kjenner du verden?",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Text = "Hva er hovedstaden i Norge?",
                            Answers = new List<Answer>
                            {
                                new Answer { Text = "Oslo", IsCorrect = true },
                                new Answer { Text = "Bergen", IsCorrect = false },
                                new Answer { Text = "Trondheim", IsCorrect = false }
                            }
                        }
                    }
                }
            };

            context.Quizzes.AddRange(quizzes);
            context.SaveChanges();
        }
    }
}
