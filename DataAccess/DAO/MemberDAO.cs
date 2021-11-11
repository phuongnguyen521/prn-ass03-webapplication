using BusinessObject.Object;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class MemberDAO
    {
        #region Initialized Objects
        //Using Singleton Pattern
        private static MemberDAO instance = null;
        private static readonly Object instanceLock = new object();
        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new MemberDAO();
                }
                return instance;
            }
        }
        #endregion

        private Member DefaultMember()
        {
            Member Default = null;
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            string email = config["Member:Default:email"];
            string password = config["Member:Default:password"];
            Default = new Member
            {
                MemberId = 1,
                Email = email,
                Password = password,
                City = "Ho Chi Minh",
                CompanyName = "Fstore",
                Country = "Viet Nam"
            };
            return Default;
        }

        public IEnumerable<Member> GetMemberList()
        {
            IEnumerable<Member> members = null;
            try
            {
                using var context = new SaleManagementContext();
                members = context.Members.ToList();
                members = members.Append(DefaultMember());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members.OrderBy(temp => temp.MemberId);
        }
        public Member GetMemberByID(int memberID)
        {
            Member member = null;
            try
            {   
                using var context = new SaleManagementContext();
                var list = context.Members.ToList();
                list.Add(DefaultMember());
                member = list.SingleOrDefault(temp => temp.MemberId == memberID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public Member Login(string email, string password)
        {
            Member member = null;
            try
            {
                IEnumerable<Member> members = GetMemberList();
                member = members.SingleOrDefault(temp
                    => temp.Email.Equals(email) && temp.Password.Equals(password));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public Member DuplicatedEmail(string email)
        {
            Member member = null;
            IEnumerable<Member> members = GetMemberList();
            member = members.SingleOrDefault(temp
                => temp.Email.Equals(email));
            return member;
        }

        public Member DuplicatedMemberID(string ID)
        {
            if (String.IsNullOrEmpty(ID))
                throw new Exception("ID is null");
            int key = Int32.Parse(ID);
            Member member = GetMemberList().SingleOrDefault(temp => temp.MemberId == key);
            return member;
        }

        public bool DuplicatedDefaultMember(Member member)
        {
            Member Default = DefaultMember();
            return member.Email.Equals(Default.Email) && member.Password.Equals(Default.Password);
        }

        public void AddMember(Member member)
        {
            try
            {
                Member _member = GetMemberByID(member.MemberId);
                if (_member == null)
                {
                    using var context = new SaleManagementContext();
                    context.Members.Add(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Member exists!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateMember(Member member)
        {
            try
            {
                Member _member = GetMemberByID(member.MemberId);
                if (_member != null)
                {
                    using var context = new SaleManagementContext();
                    context.Members.Update(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Member does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Member checkDeleteMember(int memberID)
        {
            Member member = null;
            try
            {
                using var context = new SaleManagementContext();
                IQueryable<Member> members = context.Members.Include(c => c.Orders);
                members = members.Where(temp => temp.Orders.Count > 0);
                member = members.SingleOrDefault(temp => temp.MemberId == memberID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public void DeleteMember(int MemberID)
        {
            try
            {
                Member member = GetMemberByID(MemberID);
                if (member != null)
                {
                    using var context = new SaleManagementContext();
                    context.Members.Remove(member);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Member does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Member> SearchIDAndCompanyName(int ID, string companyName)
        {
            IEnumerable<Member> members = null;
            IEnumerable<Member> MemberList = GetMemberList();
            var result = from temp in MemberList
                         where temp.MemberId == ID
                         && temp.CompanyName.ToLower().Trim().Contains(companyName.Trim().ToLower())
                         select temp;
            members = result;
            return members;
        }

        public IEnumerable<Member> SearchID(int ID)
        {
            IEnumerable<Member> members = null;
            IEnumerable<Member> MemberList = GetMemberList();
            var result = from temp in MemberList
                         where temp.MemberId == ID
                         select temp;
            members = result;
            return members;
        }

        public IEnumerable<Member> SearchCompanyName(string companyName)
        {
            IEnumerable<Member> members = null;
            IEnumerable<Member> MemberList = GetMemberList();
            var result = MemberList.Where(temp
                => temp.CompanyName.ToLower().Trim().Contains(companyName.Trim().ToLower()));
            members = result;
            return members;
        }

        public IEnumerable<Member> SearchCity(string City)
        {
            if (City == null || String.IsNullOrEmpty(City))
            {
                throw new Exception("Cty is null or empty");
            }
            IEnumerable<Member> list = null;
            var key = City.Trim().ToLower();
            IEnumerable<Member> MemberList = GetMemberList();
            if (MemberList != null)
            {
                if (City.Equals("All"))
                    return MemberList;
                list = MemberList.Where(temp => temp.City.Equals(City));
            }
            else
                throw new Exception("There is no user!");
            return list;
        }
        public IEnumerable<String> GetCityList()
        {
            IEnumerable<String> MemberList = null;
            IEnumerable<Member> List = GetMemberList();
            if (List != null)
            {
                MemberList = List.Select(temp => temp.City);
                MemberList = MemberList.Distinct();
                MemberList = MemberList.Prepend("All");
            }
            else
                throw new Exception("There is no user!");
            return MemberList;
        }

        public IEnumerable<Member> SearchCountry(string Country)
        {
            if (Country == null || String.IsNullOrEmpty(Country))
            {
                throw new Exception("Country is null or empty");
            }
            IEnumerable<Member> list = null;
            var key = Country.Trim();
            IEnumerable<Member> MemberList = GetMemberList();
            if (MemberList != null)
            {
                if (Country.Equals("All"))
                    return MemberList;
                var tempUser = MemberList.Where(temp => temp.Country.Equals(key));
                if (tempUser.Any())
                    list = tempUser;
            }
            else
                throw new Exception("There is no user!");
            return list;
        }
        public IEnumerable<String> GetCountryList()
        {
            IEnumerable<String> MemberList = null;
            IEnumerable<Member> List = GetMemberList();
            if (List != null)
            {
                MemberList = List.Select(temp => temp.Country);
                MemberList = MemberList.Distinct();
                MemberList = MemberList.Prepend("All");
            }
            else
                throw new Exception("There is no user!");
            return MemberList;
        }

    }
}
