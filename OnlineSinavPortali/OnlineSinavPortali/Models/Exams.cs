using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineSinavPortali.Models
{
    public class Exams 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamID { get; set; }

        public string UniversityDepartment {  get; set; }   

        public string TeacherID { get; set; }  
        public string CourseName { get; set; }
        public string? ExamType {  get; set; }
        public DateTime ExamDate { get; set; }  
        public AppUser User { get; set; }  
        

    }
}
