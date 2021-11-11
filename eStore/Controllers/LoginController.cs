using BusinessObject.Object;
using DataAccess.Repository.Implement;
using DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace eStore.Controllers
{
    public class LoginController : Controller
    {
        private IMemberRepository memberRepository = null;
        public LoginController()
        {
            memberRepository = new MemberRepository();
        }

        // GET: LoginController
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login ([FromForm] string email,[FromForm]string password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Member member = memberRepository.Login(email, password);
                    if (member != null)
                    {
                        //add Session
                        if (member.Email.Equals("admin@estore.com"))
                        {
                            HttpContext.Session.SetString("LoginUserEmail", "Admin");
                            return RedirectToAction("Index", "Members");
                        }
                        else
                        {
                            HttpContext.Session.SetString("LoginUserEmail", Convert.ToString(member.MemberId));
                            return RedirectToAction("Profile", "Members");
                        }
                    }
                    else
                        return RedirectToAction("LoginError", "Login");
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // GET: LoginController
        public ActionResult LoginError()
        {
            return View();
        }
    }
}
