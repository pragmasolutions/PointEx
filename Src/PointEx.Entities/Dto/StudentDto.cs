﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Common.Mapping;

namespace PointEx.Entities.Dto
{
    public class StudentDto : IMapFrom<Student>
    {
        public int Id { get; set; }
        public int EducationalInstitutionId { get; set; }
        public string EducationalInstitutionName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
    }
}
