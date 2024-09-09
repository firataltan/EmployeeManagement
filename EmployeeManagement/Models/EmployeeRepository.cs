namespace EmployeeManagement.Models
{
    public class EmployeeRepository
    {
        private static List<Employee> _employees=new List<Employee>()
        {
            /* new Employee { Id = 1, TCKimlikNo = "12345678901", Ad = "Ahmet", Soyad = "Yılmaz", Departman = "Bilgi Teknolojileri", Telefon = "0555 555 5555", Statü = "Aktif", Unvan = "Mühendis", Görev = "Yazılım Geliştirici", EğitimDurumu = "Lisans", Bolum = "Bilgisayar Mühendisliği", Cinsiyet = "Erkek", İşeGirişTarihi = new DateTime(2020, 1, 15), KanGrubu = "A+", EngellilikDurumu = "Yok", Aktiflik = "Aktif" },
             new Employee { Id = 2, TCKimlikNo = "23456789012", Ad = "Ayşe", Soyad = "Kara", Departman = "Muhasebe", Telefon = "0544 444 4444", Statü = "Aktif", Unvan = "Muhasebeci", Görev = "Finans Uzmanı", EğitimDurumu = "Lisans", Bolum = "İşletme", Cinsiyet = "Kadın", İşeGirişTarihi = new DateTime(2019, 3, 10), KanGrubu = "B+", EngellilikDurumu = "Yok", Aktiflik = "Aktif" },
             new Employee { Id = 3, TCKimlikNo = "34567890123", Ad = "Mehmet", Soyad = "Demir", Departman = "İnsan Kaynakları", Telefon = "0533 333 3333", Statü = "Pasif", Unvan = "Uzman", Görev = "İK Uzmanı", EğitimDurumu = "Lisans", Bolum = "Çalışma Ekonomisi", Cinsiyet = "Erkek", İşeGirişTarihi = new DateTime(2018, 5, 20), KanGrubu = "0+", EngellilikDurumu = "Yok", Aktiflik = "Pasif" },
        */
        };

        public List<Employee> GetAll() => _employees;

        public void Add(Employee newEmployee)=> _employees.Add(newEmployee);

        public void Remove(int id)
        {
            var hasEmployee= _employees.FirstOrDefault(x => x.Id == id);
            if (hasEmployee == null)
            {
                throw new Exception($"Bu id: ({id}) bulunamamıştır");

            }
            _employees.Remove(hasEmployee);
        }
        public void Update(Employee updateEmployee)
        {
            var hasEmployee = _employees.FirstOrDefault(x => x.Id == updateEmployee.Id);
            if (hasEmployee == null)
            {
                throw new Exception($"Bu id: ({updateEmployee.Id}) bulunmamaktadır");

            }
            hasEmployee.Departman= updateEmployee.Departman;
            hasEmployee.Unvan = updateEmployee.Unvan;
            hasEmployee.Statü = updateEmployee.Statü;
            hasEmployee.Görev = updateEmployee.Görev;
            hasEmployee.Bolum = updateEmployee.Bolum;
             
            var index = _employees.FindIndex(x => x.Id == updateEmployee.Id);
            _employees[index]=updateEmployee;

        }
    }
}
