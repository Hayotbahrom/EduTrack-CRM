using AutoMapper;
using EduTrack.Data.IRepositories;
using EduTrack.Domain.Entities;
using EduTrack.Service.DTOs.Students;
using EduTrack.Service.Exceptions;
using EduTrack.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduTrack.Service.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _repository;
    private readonly IMapper _mapper;

    public StudentService(IRepository<Student> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StudentResultDto> AddAsync(StudentCreationDto dto)
    {
        var student = await _repository.SelectAsync(s => s.FirstName == dto.FirstName && s.PhoneNumber == dto.PhoneNumber);
        if (student != null)
            throw new CustomException(404, "Student already exists.");

        var mappedStudent = _mapper.Map<Student>(dto);
        var createdStudent = await _repository.InsertAsync(mappedStudent);
        return _mapper.Map<StudentResultDto>(createdStudent);
    }

    public async Task<IEnumerable<StudentResultDto>> GetAllAsync()
    {
        var students = await _repository.SelectAll().Where(s => s.IsDeleted == false).ToListAsync();

        var mappedStudents = _mapper.Map<IEnumerable<StudentResultDto>>(students);

        return mappedStudents;
    }

    public async Task<StudentResultDto> GetByIdAsync(int id)
    {
        var student = await GetStudentAsync(id);
       
        return _mapper.Map<StudentResultDto>(student); 
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var student = await GetStudentAsync(id);

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<StudentResultDto> UpdateAsync(int id, StudentUpdateDto dto)
    {
        var student = await GetStudentAsync(id);

        var mappedStudent = _mapper.Map(dto, student);
        mappedStudent.UpdatedAt = DateTime.UtcNow;

        var result = await _repository.UpdateAsync(mappedStudent);

        return _mapper.Map<StudentResultDto>(result);
    }

    private async Task<Student> GetStudentAsync(int id)
    {
        var student = await _repository.SelectByIdAsync(id)
            ?? throw new CustomException(404, "Student not found");
        return student;
    }
}
