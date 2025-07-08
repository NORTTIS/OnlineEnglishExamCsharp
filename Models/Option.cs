namespace PRN222_English_Exam.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionText { get; set; }

        public Question Question { get; set; }
    }



}
