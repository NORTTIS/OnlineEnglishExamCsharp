namespace PRN222_English_Exam.Models
{
    public class AnswerDetail
    {
        public int DetailId { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }

        public Answer Answer { get; set; }
        public Question Question { get; set; }
    }

}
