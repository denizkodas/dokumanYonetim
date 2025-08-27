using dokumanYonetim.Data;
using dokumanYonetim.Models;
using dokumanYonetim.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace dokumanYonetim.Controllers

{
	[Authorize]
	public class UserController : Controller
    {
        private readonly IUserRepository _userRepository1;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Category> _categoryRepository;
        

        public UserController(IUserRepository userRepository, IRepository<Category> categoryRepository)
        {
            _userRepository1 = userRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var users = _userRepository1.GetAll()?.ToList() ?? new List<User>();
            return View(users);


        }

        public IActionResult Ekle()
        {
            var users = _userRepository1.GetAll().ToList();
            ViewBag.Users = users; // Kullanıcı listesini ViewBag ile view'a gönderiyoruz
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(User user)
        {
            
               _userRepository1.Ekle(user);
               _userRepository1.Kaydet();
                TempData["Successful"] = "Info:New user created successfully!";
                return RedirectToAction("Index", "User"); //action adı,controller adı                      
           
        }

        public IActionResult Guncelle(int? id)
        {
           

            if (id==null ||id==0)
            {
                return NotFound(); //NotFound Ekranını getir
            }

            User? userDb = _userRepository1.Get(u=>u.UserId==id);
            if (userDb == null)
            {
                return NotFound();
            }
            return View(userDb);
           
        }

        [HttpPost]
        public IActionResult Guncelle(User user)
        {


            _userRepository1.Guncelle(user);
            _userRepository1.Kaydet();
            TempData["Successful"] = "Info:The user has been updated successfully!";
            return RedirectToAction("Index", "User"); //action adı,controller adı

        }

        //HttpGet Action
        public IActionResult Sil(int? id)
        {
            

            if (id == null || id == 0)
            {
                return NotFound(); //NotFound Ekranını getir
            }

            User? userDb = _userRepository1.Get(u=>u.UserId== id);
            if (userDb == null)
            {
                return NotFound();
            }
            return View(userDb);

        }

        [HttpPost]
        public IActionResult Sil(User user)
        {


            _userRepository1.Sil(user);
            _userRepository1.Kaydet();
            TempData["Successful"] = "Info:The user was deleted successfully!";
            return RedirectToAction("Index", "User"); //action adı,controller adı

        }


    }
}
