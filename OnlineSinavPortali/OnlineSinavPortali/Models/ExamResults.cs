using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSinavPortali.Models
{
    public class ExamResults
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResultID { get; set; }
        public int ExamID { get; set; }
        public string StudentID { get; set; }
        public int Score { get; set; }  
        public Exams Exam { get; set; }   
        public AppUser User { get; set; }



    }
}

