using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymApp.Models;

namespace GymApp.Services
{
    //Service layer is a lot like the repsitory layer but it handles the business logic from the users perspective where as the repository layer is from the databases perspective
    //Service layer is a lot like the repsitory layer but it handles the business logic from the users perspective where as the repository layer is from the databases perspective
    public interface IMemberService
    {
        void RegisterMember(Member member);
        Member GetMemberByName(string fname, string sname);
        Member GetMemberById(int memId);
        void UpdateMember(Member member, string originalFname, string originalSname);
        void RenewMember(int memId);
        void CloseMembership(int memId);
        void AddFunds(string fname, string sname, double amount);
        List<Member> GetInactiveMembers();
        List<Member> GetActiveMembers();
    }
}