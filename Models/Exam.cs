namespace PRN222_English_Exam.Models
{
    public class Exam
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }  // theo phút
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }

}
