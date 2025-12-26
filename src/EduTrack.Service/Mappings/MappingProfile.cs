using AutoMapper;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.Attendances;
using EduTrack.Service.DTOs.Branches;
using EduTrack.Service.DTOs.Groups;
using EduTrack.Service.DTOs.Payments;
using EduTrack.Service.DTOs.Rooms;
using EduTrack.Service.DTOs.Students;
using EduTrack.Service.DTOs.Users;


namespace EduTrack.Service.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Student Mappings
        CreateMap<Student, StudentCreationDto>().ReverseMap();
        CreateMap<Student, StudentResultDto>().ReverseMap();
        CreateMap<Student, StudentUpdateDto>().ReverseMap();

        // Payment Mappings
        CreateMap<Payment, PaymentUpdateDto>().ReverseMap();
        CreateMap<Payment, PaymentResultDto>().ReverseMap();
        CreateMap<Payment, PaymentCreationDto>().ReverseMap();

        // Attendance
        CreateMap<Attendance, AttendanceCreationDto>().ReverseMap();
        CreateMap<Attendance, AttendanceResultDto>().ReverseMap();
        CreateMap<Attendance, AttendanceUpdateDto>().ReverseMap();

        // Branch
        CreateMap<Branch, BranchCreationDto>().ReverseMap();
        CreateMap<Branch, BranchResultDto>().ReverseMap();
        CreateMap<Branch, BranchUpdateDto>().ReverseMap();

        // Group
        CreateMap<Group, GroupCreationDto>().ReverseMap();
        CreateMap<Group, GroupResultDto>().ReverseMap();
        CreateMap<Group, GroupUpdateDto>().ReverseMap();

        // Room
        CreateMap<Room, RoomCreationDto>().ReverseMap();
        CreateMap<Room, RoomResultDto>().ReverseMap();
        CreateMap<Room, RoomUpdateDto>().ReverseMap();

        // User
        CreateMap<User, UserCreationDto>().ReverseMap();
        CreateMap<User, UserResultDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
    }
}
