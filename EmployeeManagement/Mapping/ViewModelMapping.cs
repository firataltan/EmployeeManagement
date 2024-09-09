using AutoMapper;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;

namespace EmployeeManagement.Mapping
{
    public class ViewModelMapping:Profile
    {
        public ViewModelMapping()
        {

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        }
    }
}
