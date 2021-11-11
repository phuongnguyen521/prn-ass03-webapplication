using BusinessObject.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Interface
{
    public interface IMemberRepository
    {
        public IEnumerable<Member> GetMemberList();
        public Member GetMemberByID(int memberID);
        public Member Login(string email, string password);
        public Member DuplicatedEmail(string email);
        public Member DuplicatedMemberID(string ID);
        public bool DuplicatedDefaultMember(Member member);
        public void AddMember(Member member);
        public void UpdateMember(Member member);
        public Member checkDeleteMember(int memberID);
        public void DeleteMember(int MemberID);
        public IEnumerable<Member> SearchIDAndCompanyName(int ID, string companyName);
        public IEnumerable<Member> SearchID(int ID);
        public IEnumerable<Member> SearchCompanyName(string companyName);
        public IEnumerable<Member> SearchCity(string City);
    }
}
