using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSinavPortali.Models
{
    public class StudentAnswers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentAnswerID { get; set; }
        public string StudentID { get; set; }
        public int QuestionID { get; set;}
        public int ExamID { get; set;}  
        public string? AnswerText { get; set; }  
        public AppUser User { get; set; }
        public Questions Question { get; set; }
        public Exams Exam { get; set; }



    }
}
