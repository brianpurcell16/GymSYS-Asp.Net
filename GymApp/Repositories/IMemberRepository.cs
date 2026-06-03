using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymApp.Models;


namespace GymApp.Repositories
{
    public interface IMemberRepository
    {
        void Add(Member member);
        Member GetByName(string fname, string sname);
        Member GetById(int memId);
        void Update(Member member, string originalFname, string originalSname);
        void SetStatus(int memId, string status);
        void AddFunds(string fname, string sname, double amount);
        List<Member> GetByStatus(string status);

    }
}