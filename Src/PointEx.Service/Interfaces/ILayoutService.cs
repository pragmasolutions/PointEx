using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface ILayoutService
    {
        List<MenuItem> GetAdminMenuItems(string roleName);
    }
}