using EduTrack.Data.IRepositories;
using EduTrack.Data.Repositories;
using EduTrack.Service.Interfaces;
using EduTrack.Service.Mappings;
using EduTrack.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EduTrack.MVC.Extentions;

public static class ServiceExtention
{
    public static void AddServiceExtention(this IServiceCollection services)
    {
        // Automapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


        // Services
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEnumHelperService, EnumHelperService>();
        services.AddScoped<IStudentGroupService, StudentGroupService>();
    }   
}
