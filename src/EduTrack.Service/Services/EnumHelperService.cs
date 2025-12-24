using EduTrack.Service.DTOs;
using EduTrack.Service.Interfaces;

namespace EduTrack.Service.Services;

public class EnumHelperService : IEnumHelperService
{
    /// <summary>
    /// Generic method to convert any enum to list of items
    /// </summary>
    public List<EnumItem> GetEnumList<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(e => new EnumItem
            {
                Value = Convert.ToInt32(e),
                Text = e.ToString()
            })
            .ToList();
    }

    /// <summary>
    /// Specifically for UserRole enum
    /// </summary>
    public List<EnumItem> GetRoleList()
    {
        return GetEnumList<EduTrack.Domain.Enums.UserRole>();
    }
}
