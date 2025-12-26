namespace EduTrack.Domain.Enums;

public enum UserRole
{
    Admin = 0,           // Full system access
    Director = 1,        // Branch management, reports
    Manager = 2,         // Group & student management
    Teacher = 3,         // Teaching, attendance marking
    AssistantTeacher = 4, // Helping teachers
    Accountant = 5       // Payment management
}
