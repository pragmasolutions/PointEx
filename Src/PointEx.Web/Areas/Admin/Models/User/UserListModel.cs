using System.ComponentModel.DataAnnotations;
using PagedList;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Web.Models;

namespace PointEx.Web.Models
{
    public class UserListModel
    {
        public UserListModel(IPagedList<UserDto> list, UserListFiltersModel filters)
        {
            List = list;
            Filters = filters;
        }
        public IPagedList<UserDto> List { get; set; }

        public UserListFiltersModel Filters { get; set; }
    }
}