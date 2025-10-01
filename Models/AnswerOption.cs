namespace ITPE3200FAM.Models;

public class AnswerOption
{
    public int AnswerOptionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public virtual Question Question { get; set; } = default!;
}