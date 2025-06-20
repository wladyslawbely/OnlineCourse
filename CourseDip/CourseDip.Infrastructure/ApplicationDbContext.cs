using Microsoft.EntityFrameworkCore;
using CourseDip.Domain.Entities;
using File = CourseDip.Domain.Entities.File;

namespace CourseDip.Infrastructure.Persistence;

public partial class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {    }


    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Coursecategory> Coursecategories { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Lessonprogress> Lessonprogresses { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("assignments_pkey");

            entity.ToTable("assignments");

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DueDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("due_date");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("assignments_lesson_id_fkey");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.CourseId).HasColumnName("course_id");

            entity.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");

            entity.Property(e => e.IsAdmin)
                .HasColumnName("is_admin")
                .HasDefaultValue(false);

            entity.HasOne(d => d.Lesson).WithMany(p => p.Comments)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("comments_lesson_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("comments_user_id_fkey");

            entity.HasOne(d => d.Course).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("comments_course_id_fkey");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.Replies)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("comments_parent_comment_id_fkey")
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("courses_pkey");

            entity.ToTable("courses");

           

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasMany(d => d.Comments).WithOne(p => p.Course)
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_course_id_fkey");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("courses_instructor_id_fkey");

            entity.HasMany(d => d.Categories).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "Coursecategorymapping",
                    r => r.HasOne<Coursecategory>().WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("coursecategorymapping_category_id_fkey"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .HasConstraintName("coursecategorymapping_course_id_fkey"),
                    j =>
                    {
                        j.HasKey("CourseId", "CategoryId").HasName("coursecategorymapping_pkey");
                        j.ToTable("coursecategorymapping");
                        j.IndexerProperty<int>("CourseId").HasColumnName("course_id");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });
        });

        modelBuilder.Entity<Coursecategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("coursecategories_pkey");

            entity.ToTable("coursecategories");

            entity.HasIndex(e => e.Name, "coursecategories_name_key").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("enrollments_pkey");

            entity.ToTable("enrollments");

            entity.HasIndex(e => new { e.UserId, e.CourseId }, "enrollments_user_id_course_id_key").IsUnique();

            entity.Property(e => e.EnrollmentId).HasColumnName("enrollment_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.EnrolledAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enrolled_at");
            entity.Property(e => e.Progress)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("progress");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("enrollments_course_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("enrollments_user_id_fkey");

            entity.Property(e => e.TestResultsJson)
                .HasColumnType("text")
                .HasColumnName("testresultsjson");
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("files_pkey");

            entity.ToTable("files");

            entity.Property(e => e.FileId).HasColumnName("file_id");
            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(500)
                .HasColumnName("file_url");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("uploaded_at");
            entity.Property(e => e.UploaderId).HasColumnName("uploader_id");

            entity.HasOne(d => d.Assignment).WithMany(p => p.Files)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("files_assignment_id_fkey");

            entity.HasOne(d => d.Submission)
                 .WithMany(p => p.Files)
                 .HasForeignKey(d => d.SubmissionId)
                 .OnDelete(DeleteBehavior.Cascade) 
                 .HasConstraintName("files_submission_id_fkey");


            entity.HasOne(d => d.Lesson).WithMany(p => p.Files)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("files_lesson_id_fkey");

            entity.HasOne(d => d.Uploader).WithMany(p => p.Files)
                .HasForeignKey(d => d.UploaderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("files_uploader_id_fkey");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("lessons_pkey");

            entity.ToTable("lessons");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(500)
                .HasColumnName("video_url");

            entity.HasOne(d => d.Module).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("lessons_module_id_fkey");
        });

        modelBuilder.Entity<Lessonprogress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("lessonprogress_pkey");

            entity.ToTable("lessonprogress");

            entity.HasIndex(e => new { e.UserId, e.LessonId }, "lessonprogress_user_id_lesson_id_key").IsUnique();

            entity.Property(e => e.ProgressId).HasColumnName("progress_id");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Lessonprogresses)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("lessonprogress_lesson_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Lessonprogresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("lessonprogress_user_id_fkey");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("modules_pkey");

            entity.ToTable("modules");

            entity.Property(e => e.ModuleId).HasColumnName("module_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.OrderIndex).HasColumnName("order_index");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Course).WithMany(p => p.Modules)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("modules_course_id_fkey");

            entity.Property(e => e.TestJson)
                .HasColumnType("text")
                .HasColumnName("testjson");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_user_id_fkey");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.SubmissionId).HasName("submissions_pkey");

            entity.ToTable("submissions");

            entity.HasIndex(e => new { e.AssignmentId, e.UserId }, "submissions_assignment_id_user_id_key").IsUnique();

            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");
            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Feedback).HasColumnName("feedback");
            entity.Property(e => e.Grade)
                .HasPrecision(5, 2)
                .HasColumnName("grade");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("submitted_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Assignment).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("submissions_assignment_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("submissions_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastLogin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RegisteredAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("registered_at");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
