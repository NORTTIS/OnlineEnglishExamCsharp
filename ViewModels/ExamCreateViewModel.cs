using System.ComponentModel.DataAnnotations;
namespace PRN222_English_Exam.ViewModels

{
    public class ExamCreateViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        public int Duration { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }

    public class QuestionViewModel
    {
        [Required(ErrorMessage = "Question text is required.")]
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // "radio", "checkbox", "text"
        public List<string> Options { get; set; } = new();
    }

}
