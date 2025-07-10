using System.ComponentModel.DataAnnotations;
namespace PRN222_English_Exam.ViewModels

{
    public class ExamCreateViewModel
    {
        public int? ExamId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }

    public class QuestionViewModel
    {
        [Required(ErrorMessage = "Question text is required.")]
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // "radio", "checkbox", "text"

        public int? SingleChoiceAnswer { get; set; } // for radio
        public List<int> MultipleChoiceAnswer { get; set; } = new List<int>(); // for checkbox
        public string? TextAnswer { get; set; } // for text input
        public List<OptionViewModel> Options { get; set; } = new();
    }

    public class OptionViewModel
    {
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; } // for marking correct answers

    }
}
