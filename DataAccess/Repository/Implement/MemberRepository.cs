using BusinessObject.Object;
using DataAccess.DAO;
using DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.Implement
{
    public class MemberRepository : IMemberRepository
    {
        public void AddMember(Member member)
        => MemberDAO.Instance.AddMember(member);

        public Member checkDeleteMember(int memberID)
        => MemberDAO.Instance.checkDeleteMember(memberID);

        public void DeleteMember(int MemberID)
        => MemberDAO.Instance.DeleteMember(MemberID);

        public bool DuplicatedDefaultMember(Member member)
        => MemberDAO.Instance.DuplicatedDefaultMember(member);

        public Member DuplicatedEmail(string email)
        => MemberDAO.Instance.DuplicatedEmail(email);

        public Member DuplicatedMemberID(string ID)
        => MemberDAO.Instance.DuplicatedMemberID(ID);

        public Member GetMemberByID(int memberID)
        => MemberDAO.Instance.GetMemberByID(memberID);

        public IEnumerable<Member> GetMemberList()
        => MemberDAO.Instance.GetMemberList();

        public Member Login(string email, string password)
        => MemberDAO.Instance.Login(email,password);

        public IEnumerable<Member> SearchCity(string City)
        => MemberDAO.Instance.SearchCity(City);

        public IEnumerable<Member> SearchCompanyName(string companyName)
        => MemberDAO.Instance.SearchCompanyName(companyName);

        public IEnumerable<Member> SearchID(int ID)
        => MemberDAO.Instance.SearchID(ID);

        public IEnumerable<Member> SearchIDAndCompanyName(int ID, string companyName)
        => MemberDAO.Instance.SearchIDAndCompanyName(ID,companyName);

        public void UpdateMember(Member member)
        => MemberDAO.Instance.UpdateMember(member);
    }
}
