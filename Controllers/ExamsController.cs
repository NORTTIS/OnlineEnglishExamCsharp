using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PRN222_English_Exam.Models;
using PRN222_English_Exam.ViewModels;

namespace PRN222_English_Exam.Controllers
{
    [Route("Admin/[controller]/[action]")]
    public class ExamsController(OnlineExamDbContext context) : Controller
    {
        public IActionResult ListExam(List<Exam> listExam)
        {
            if (listExam != null && listExam.Count > 0)
            {
                return View(listExam);
            }

            List<Exam> Ex = context.Exam.OrderByDescending(e => e.CreatedAt).ToList();

            return View(Ex);
        }

        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateExam(ExamCreateViewModel examCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View("Create");
                
            }
            else
            {
                Exam exam = new Exam
                {
                    Title = examCreateViewModel.Title,
                    Duration = examCreateViewModel.Duration,
                    Questions = new List<Question>()
                };
                foreach (var q in examCreateViewModel.Questions)
                {
                    Question question = new Question
                    {
                        QuestionText = q.QuestionText,
                        QuestionType = q.QuestionType,
                        Options = new List<Option>()
                    };

                    if (q.QuestionType == "text")
                    {
                        question.Options.Add(new Option
                        {
                            OptionText = q.TextAnswer,
                            IsCorrect = true
                        });
                    }
                    else if (q.QuestionType == "radio")
                    {
                        if (q.Options.Count == 0)
                        {
                            TempData["Error"] = "At least one option is required for single choice questions.";
                            return View("Create");
                        }
                        for (int i = 0; i < q.Options.Count; i++)
                        {
                            question.Options.Add(new Option
                            {
                                OptionText = q.Options[i].OptionText,
                                IsCorrect = i == q.SingleChoiceAnswer
                            });
                        }
                    }
                    else if (q.QuestionType == "checkbox")
                    {
                        for (int i = 0; i < q.Options.Count; i++)
                        {
                            question.Options.Add(new Option
                            {
                                OptionText = q.Options[i].OptionText,
                                IsCorrect = q.MultipleChoiceAnswer?.Contains(i) == true
                            });
                        }
                    }

                    exam.Questions.Add(question);
                }
                context.Exam.Add(exam);
                context.SaveChanges();
                TempData["Message"] = "Exam created successfully!";
                return RedirectToAction("ListExam");
            }

        }

        //Detail
        [HttpGet("{id}")]
        public IActionResult Detail(int id)
        {
            var exam = context.Exam
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null)
            {
                TempData["Error"] = "Exam not found!";
                return View("ListExam");
            }
            var examDetail = new ExamCreateViewModel
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Duration = exam.Duration,
                CreatedAt = exam.CreatedAt,

                Questions = exam.Questions.Select(q => new QuestionViewModel
                {
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = q.Options.Select(o => new OptionViewModel
                    {
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return View(examDetail);
        }

        //Edit
        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var exam = context.Exam
        .Include(e => e.Questions)
            .ThenInclude(q => q.Options)
        .FirstOrDefault(e => e.ExamId == id);

            if (exam == null) return NotFound();

            var examDetail = new ExamCreateViewModel
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Duration = exam.Duration,
                CreatedAt = exam.CreatedAt,

                Questions = exam.Questions.Select(q => new QuestionViewModel
                {
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = q.Options.Select(o => new OptionViewModel
                    {
                        OptionText = o.OptionText,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return View(examDetail);
        }

        [HttpPost]
        public IActionResult EditExam(ExamDetail examDetail)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View("Edit", examDetail);
            }

            var exam = context.Exam
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefault(e => e.ExamId == examDetail.ExamId);

            if (exam == null) return NotFound();

            exam.Title = examDetail.Title;
            exam.Duration = examDetail.Duration;
            exam.UpdatedAt = DateTime.Now;

            // Xóa câu hỏi cũ và thêm lại
            context.Question.RemoveRange(exam.Questions);

            exam.Questions = examDetail.Questions.Select(q => new Question
            {
                QuestionText = q.QuestionText,
                QuestionType = q.QuestionType,
                Options = q.Options.Select(opt => new Option
                {
                    OptionText = opt
                }).ToList()
            }).ToList();

            context.SaveChanges();
            TempData["Message"] = "Exam updated successfully!";
            return RedirectToAction("ListExam");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var exam = context.Exam.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            context.Exam.Remove(exam);
            context.SaveChanges();
            TempData["Message"] = "Exam deleted successfully!";
            return RedirectToAction("ListExam");
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
            return View("ListExam", exams);
        }
    }
}
