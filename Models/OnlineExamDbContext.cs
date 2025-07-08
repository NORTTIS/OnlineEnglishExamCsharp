using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PRN222_English_Exam.Models
{
	public class OnlineExamDbContext : IdentityDbContext<Users>
	{
		public OnlineExamDbContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<Exam> Exam { get; set; }
		public virtual DbSet<Question> Question { get; set; }
		public virtual DbSet<Option> Option { get; set; }
		public virtual DbSet<Answer> Answer { get; set; }
		public virtual DbSet<AnswerDetail> AnswerDetail { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Exam>(entity =>
			{
				entity.HasKey(e => e.ExamId);
				entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
				entity.Property(e => e.Duration);
			});

			modelBuilder.Entity<Question>(entity =>
			{
				entity.HasKey(q => q.QuestionId);
				entity.Property(q => q.QuestionText).IsRequired();
				entity.Property(q => q.QuestionType).IsRequired();

				entity.HasOne(q => q.Exam)
					  .WithMany(e => e.Questions)
					  .HasForeignKey(q => q.ExamId)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Option>(entity =>
			{
				entity.HasKey(o => o.OptionId);
				entity.Property(o => o.OptionText).IsRequired();

				entity.HasOne(o => o.Question)
					  .WithMany(q => q.Options)
					  .HasForeignKey(o => o.QuestionId)
					  .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Answer>(entity =>
			{
				entity.HasKey(a => a.AnswerId);

				entity.HasOne(a => a.Exam)
					  .WithMany(e => e.Answers)
					  .HasForeignKey(a => a.ExamId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(a => a.User)
					  .WithMany()
					  .HasForeignKey(a => a.UserId)
					  .OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<AnswerDetail>(entity =>
			{
				entity.HasKey(ad => ad.DetailId);

				entity.Property(ad => ad.AnswerText).IsRequired();

				entity.HasOne(ad => ad.Answer)
					  .WithMany(a => a.AnswerDetails)
					  .HasForeignKey(ad => ad.AnswerId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(ad => ad.Question)
					  .WithMany(q => q.AnswerDetails)
					  .HasForeignKey(ad => ad.QuestionId)
					  .OnDelete(DeleteBehavior.Restrict);
			});
		}



	}

}
