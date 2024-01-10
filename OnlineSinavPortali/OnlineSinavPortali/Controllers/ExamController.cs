using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using OnlineSinavPortali.Migrations;
using OnlineSinavPortali.Models;
using OnlineSinavPortali.ViewModels;
using System.Diagnostics.Eventing.Reader;
using System.Security.Policy;

namespace OnlineSinavPortali.Controllers
{
    public class ExamController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notify;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ExamController(AppDbContext context, INotyfService notify, UserManager<AppUser> userManager, ILogger<HomeController> logger)
        {
            _context = context;
            _notify = notify;
            _userManager = userManager;
            _logger = logger;
        }


        
        public IActionResult ExamListAjax()
        {
            var UserID = _userManager.GetUserId(User);
            var examModel = _context.Exams.Where(s => s.TeacherID == UserID)
                    .Select(x => new
                    ExamModel
                    {
                        UniversityDepartment = x.UniversityDepartment,
                        ExamID = x.ExamID,
                        CourseName = x.CourseName,
                        ExamType = x.ExamType,
                        ExamDate = x.ExamDate,
                    }).ToList();

            return Json(examModel);
        }



        public IActionResult DeleteQuestion(int id)
        {
            var questionToDelete = _context.Questions.Find(id);

            var examId = questionToDelete.ExamID;

            _context.Questions.Remove(questionToDelete);
            _context.SaveChanges();
            _notify.Warning("Soru silindi");
            // Başarılı bir şekilde silindiğini belirten bir mesaj veya başka bir işlem yapabilirsiniz.
            return RedirectToAction("Questions", "Exam", new { id = examId });

        }







        public IActionResult CreateExam()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateExam(string courseId, string examType, string examDateTime, string UniversityDepartment)
        {

            try
            {
                var userId = _userManager.GetUserId(User);
                DateTime parsedDateTime = DateTime.Parse(examDateTime);

                Exams newExam = new()
                {
                    UniversityDepartment = UniversityDepartment,
                    TeacherID = userId,
                    CourseName = courseId,
                    ExamDate = parsedDateTime,
                    ExamType = examType
                };
                _context.Exams.Add(newExam);

                int affectedRows = _context.SaveChanges();

                return Ok(new { message = "Exam successfully created." });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return BadRequest(new { message = "Error creating exam.", error = ex.Message });
            }









        }
        public IActionResult MyExams()
        {
            return View();
        }



