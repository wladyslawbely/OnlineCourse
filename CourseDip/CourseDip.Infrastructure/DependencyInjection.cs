using CourseDip.Application.Interfaces.Repository;
using CourseDip.Application.Interfaces.Service;
using CourseDip.Application.Services;
using CourseDip.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;


namespace CourseDip.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IUserService,UserService>();

            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonService, LessonService>();

            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IModuleService, ModuleService>();

            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();

            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAssignmentService, AssignmentService>();

            services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            services.AddScoped<ISubmissionService, SubmissionService>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
            services.AddScoped<ILessonProgressService, LessonProgressService>();

            services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
            services.AddScoped<ICourseCategoryService, CourseCategoryService>();

            services.AddScoped<ICertificateService, CertificateService>();

            return services;
        }
    }
}
