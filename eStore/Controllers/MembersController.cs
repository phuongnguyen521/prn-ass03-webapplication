using BusinessObject.Object;
using DataAccess.Repository.Implement;
using DataAccess.Repository.Interface;
using eStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        private IMemberRepository memberRepository = null;
        private AuthorityController authorityController = new AuthorityController();
        public MembersController()
        {
            memberRepository = new MemberRepository();

        }
        private bool First = true;
        private Member memberTemp = null;
        // GET: MemberController
        public async Task<IActionResult> Index(int? page)
        {

            if (page == null)
                page = 1;
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                IEnumerable<Member> memberList = memberRepository.GetMemberList();
                int pageSize = 5;
                return View(await PaginatedList<Member>
                    .CreateAsync(memberList.AsQueryable(), page ?? 1, pageSize));
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // GET: MemberController/Details/5
        public ActionResult Details(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (id == null)
                    return NotFound();
                var member = memberRepository.GetMemberByID(id.Value);
                if (member == null)
                    return NotFound();
                return View(member);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }
        //GET: MemberController/Profile/{id}
        public ActionResult Profile()
        {
            try
            {
                string id = HttpContext.Session.GetString("LoginUserEmail");
                if (String.IsNullOrEmpty(id))
                    return NotFound();
                int memberID = Convert.ToInt32(id);
                var member = memberRepository.GetMemberByID(memberID);
                if (member == null)
                    return NotFound();
                return View(member);
            }
            catch
            {
                return NotFound();
            }

        }

        // GET: MemberController/Create
        [HttpGet]
        // POST: MemberController/Create
        public ActionResult Create()
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (First)
                    return View();
                else
                    return View(memberTemp);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: MemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    String errorEmail = ValidationEmail(member);
                    string errorID = ValidationID(member); 
                    if (errorID == null && errorEmail == null)
                    {
                        First = true;
                        memberRepository.AddMember(member);
                        memberTemp = null;
                    }
                    else
                    {
                        if (errorID != null && errorEmail != null)
                            ViewBag.Message = errorID + "\n" + errorEmail;
                        else if (errorID != null)
                            ViewBag.Message = errorID;
                        else
                            ViewBag.Message = errorEmail;
                        First = false;
                        memberTemp = member;
                        return View();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(member);
            }
        }

        public String ValidationEmail(Member member)
        {
            string error = null;
            Member _member = memberRepository.DuplicatedEmail(member.Email);
            bool check = !_member.Email.Equals(member.Email);
            if (memberRepository.DuplicatedDefaultMember(member) || check)
                error = "Duplicated Email";
            return error;
        }
        public String ValidationID(Member member)
        {
           string error = null;
            if (memberRepository.DuplicatedMemberID(Convert.ToString(member.MemberId)) != null)
                error = "Duplicated ID";
            return error;
        }

        // GET: MemberController/Edit/5
        public ActionResult Edit(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                Member member = null;
                if (First)
                {
                    if (id == null)
                        return NotFound();
                    member = memberRepository.GetMemberByID(id.Value);
                    if (member == null)
                        return NotFound();
                }
                else
                    member = memberTemp;
                return View(member);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: MemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Member member)
        {
            try
            {
                if (id != member.MemberId)
                {
                    ViewBag.Message = "Member ID shall not be changed";
                    return RedirectToAction(nameof(Edit));
                }
                if (ModelState.IsValid)
                {
                    string error = ValidationEmail(member);
                    if (error == null)
                    {
                        memberRepository.UpdateMember(member);
                        memberTemp = null;
                        First = true;
                        string user = HttpContext.Session.GetString("LoginUserEmail");
                        if (!user.Equals("admin@estore.com"))
                            return RedirectToAction(nameof(Profile));
                        else
                            return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Message  = error;
                        memberTemp = member;
                        First = false;
                        return RedirectToAction(nameof(Edit));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(member);
            }
        }

        // GET: MemberController/Delete/5
        public ActionResult Delete(int? id)
        {
            string path = Request.Path.ToString();
            string user = HttpContext.Session.GetString("LoginUserEmail");
            if (authorityController.checkAuthority(path, user))
            {
                if (id == null)
                    return NotFound();
                var member = memberRepository.GetMemberByID(id.Value);
                if (member == null)
                    return NotFound();
                return View(member);
            }
            return RedirectToAction("UserDenialPermission", "Home");
        }

        // POST: MemberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Member member = memberRepository.checkDeleteMember(id);
                if (member == null)
                {
                    memberRepository.DeleteMember(id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "Cannot Delete this member due to orders";
                    return RedirectToAction(nameof(Delete));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
