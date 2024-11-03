using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository;

public partial class MultipleChoiceTestDbContext : DbContext
{
	private readonly IConfiguration _configuration;
	public MultipleChoiceTestDbContext()
	{
	}

	public MultipleChoiceTestDbContext(DbContextOptions<MultipleChoiceTestDbContext> options, IConfiguration configuration)
		: base(options)
	{
		_configuration = configuration;
	}

	public virtual DbSet<AccessStatistic> AccessStatistics { get; set; }

	public virtual DbSet<Exam> Exams { get; set; }

	public virtual DbSet<ExamAttempt> ExamAttempts { get; set; }

	public virtual DbSet<ExamResult> ExamResults { get; set; }

	public virtual DbSet<Feedback> Feedbacks { get; set; }

	public virtual DbSet<Lesson> Lessons { get; set; }

	public virtual DbSet<Question> Questions { get; set; }

	public virtual DbSet<Session> Sessions { get; set; }

	public virtual DbSet<Subject> Subjects { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = _configuration.GetConnectionString("DefaultConnection");
		optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MultipleChoiceTestDB;Integrated Security=True;Trust Server Certificate=True");

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AccessStatistic>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__AccessSt__3214EC07659DDD87");

			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<Exam>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Exam__3214EC07382545C6");

			entity.ToTable("Exam");

			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.ExamName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);

			entity.HasOne(d => d.Lesson).WithMany(p => p.Exams)
				.HasForeignKey(d => d.LessonId)
				.HasConstraintName("FK__Exam__LessonId__571DF1D5");

			entity.HasOne(d => d.Subject).WithMany(p => p.Exams)
				.HasForeignKey(d => d.SubjectId)
				.HasConstraintName("FK__Exam__SubjectId__5629CD9C");
		});

		modelBuilder.Entity<ExamAttempt>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__ExamAtte__3214EC076B8846A7");

			entity.ToTable("ExamAttempt");

			entity.Property(e => e.Answer).HasColumnType("text");
			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);

			entity.HasOne(d => d.Exam).WithMany(p => p.ExamAttempts)
				.HasForeignKey(d => d.ExamId)
				.HasConstraintName("FK__ExamAttem__ExamI__5EBF139D");

			entity.HasOne(d => d.Question).WithMany(p => p.ExamAttempts)
				.HasForeignKey(d => d.QuestionId)
				.HasConstraintName("FK__ExamAttem__Quest__5FB337D6");

			entity.HasOne(d => d.User).WithMany(p => p.ExamAttempts)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__ExamAttem__UserI__5DCAEF64");
		});

		modelBuilder.Entity<ExamResult>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__ExamResu__3214EC07D7C29BE2");

			entity.ToTable("ExamResult");

			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.Rank)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);

			entity.HasOne(d => d.Exam).WithMany(p => p.ExamResults)
				.HasForeignKey(d => d.ExamId)
				.HasConstraintName("FK__ExamResul__ExamI__6383C8BA");

			entity.HasOne(d => d.User).WithMany(p => p.ExamResults)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__ExamResul__UserI__6477ECF3");
		});

		modelBuilder.Entity<Feedback>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07572B0523");

			entity.ToTable("Feedback");

			entity.Property(e => e.Address)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Content).HasColumnType("text");
			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.Email)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.FullName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.Title)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<Lesson>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Lesson__3214EC07C56FC12F");

			entity.ToTable("Lesson");

			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.LessonName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);

			entity.HasOne(d => d.Subject).WithMany(p => p.Lessons)
				.HasForeignKey(d => d.SubjectId)
				.HasConstraintName("FK__Lesson__SubjectI__4D94879B");
		});

		modelBuilder.Entity<Question>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07657272FB");

			entity.ToTable("Question");

			entity.Property(e => e.Choices).HasColumnType("text");
			entity.Property(e => e.CorrectAnswer).HasColumnType("text");
			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.QuestionText).HasColumnType("text");
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);

			entity.HasOne(d => d.Lesson).WithMany(p => p.Questions)
				.HasForeignKey(d => d.LessonId)
				.HasConstraintName("FK__Question__Lesson__52593CB8");

			entity.HasOne(d => d.Subject).WithMany(p => p.Questions)
				.HasForeignKey(d => d.SubjectId)
				.HasConstraintName("FK__Question__Subjec__5165187F");
		});

		modelBuilder.Entity<Session>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Session__3214EC07F9FD0031");

			entity.ToTable("Session");

			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.SessionEnd).HasColumnType("datetime");
			entity.Property(e => e.SessionStart).HasColumnType("datetime");
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);

			entity.HasOne(d => d.User).WithMany(p => p.Sessions)
				.HasForeignKey(d => d.UserId)
				.HasConstraintName("FK__Session__UserId__68487DD7");
		});

		modelBuilder.Entity<Subject>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC078C762F0A");

			entity.ToTable("Subject");

			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.SubjectName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__User__3214EC0787C57722");

			entity.ToTable("User");

			entity.Property(e => e.AccountName)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.CreatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.Email)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Gender)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.IsDeleted).HasDefaultValue(false);
			entity.Property(e => e.PasswordHash)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.UpdatedBy)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.UserName)
				.HasMaxLength(255)
				.IsUnicode(false);
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