        [HttpPost]
        public JsonResult GetQuestionsByExamID([FromBody] int examID)
        {
            try
            {
                var questions = _context.Questions
                    .Where(q => q.ExamID == examID)
                    .Select(q => new QuestionModel
                    {
                        QuestionID = q.QuestionID,
                        QuestionText = q.QuestionText,
                        QuestionAnswersText = q.QuestionAnswer,
                    })
                    .ToList();

                return Json(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Sorular getirilirken hata oluştu: " + ex.Message);
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Questions(int id)
        {
            TempData["examID"] = id;
            ViewBag.ExamID = id;
            var Exam = _context.Exams
            .FirstOrDefault(q => q.ExamID == id);
            var examType = Exam.ExamType;

            _logger.LogInformation("exam ID =====" + id);
            var questionsModel = await _context.Questions
            .Where(a => a.ExamID == id)
            .Select(x => new QuestionModel()
            {
                ExamType = examType,
                optionAText = x.optionAText,
                optionBText = x.optionBText,
                optionDText = x.optionDText,
                optionCText = x.optionCText,
                ExamID = x.ExamID,
                QuestionID = x.QuestionID,
                QuestionText = x.QuestionText,
                QuestionAnswersText = x.QuestionAnswer
            })
            .ToListAsync();
            _logger.LogInformation("exam Count =====" + questionsModel.Count);

            return View(questionsModel);


        }



        public IActionResult EditQuestion(int id)
        {

            TempData["QuestionID"] = id;
            var questionModel = _context.Questions
             .Where(a => a.QuestionID == id)
             .Select(x => new QuestionModel()
             {
                 optionAText = x.optionAText,
                 optionBText = x.optionBText,
                 optionDText = x.optionDText,
                 optionCText = x.optionCText,
                 ExamID = x.ExamID,
                 QuestionID = x.QuestionID,
                 QuestionText = x.QuestionText,
                 QuestionAnswersText = x.QuestionAnswer
             })
            .FirstOrDefault();

            return View(questionModel);
        }
        [HttpPost]
        public IActionResult EditQuestion(QuestionPostModel model)
        {
            ////int examID = (int)TempData["examID"];


            int QuestionID = (int)TempData["QuestionID"];

            var existingQuestion = _context.Questions
            .FirstOrDefault(q => q.QuestionID == QuestionID);
            existingQuestion.QuestionText = model.QuestionText;
            existingQuestion.QuestionAnswer = model.QuestionAnswersText;
            _context.SaveChanges();
            _notify.Success("Soru Güncellendi");
            return RedirectToAction("Questions", "Exam", new { id = existingQuestion.ExamID });

        }

        public IActionResult CreateQuestion(int id)
        {

            var Exam = _context.Exams
            .FirstOrDefault(q => q.ExamID == id);
            var examType = Exam.ExamType;
            var questionModel = new QuestionPostModel()
            {
                ExamType = examType
            };

            return View(questionModel);


        }
        [HttpPost]
        public IActionResult CreateQuestion([FromForm] QuestionPostModel model)
        {

            int examID = (int)TempData["examID"];
            TempData["examID"] = examID;
            var Exam = _context.Exams
            .FirstOrDefault(q => q.ExamID == examID);
            var examType = Exam.ExamType;
            _logger.LogInformation($"soru id = {examID}");
            if (ModelState.IsValid)
            {

                var newQuestionModel = new Questions()
                {
                    ExamType = model.ExamType,
                    optionAText = model.optionAText,
                    optionBText = model.optionBText,
                    optionCText = model.optionCText,
                    optionDText = model.optionDText,
                    ExamID = examID,
                    QuestionText = model.QuestionText,
                    QuestionAnswer = model.QuestionAnswersText,
                };
                _context.Questions.Add(newQuestionModel);
                _context.SaveChanges();

                _notify.Success("Soru Eklendi", 4);
                return RedirectToAction("Questions", "Exam", new { id = examID });




            }
            _notify.Error("Formda Hata Var");
            // ModelState.IsValid false ise, gerekli hata işlemlerini burada yapabilirsiniz.
            return View(model); // Örneğin, hata durumunda aynı sayfaya geri dönebilirsiniz.
        }

        public IActionResult EditExam(int id)
        {
            var examModel = _context.Exams
             .Where(a => a.ExamID == id)
             .Select(x => new ExamModel()
             {
                 CourseName = x.CourseName,
                 UniversityDepartment = x.UniversityDepartment,
                 ExamType = x.ExamType,
                 ExamDate = x.ExamDate,
                 ExamID = x.ExamID,
             })
            .FirstOrDefault();

            return View(examModel);


        }
        [HttpPost]
        public IActionResult EditExam(ExamModel model)
        {


            DateTime examDateTime = model.ExamDate1.Date.Add(model.ExamTime.TimeOfDay);
            var existingQuestion = _context.Exams
            .FirstOrDefault(q => q.ExamID == model.ExamID);

            existingQuestion.CourseName = model.CourseName;
            existingQuestion.UniversityDepartment = model.UniversityDepartment;
            existingQuestion.ExamDate = examDateTime;

            _context.SaveChanges();
            _notify.Success("Soru Güncellendi");

            return RedirectToAction("MyExams");

        }



        public IActionResult ExamDelete(int id)
        {

            var exam = _context.Exams
            .FirstOrDefault(q => q.ExamID == id);

            TempData["examID"] = id;
            var examModel = new ExamModel()
            {
                CourseName = exam.CourseName,
                UniversityDepartment = exam.UniversityDepartment,
                ExamDate = exam.ExamDate,
                ExamID = id,
            };
            return View(examModel);
        }

        [HttpPost]
        public IActionResult ExamDelete()
        {
            int id = (int)TempData["examID"];
            TempData["examID"] = id;
            var exam = _context.Exams
            .FirstOrDefault(q => q.ExamID == id);
            _context.Exams.Remove(exam);
            _context.SaveChanges();
            _notify.Success("Sınav Silindi");
            return RedirectToAction("MyExams");
        }


        // STUDENT KISMI

        public IActionResult StudentMyExams()
        {
            var userId = _userManager.GetUserId(User);
            var user = _context.Users
            .FirstOrDefault(q => q.Id == userId);


            var examModel = _context.Exams
             .Where(a => a.UniversityDepartment == user.UniversityDepartment)
             .Select(x => new ExamModel()
             {
                 CourseName = x.CourseName,
                 UniversityDepartment = x.UniversityDepartment,
                 ExamDate = x.ExamDate,
                 ExamID = x.ExamID,
             })
            .ToList();

            return View(examModel);


        }
        public IActionResult EnterExam(int id)
        {
            var exam = _context.Exams
            .FirstOrDefault(q => q.ExamID == id);
            var now = DateTime.Now;
            var examDate = exam.ExamDate;
            var sure = now - examDate;
            if (sure.Days == 0 && sure.Hours == 0 && (sure.Minutes > 10 || sure.Minutes < 10))
            {
                  var studentAnswerModel = _context.Questions
                 .Where(a => a.ExamID == id)
                 .Select(x => new StudentAnswersModel()
                 {
                     
                     optionAText = x.optionAText,
                     optionBText = x.optionBText,
                     optionCText = x.optionCText,
                     optionDText = x.optionDText,
                     ExamID = id,
                     QuestionText = x.QuestionText,
                     QuestionAnswersText = x.QuestionAnswer,
                 })
                .ToList();
                //_logger.LogInformation($"Geçen süre: {sure.Days} gün, {sure.Hours} saat, {sure.Minutes} dakika");
                return View(studentAnswerModel);
            }
            else
            {
                var zamanModel = new ZamanModel
                {
                    Sure = sure,
                };
                return RedirectToAction("ExamTimeOut",zamanModel);
            }
                
            
            
        }
        [HttpPost]
        public IActionResult EnterExam(StudentAnswersModel model)
        {




            _notify.Success("Sınav Kaydınız Yapıldı.");
            return RedirectToAction("Index", "Home");
        }






        public IActionResult ExamTimeOut(ZamanModel model)
        {
            return View(model);  
        }
    }
}
            



    
