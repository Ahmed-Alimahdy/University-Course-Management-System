using static System.Runtime.InteropServices.JavaScript.JSType;
using universityManagementSys.ApiServices.DTOs;
using AutoMapper;
using universityManagementSys.Models;
namespace universityManagementSys.DTOs
{
    
public class DTOsMapper : Profile
    {
        public DTOsMapper()
        {
            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>();
                
        }
    }
}
