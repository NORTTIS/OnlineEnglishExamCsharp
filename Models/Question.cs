namespace PRN222_English_Exam.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int ExamId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // e.g., "checkbox", "radio", "text"

        public Exam Exam { get; set; }
        public ICollection<Option> Options { get; set; }
        public ICollection<AnswerDetail> AnswerDetails { get; set; }
    }


}
