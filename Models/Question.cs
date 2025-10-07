namespace ITPE3200FAM.Models;


public class Question
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public List<AnswerOption> AnswerOptions { get; set; } = new();
}