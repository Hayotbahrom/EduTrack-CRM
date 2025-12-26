using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.StudentGroups;
using EduTrack.Service.DTOs.Students;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Service.Services;

public class StudentGroupService : IStudentGroupService
{
    private readonly IRepository<StudentGroup> _studentGroupRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Group> _groupRepository;
    private readonly IMapper _mapper;

    public StudentGroupService(
        IRepository<StudentGroup> studentGroupRepository,
        IRepository<Student> studentRepository,
        IRepository<Group> groupRepository,
        IMapper mapper)
    {
        _studentGroupRepository = studentGroupRepository;
        _studentRepository = studentRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    // =====================================================
    // EnrollStudentAsync - Student-ni Group-ga qo'shish
    // =====================================================
    public async Task<StudentGroupResultDto> EnrollStudentAsync(StudentGroupCreationDto dto)
    {
        // Student mavjud ekanligini tekshir
        var student = await _studentRepository.SelectByIdAsync(dto.StudentId)
            ?? throw new CustomException(404, "O'quvchi topilmadi");

        // Group mavjud ekanligini tekshir
        var group = await _groupRepository.SelectByIdAsync(dto.GroupId)
            ?? throw new CustomException(404, "Guruh topilmadi");

        // Allaqachon ro'yxatda borligini tekshir
        var existingEnrollment = await _studentGroupRepository.SelectAsync(
            sg => sg.StudentId == dto.StudentId && sg.GroupId == dto.GroupId);

        if (existingEnrollment != null)
            throw new CustomException(400, "Bu o'quvchi allaqachon bu guruha ro'yxatdan o'tgan");

        // Yangi StudentGroup yaratish
        var studentGroup = new StudentGroup
        {
            StudentId = dto.StudentId,
            GroupId = dto.GroupId,
            CreatedAt = DateTime.UtcNow
        };

        var createdStudentGroup = await _studentGroupRepository.InsertAsync(studentGroup);

        return await MapStudentGroupToResultDto(createdStudentGroup);
    }

    // =====================================================
    // RemoveStudentAsync - Student-ni Group-dan chiqarish
    // =====================================================
    public async Task<bool> RemoveStudentAsync(int studentId, int groupId)
    {
        var studentGroup = await _studentGroupRepository.SelectAsync(
            sg => sg.StudentId == studentId && sg.GroupId == groupId)
            ?? throw new CustomException(404, "Bu o'quvchi bu guruhda ro'yxatda yo'q");

        await _studentGroupRepository.DeleteAsync(studentGroup.Id);
        return true;
    }

    // =====================================================
    // GetStudentsByGroupAsync - Group-dagi barcha o'quvchilar
    // =====================================================
    public async Task<IEnumerable<StudentGroupResultDto>> GetStudentsByGroupAsync(int groupId)
    {
        var group = await _groupRepository.SelectByIdAsync(groupId)
            ?? throw new CustomException(404, "Guruh topilmadi");

        var studentGroups = _studentGroupRepository.SelectAll()
            .Where(sg => sg.GroupId == groupId)
            .ToList();

        var result = new List<StudentGroupResultDto>();

        foreach (var sg in studentGroups)
        {
            result.Add(await MapStudentGroupToResultDto(sg));
        }

        return result;
    }

    // =====================================================
    // GetGroupsByStudentAsync - O'quvchining barcha guruhlari
    // =====================================================
    public async Task<IEnumerable<StudentGroupResultDto>> GetGroupsByStudentAsync(int studentId)
    {
        var student = await _studentRepository.SelectByIdAsync(studentId)
            ?? throw new CustomException(404, "O'quvchi topilmadi");

        var studentGroups = await _studentGroupRepository.SelectAll()
            .Where(sg => sg.StudentId == studentId)
            .Include(sg => sg.Group)
            .Include(sg => sg.Student)
            .ToListAsync();

        var result = new List<StudentGroupResultDto>();

        foreach (var sg in studentGroups)
        {
            result.Add(await MapStudentGroupToResultDto(sg));
        }

        return result;
    }

    // =====================================================
    // GetStudentCountByGroupAsync - Guruhda qancha o'quvchi
    // =====================================================
    public async Task<int> GetStudentCountByGroupAsync(int groupId)
    {
        var count = _studentGroupRepository.SelectAll()
            .Where(sg => sg.GroupId == groupId)
            .Count();

        return count;
    }

    // =====================================================
    // Helper Method - Mapping bilan navigation properties
    // =====================================================
    private async Task<StudentGroupResultDto> MapStudentGroupToResultDto(StudentGroup sg)
    {
        var dto = _mapper.Map<StudentGroupResultDto>(sg);

        // Student ma'lumotlarini qo'shish
        if (sg.StudentId > 0)
        {
            var student = await _studentRepository.SelectByIdAsync(sg.StudentId);
            if (student != null)
            {
                dto.Student = student;
            }
        }

        // Group ma'lumotlarini qo'shish
        if (sg.GroupId > 0)
        {
            var group = await _groupRepository.SelectByIdAsync(sg.GroupId);
            if (group != null)
            {
                dto.Group = group;
            }
        }

        return dto;
    }
}
