using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApp.Models;
using GymApp.Repositories;
using GymApp.Services;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GymApp.Tests.Services
{
    [TestClass]
    public class MemberServiceTests
    {
        private Mock<IMemberRepository> _memberRepositoryMock;
        private MemberService _memberService;

        [TestInitialize]
        public void Setup()
        {
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _memberService = new MemberService(_memberRepositoryMock.Object);
        }

        [TestMethod]
        public void RegisterMember_SetStatusToActive()
        {
            var member = new Member { Fname = "Brian", Sname = "Purcell" };
            _memberService.RegisterMember(member);
            Assert.AreEqual("A", member.Status);
        }

        [TestMethod]
        public void RegisterMember_SetWalletToZero()
        {
            var member = new Member { Fname = "Steve", Sname = "Blum" };
            _memberService.RegisterMember(member);
            Assert.AreEqual(0, member.Wallet);
        }

        [TestMethod]
        public void RegisterMember_CallsRepositoryAdd_Once()
        {
            var member = new Member { Fname = "Bernardo", Sname = "Silva" };
            _memberService.RegisterMember(member);
            _memberRepositoryMock.Verify(x => x.Add(member), Times.Once);
        }

        [TestMethod]
        public void RenewMember_CallsSetStatus_WithActiveFlag()
        {
            _memberService.RenewMember(5);
            _memberRepositoryMock.Verify(x => x.SetStatus(5, "A"), Times.Once);
        }

        [TestMethod]
        public void CloseMembership_CallsSetStatus_WithInactiveFlag()
        {
            _memberService.CloseMembership(3);
            _memberRepositoryMock.Verify(r => r.SetStatus(3, "I"), Times.Once);
        }

        [TestMethod]
        public void GetMemberByName_ReturnsCorrectMember()
        {
            var expected = new Member { MemID = 1, Fname = "Brian", Sname = "Purcell" };
            _memberRepositoryMock.Setup(r => r.GetByName("Brian", "Purcell")).Returns(expected);

            var result = _memberService.GetMemberByName("Brian", "Purcell");

            Assert.AreEqual(expected.MemID, result.MemID);
        }

        [TestMethod]
        public void GetMemberByName_ReturnsNull_WhenNotFound()
        {
            _memberRepositoryMock.Setup(r => r.GetByName("Ghost", "User")).Returns((Member)null);
            Assert.IsNull(_memberService.GetMemberByName("Ghost", "User"));
        }


    }
}
