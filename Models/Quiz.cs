namespace ITPE3200FAM.Models;


public class Quiz
{
    public int QuizId { get; set; }
    public string Title { get; set; } = string.Empty;
    public virtual List<Question> Questions { get; set; } = new();
}