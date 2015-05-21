using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Common.Mapping;

namespace PointEx.Entities.Dto
{
    public class EducationalInstitutionDto : IMapFrom<EducationalInstitution>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TownName { get; set; }
    }
}
