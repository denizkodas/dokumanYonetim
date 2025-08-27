using dokumanYonetim.Data;
using dokumanYonetim.Models;
using dokumanYonetim.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dokumanYonetim.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Category> _categoryRepository;

        public DocumentController(IDocumentRepository documentRepository, IRepository<User> userRepository, IRepository<Category> categoryRepository)
        {
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(string searchTerm, int? categoryId, DateTime? startDate, DateTime? endDate)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Geçersiz kullanıcı kimliği.");
            }

            var allDocuments = _documentRepository.GetAll()
               .Include(d => d.CreatedByUser)
               .ToList();

            var documents = allDocuments.Where(d =>
                d.CreatedByUserId == userIdInt ||
                (d.CreatedByUser != null &&
                 d.CreatedByUser.ManagerId.HasValue &&
                 d.CreatedByUser.ManagerId.Value == userIdInt &&
                 d.IsApproved == false)) // Onay bekleyen dökümanlar
                .ToList();

            // Diğer filtreleme kriterleri
            if (!string.IsNullOrEmpty(searchTerm))
            {
                documents = documents.Where(d => d.DocumentTitle.Contains(searchTerm)).ToList();
            }

            if (categoryId.HasValue)
            {
                documents = documents.Where(d => d.CategoryId == categoryId.Value).ToList();
            }

            if (startDate.HasValue)
            {
                documents = documents.Where(d => d.UploadDate >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                documents = documents.Where(d => d.UploadDate <= endDate.Value).ToList();
            }

            return View(documents);
        }




        public IActionResult Ekle()
        {
            ViewBag.Users = _userRepository.GetAll().ToList();
            ViewBag.Categories = _categoryRepository.GetAll().ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Document document, IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = _userRepository.Get(u => u.UserId == int.Parse(userId));
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine("wwwroot/Documents", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                document.FilePath = "Documents/" + fileName;
                document.FileType = Path.GetExtension(file.FileName);
                document.UploadDate = DateTime.Now;
            }

            document.CreatedByUserId = user.UserId;
            document.IsApproved = false;

            // Menajeri mevcutsa dökümanı onaylaması için yönlendir
            document.CurrentApproverId = user.ManagerId;
            document.HighestManagerId = user.ManagerId; // En üst yönetici olarak belirliyoruz.

            _documentRepository.Ekle(document);
            _documentRepository.Kaydet();

            TempData["Successful"] = "Info: New document created successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Guncelle(int? id)
        {
            ViewBag.Users = _userRepository.GetAll().ToList();
            ViewBag.Categories = _categoryRepository.GetAll().ToList();

            if (id == null || id == 0)
            {
                return NotFound();
            }

            Document? documentDb = _documentRepository.Get(u => u.DocumentId == id);
            if (documentDb == null)
            {
                return NotFound();
            }
            return View(documentDb);
        }

        [HttpPost]
        public IActionResult Guncelle(Document document, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine("wwwroot/Documents", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                document.FilePath = "Documents/" + fileName;
                document.FileType = Path.GetExtension(file.FileName);
                document.UploadDate = DateTime.Now;
            }

            _documentRepository.Guncelle(document);
            _documentRepository.Kaydet();
            TempData["Successful"] = "Info: The document has been updated successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Sil(int? id)
        {
            ViewBag.Users = _userRepository.GetAll().ToList();
            ViewBag.Categories = _categoryRepository.GetAll().ToList();

            if (id == null || id == 0)
            {
                return NotFound();
            }

            Document? documentDb = _documentRepository.Get(u => u.DocumentId == id);
            if (documentDb == null)
            {
                return NotFound();
            }
            return View(documentDb);
        }

        [HttpPost]
        public IActionResult Sil(Document document)
        {
            var existingDocument = _documentRepository.Get(d => d.DocumentId == document.DocumentId);
            if (existingDocument == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine("wwwroot/Documents", Path.GetFileName(existingDocument.FilePath));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _documentRepository.Sil(existingDocument);
            _documentRepository.Kaydet();
            TempData["Successful"] = "Info: The document was deleted successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Onayla(int id)
        {
            var document = _documentRepository.Get(d => d.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (document.CurrentApproverId != int.Parse(userId))
            {
                return Unauthorized("Bu dökümanı onaylamaya yetkiniz yok.");
            }

            // bir sonraki yöneticiyi bulmak için;
            var managerId = _userRepository.Get(u => u.UserId == document.CurrentApproverId)?.ManagerId;

            if (managerId != null)
            {
                document.CurrentApproverId = managerId; // bir sonraki yöneticiye aktar.
                _documentRepository.Guncelle(document);
                _documentRepository.Kaydet();
                TempData["Successful"] = "Bilgi: Döküman bir üst yöneticiye gönderildi!";
                return RedirectToAction("Index");
            }

            // en üst yönetici onaylıyorsa tamamen onayla
            document.IsApproved = true;
            _documentRepository.Guncelle(document);
            _documentRepository.Kaydet();

            TempData["Successful"] = "Bilgi: Döküman tamamen onaylandı!";
            return RedirectToAction("Index");
        }





        public IActionResult Reddet(int id)
        {
            var document = _documentRepository.Get(d => d.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            document.IsApproved = false; // Reddet
            _documentRepository.Guncelle(document);
            _documentRepository.Kaydet();

            TempData["Successful"] = "Info: Document has been rejected successfully!";
            return RedirectToAction("Index");
        }
    }
}
