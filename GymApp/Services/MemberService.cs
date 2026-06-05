using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymApp.Models;
using GymApp.Repositories;

namespace GymApp.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public void RegisterMember(Member member)
        {
            member.DateRegistered = DateTime.Now;
            member.Status = "A";
            member.Wallet = 0;
            _memberRepository.Add(member);
        }

        public Member GetMemberByName(string fname, string sname)
        {
            return _memberRepository.GetByName(fname, sname);
        }

        public Member GetMemberById(int memId)
        {
            return _memberRepository.GetById(memId);
        }

        public void UpdateMember(Member member, string originalFname, string originalSname)
        {
            _memberRepository.Update(member, originalFname, originalSname);
        }

        public void RenewMember(int memId)
        {
            _memberRepository.SetStatus(memId, "A");
        }

        public void CloseMembership(int memId)
        {
            _memberRepository.SetStatus(memId, "I");
        }

        public void AddFunds(string fname, string sname, double amount)
        {
            _memberRepository.AddFunds(fname, sname, amount);
        }

        public List<Member> GetInactiveMembers()
        {
            return _memberRepository.GetByStatus("I");
        }

        public List<Member> GetActiveMembers()
        {
            return _memberRepository.GetByStatus("A");
        }
    }
}