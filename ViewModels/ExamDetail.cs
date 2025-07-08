using System.ComponentModel.DataAnnotations;

namespace PRN222_English_Exam.ViewModels
{
    public class ExamDetail
    {
        public int ExamId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public List<QuestionDetail> Questions { get; set; }
    }

    public class QuestionDetail
    {
        [Required(ErrorMessage = "Question text is required.")]
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public List<string> Options { get; set; } = new List<string>();
    }
}
