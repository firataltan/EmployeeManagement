using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.IO.Font;
using AutoMapper;
using EmployeeManagement.ViewModels;

public class EmployeeController : Controller
{
    private AppDbContext _context;

    private readonly IMapper _mapper;

    private readonly EmployeeRepository _employeeRepository;

    public EmployeeController(AppDbContext context, IMapper mapper)
    {
        _employeeRepository = new EmployeeRepository();
        _context = context;
        _mapper = mapper;
    }
    public IActionResult ExportToPdf(int searchID, string searchTCKimlikNo, string searchAd, string searchSoyad, string searchDepartman)
    {
        var filteredEmployees = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTCKimlikNo))
        {
            filteredEmployees = filteredEmployees.Where(e => e.TCKimlikNo == searchTCKimlikNo);
        }
        if (searchID != 0)
        {
            filteredEmployees = filteredEmployees.Where(e => e.Id == searchID);
        }
        if (!string.IsNullOrWhiteSpace(searchAd))
        {
            filteredEmployees = filteredEmployees.Where(e => e.Ad.ToLower() == searchAd.ToLower());
        }
        if (!string.IsNullOrWhiteSpace(searchSoyad))
        {
            filteredEmployees = filteredEmployees.Where(e => e.Soyad.ToLower() == searchSoyad.ToLower());
        }
        if (!string.IsNullOrWhiteSpace(searchDepartman))
        {
            filteredEmployees = filteredEmployees.Where(e => e.Departman.ToLower() == searchDepartman.ToLower());
        }

        var employees = filteredEmployees.ToList();

        using (var stream = new MemoryStream())
        {
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            // Türkçe karakter desteği sağlayan font kullanımı
            var fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "arial.ttf");
            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

            document.SetFont(font);

            document.Add(new Paragraph("Personel Listesi")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFontSize(14));

            var table = new Table(UnitValue.CreatePercentArray(new float[] { 0.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f }))
                .SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell().Add(new Paragraph("TCKimlikNo").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Ad").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Soyad").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Departman").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Telefon").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Statü").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Unvan").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Görev").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Eğitim Durumu").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Bölüm").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Cinsiyet").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("İşe Giriş Tarihi").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Kan Grubu").SetFontSize(6)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Engellilik Durumu").SetFontSize(6)));

            foreach (var emp in employees)
            {
                table.AddCell(new Cell().Add(new Paragraph(emp.TCKimlikNo).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Ad).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Soyad).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Departman).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Telefon).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Statü).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Unvan).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Görev).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.EğitimDurumu).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Bolum).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.Cinsiyet).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.İşeGirişTarihi.ToString("dd.MM.yyyy")).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.KanGrubu).SetFontSize(4)));
                table.AddCell(new Cell().Add(new Paragraph(emp.EngellilikDurumu).SetFontSize(4)));
            }

            document.Add(table);
            document.Close();

            return File(stream.ToArray(), "application/pdf", "FilteredEmployeeList.pdf");
        }
    }





    public IActionResult Remove(int id)
    {
        var employee = _context.Employees.Find(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Kayıt başarıyla silindi!";
        }
        else
        {
            TempData["SuccessMessage"] = "Kayıt bulunamadı!";
        }

        return RedirectToAction("Index");
    }

    public IActionResult Add()
    {


        return View();
    }

    [HttpPost]
    public IActionResult Add(EmployeeViewModel newEmployee)
    {
        if (ModelState.IsValid)
        {
            _context.Employees.Add(_mapper.Map<Employee>(newEmployee));
            _context.SaveChanges();

            TempData["status"] = "Personel Başarıyla Eklendi ";
            return RedirectToAction("Index");

        }
        else
        {
            return View();
        }

    }

     

     public IActionResult Update(int id)
     {
         var employee = _context.Employees.Find(id);

         return View(_mapper.Map<EmployeeViewModel>(employee));
     }

     




    [HttpPost]
    public IActionResult Update(EmployeeViewModel updateEmployee)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        _context.Employees.Update(_mapper.Map<Employee>(updateEmployee));
        _context.SaveChanges();
        TempData["status"] = "Personel Başarıyla Güncellendi";
        return RedirectToAction("Index");
    }
    

     


    [AcceptVerbs("GET", "POST")]
    public IActionResult HasEmployeeTc(string TCKimlikNo  )
    {
         

        var anyEmployee = _context.Employees.Any(x => x.TCKimlikNo == TCKimlikNo);
        if (anyEmployee)
        {
            return Json("Kaydetmeye çalıştığınız personel veritabanında bulunmaktadır");
        }
        else
        {
            return Json(true);
        }
    }

    [HttpPost]
    public JsonResult Create(EmployeeViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Personel ekleme işlemini gerçekleştir
            // Örnek: _context.Personeller.Add(model);
            // _context.SaveChanges();

            return Json(new { success = true });
        }

        return Json(new { success = false, message = "Hatalı bilgi girdiniz." });
    }


    public IActionResult Index(int searchID, string searchTCKimlikNo, string searchAd, string searchSoyad, string searchDepartman, string searchTelefon, string searchStatü,
string searchUnvan,
string searchGörev,
string searchEğitimDurumu,
string searchBolum,
string searchCinsiyet,
DateTime searchİşeGirişTarihiStart,
string searchKanGrubu,
string searchEngellilikDurumu,
string searchAktiflik)
    {
        // var filteredEmployees = _employeeRepository.GetAll().AsQueryable();
        var filteredEmployees = _context.Employees.ToList().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTCKimlikNo))
        {
            filteredEmployees = filteredEmployees.Where(e => e.TCKimlikNo == searchTCKimlikNo);
        }
        if (searchID != 0)
        {
            filteredEmployees = filteredEmployees.Where(e => e.Id == searchID);
        }

        if (!string.IsNullOrWhiteSpace(searchAd))
        {
            filteredEmployees = filteredEmployees.Where(e => e.Ad.Equals(searchAd, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchSoyad))
        {
            filteredEmployees = filteredEmployees.Where(e => e.Soyad.Equals(searchSoyad, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchDepartman))
        {
            filteredEmployees = filteredEmployees.Where(e => e.Departman.Equals(searchDepartman, StringComparison.OrdinalIgnoreCase));
        }
 


        ViewData["SearchTCKimlikNo"] = searchTCKimlikNo;
        ViewData["SearchAd"] = searchAd;
        ViewData["SearchSoyad"] = searchSoyad;
        ViewData["SearchDepartman"] = searchDepartman;
        ViewData["SearchTelefon"] = searchTelefon;
        ViewData["SearchStatü"] = searchStatü;
        ViewData["SearchUnvan"] = searchUnvan;
        ViewData["SearchGörev"] = searchGörev;
        ViewData["SearchEğitimDurumu"] = searchEğitimDurumu;
        ViewData["SearchBolum"] = searchBolum;
        ViewData["SearchCinsiyet"] = searchCinsiyet;
        ViewData["SearchİşeGirişTarihiStart"] = searchİşeGirişTarihiStart;

        ViewData["SearchKanGrubu"] = searchKanGrubu;
        ViewData["SearchEngellilikDurumu"] = searchEngellilikDurumu;
        ViewData["SearchAktiflik"] = searchAktiflik;

        bool isSearchPerformed = !string.IsNullOrEmpty(searchTCKimlikNo) ||
                             !string.IsNullOrEmpty(searchAd) ||
                             !string.IsNullOrEmpty(searchSoyad) ||
                             (!string.IsNullOrEmpty(searchDepartman) && searchDepartman != "Tümü") ||
                             !string.IsNullOrEmpty(searchTelefon) ||
                             !string.IsNullOrEmpty(searchStatü) ||
                             !string.IsNullOrEmpty(searchUnvan) ||
                             !string.IsNullOrEmpty(searchGörev) ||
                             !string.IsNullOrEmpty(searchEğitimDurumu) ||
                             !string.IsNullOrEmpty(searchBolum) ||
                             !string.IsNullOrEmpty(searchCinsiyet) ||
                             searchİşeGirişTarihiStart != DateTime.MinValue ||
                             !string.IsNullOrEmpty(searchKanGrubu) ||
                             !string.IsNullOrEmpty(searchEngellilikDurumu) ||
                             !string.IsNullOrEmpty(searchAktiflik);



        ViewBag.DepartmanList = new SelectList(new List<DepartmanList>()
    {
        new(){ Data="Basın Yayın ve Halkla İlişkiler" , Value="Basın Yayın ve Halkla İlişkiler"},
        new(){ Data="Bilgi Teknolojileri" , Value="Bilgi Teknolojileri"},
        new(){ Data="İnsan Kaynakları" , Value="İnsan Kaynakları"},

    }, "Value", "Data");


         
        return View(_mapper.Map<List<EmployeeViewModel>>(filteredEmployees.ToList()));
       // return View(filteredEmployees.ToList());
    }
}

