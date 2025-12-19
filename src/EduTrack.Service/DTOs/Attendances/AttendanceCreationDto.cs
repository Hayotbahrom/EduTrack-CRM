using EduTrack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EduTrack.Service.DTOs.Attendances;

public class AttendanceCreationDto
{
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public string Remarks { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int GroupId { get; set; }
    public Domain.Entities.Group Group { get; set; }
}
