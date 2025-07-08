using Microsoft.AspNetCore.Identity;

namespace PRN222_English_Exam.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string UserId { get; set; }
        public int ExamId { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public Exam Exam { get; set; }
        public Users User { get; set; }
        public ICollection<AnswerDetail> AnswerDetails { get; set; }
    }




}
