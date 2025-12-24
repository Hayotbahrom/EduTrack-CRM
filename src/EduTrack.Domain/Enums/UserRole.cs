namespace EduTrack.Domain.Enums;

public enum UserRole
{
    Admin = 1,           // Full system access
    Director = 2,        // Branch management, reports
    Manager = 3,         // Group & student management
    Teacher = 4,         // Teaching, attendance marking
    AssistantTeacher = 5, // Helping teachers
    Accountant = 6       // Payment management
}
