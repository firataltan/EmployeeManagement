namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? TCKimlikNo { get; set; }
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
