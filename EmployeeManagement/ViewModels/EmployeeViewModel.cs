
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.ComponentModel.DataAnnotations;
 

namespace EmployeeManagement.ViewModels
{
    public class EmployeeViewModel
    {
        
        public int Id { get; set; }

        //[Required(ErrorMessage = "tc alanı boş olamaz")]
        //[Remote(action: "HasEmployeeTc", controller: "Employee")]                 
        public string? TCKimlikNo { get; set; }

        [Required(ErrorMessage ="Ad alanı boş olamaz")]
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Departman { get; set; }
        public string? Telefon { get; set; }
        public string? Statü { get; set; }
        public string? Unvan { get; set; }
        public string? Görev { get; set; }
        public string? EğitimDurumu { get; set; }
        public string? Bolum { get; set; }
        public string? Cinsiyet { get; set; }
        public DateTime İşeGirişTarihi { get; set; }
        public string? KanGrubu { get; set; }
        public string? EngellilikDurumu { get; set; }

        public string? Aktiflik { get; set; }

        
    }
}
