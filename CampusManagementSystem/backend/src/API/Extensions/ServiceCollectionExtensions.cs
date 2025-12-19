using CampusManagementSystem.Application.Interfaces;
using CampusManagementSystem.Application.Services;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Repositories;

namespace CampusManagementSystem.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Auth Service
        services.AddScoped<IAuthService, AuthService>();

        // User Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IStaffService, StaffService>();

        // Academic Services
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();

        // Resource Services
        services.AddScoped<IClassroomService, ClassroomService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IFacilityService, FacilityService>();
        services.AddScoped<IIssueReportService, IssueReportService>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Generic and specific repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IClassroomRepository, ClassroomRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IFacilityRepository, FacilityRepository>();
        services.AddScoped<IIssueReportRepository, IssueReportRepository>();

        return services;
    }
}
