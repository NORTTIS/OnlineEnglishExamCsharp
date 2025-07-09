using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN222_English_Exam.Models;
using PRN222_English_Exam.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace PRN222_English_Exam.Controllers
{
    public class HomeController(OnlineExamDbContext context) : Controller
    {

        public IActionResult Index(List<Exam> listExam)
        {

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("ADMIN"))
                {
                    return RedirectToAction("Logout", "Account");
                }
            }
            listExam = context.Exam
                       .OrderByDescending(e => e.CreatedAt)
                       .ToList();
            return View(listExam);
        }

        [HttpPost]
        public IActionResult Search(string SearchValue, string SortBy, string SortOrder)
        {
            var exams = context.Exam.ToList(); ;
            if (!string.IsNullOrEmpty(SearchValue))
            {
                exams = context.Exam
                .Where(e => e.Title.Contains(SearchValue) || e.ExamId.ToString().Contains(SearchValue))
                .ToList();
            }

            if (!string.IsNullOrEmpty(SortOrder))
            {
                if (SortOrder == "ASC")
                {
                    if (SortBy == "Title")
                        exams = exams.OrderBy(e => e.Title).ToList();
                    else if (SortBy == "Duration")
                        exams = exams.OrderBy(e => e.Duration).ToList();
                    else if (SortBy == "CreatedAt")
                        exams = exams.OrderBy(e => e.CreatedAt).ToList();
                    else if (SortBy == "UpdatedAt")
                        exams = exams.OrderBy(e => e.UpdatedAt).ToList();
                    else
                        exams = exams.OrderBy(e => e.ExamId).ToList(); // Default sort by ExamId
                }
                else if (SortOrder == "DESC")
                {

                    if (SortBy == "Title")
                        exams = exams.OrderByDescending(e => e.Title).ToList();
                    else if (SortBy == "Duration")
                        exams = exams.OrderByDescending(e => e.Duration).ToList();
                    else if (SortBy == "CreatedAt")
                        exams = exams.OrderByDescending(e => e.CreatedAt).ToList();
                    else if (SortBy == "UpdatedAt")
                        exams = exams.OrderByDescending(e => e.UpdatedAt).ToList();
                    else
                        exams = exams.OrderByDescending(e => e.ExamId).ToList(); // Default sort by ExamId
                }
            }
            TempData["SortBy"] = SortBy;
            TempData["SortOrder"] = SortOrder;
            TempData["SearchValue"] = SearchValue;
            return View("Index", exams);
        }

        public IActionResult TakeExam(int Id)
        {
            if (Id > 0)
            {
                var ex = context.Exam
                    .Include(e => e.Questions)
                        .ThenInclude(q => q.Options)
                    .FirstOrDefault(e => e.ExamId == Id);

                if (ex != null)
                {
                    var examDetail = new ExamDetail
                    {
                        ExamId = ex.ExamId,
                        Title = ex.Title,
                        Duration = ex.Duration,
                        CreatedAt = ex.CreatedAt,
                        Questions = ex.Questions.Select(q => new QuestionDetail
                        {
                            QuestionId = q.QuestionId,
                            QuestionText = q.QuestionText,
                            QuestionType = q.QuestionType,
                            Options = q.Options.Select(o => o.OptionText).ToList()
                        }).ToList()
                    };
                    return View("TakeExam", examDetail);
                }
            }
            TempData["Message"] = "Exam not found!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "USER")]
        [HttpPost]
        public IActionResult SubmitExam(int id, ExamDetail model)
        {
            var exam = context.Exam
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null)
            {
                TempData["Message"] = "Exam not found!";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                TempData["Error"] = "User not found! Please log in again.";
                return RedirectToAction("Login", "Account");
            }
            var answer = new Answer
            {
                ExamId = exam.ExamId,
                UserId = userId,
                SubmittedAt = DateTime.UtcNow,
                AnswerDetails = new List<AnswerDetail>()
            };

            foreach (var qDetail in model.Questions)
            {
                var question = exam.Questions.FirstOrDefault(q => q.QuestionId == qDetail.QuestionId);
                if (question == null) continue;

                if (question.QuestionType == "checkbox")
                {
                    foreach (var opt in qDetail.SelectedOptions)
                    {
                        answer.AnswerDetails.Add(new AnswerDetail
                        {
                            QuestionId = question.QuestionId,
                            AnswerText = opt
                        });
                    }
                }
                else
                {
                    answer.AnswerDetails.Add(new AnswerDetail
                    {
                        QuestionId = question.QuestionId,
                        AnswerText = qDetail.AnswerText
                    });
                }
            }

            context.Answer.Add(answer);
            context.SaveChanges();

            TempData["Message"] = "Exam submitted successfully!";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
