using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface IStudentService
    {
        void Create(Student student);
        void Edit(Student student);
        void Delete(int studentId);
        IQueryable<Student> GetAll();
        List<StudentDto> GetAll(string sortBy, string sortDirection, string criteria, int? category, int? townId, int pageIndex,
            int pageSize, out int pageTotal);
        Student GetById(int id);
        void Dispose();
    }
}