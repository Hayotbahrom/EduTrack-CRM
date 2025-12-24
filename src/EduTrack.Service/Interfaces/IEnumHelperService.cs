using EduTrack.Service.DTOs;

namespace EduTrack.Service.Interfaces
{
    public interface IEnumHelperService
    {
        List<EnumItem> GetEnumList<TEnum>() where TEnum : Enum;
        List<EnumItem> GetRoleList();
    }
}
