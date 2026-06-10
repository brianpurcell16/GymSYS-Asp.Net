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
    public class BookingServiceTests
    {
        private Mock<IMemberRepository> _mockMemberRepo;
        private Mock<IClassRepository> _mockClassRepo;
        private Mock<IBookingRepository> _mockBookingRepo;
        private BookingService _bookService;

        [TestInitialize]
        public void Setup()
        {
            _mockMemberRepo = new Mock<IMemberRepository>();
            _mockClassRepo = new Mock<IClassRepository>();
            _mockBookingRepo = new Mock<IBookingRepository>();

            _bookService = new BookingService(
                _mockMemberRepo.Object,
                _mockClassRepo.Object,
                _mockBookingRepo.Object
                );
        }

        [TestMethod]
        public void BookClass_ReturnsMemberNotFound_WhenMemberDoesNotExist()
        {
            _mockMemberRepo.Setup(x => x.GetByName("Ghost", "User")).Returns((Member)null);
            Assert.AreEqual(BookingResult.MemberNotFound, _bookService.BookClass("Ghost", "User", 1));
        }

        [TestMethod]
        public void BookClass_ReturnsClassFull_WhenDecrementFails()
        {
            _mockMemberRepo.Setup(r => r.GetByName("Brian", "Purcell"))
                .Returns(new Member { MemID = 1, Wallet = 50 });
            _mockClassRepo.Setup(r => r.GetById(1))
                .Returns(new GymClass { ClassID = 1, Price = 10, Status = "A", AvailableSpaces = 0 });
            _mockClassRepo.Setup(r => r.DecreaseSpaces(1)).Returns(false); // sp returned 0 rows

            Assert.AreEqual(BookingResult.ClassFull,
                _bookService.BookClass("Brian", "Purcell", 1));
        }

        [TestMethod]
        public void BookClass_ReturnsInsufficientFunds_AndRestoresSpace_WhenWalletLow()
        {
            _mockMemberRepo.Setup(r => r.GetByName("Brian", "Purcell"))
                .Returns(new Member { MemID = 1, Wallet = 5 });
            _mockClassRepo.Setup(r => r.GetById(1))
                .Returns(new GymClass { ClassID = 1, Price = 10, Status = "A", AvailableSpaces = 5 });
            _mockClassRepo.Setup(r => r.DecreaseSpaces(1)).Returns(true);
            _mockBookingRepo.Setup(r => r.DeductWallet("Brian", "Purcell", 1)).Returns(false);

            var result = _bookService.BookClass("Brian", "Purcell", 1);

            Assert.AreEqual(BookingResult.InsufficientFunds, result);
            // Must restore the space that was decremented before the wallet check failed
            _mockClassRepo.Verify(r => r.IncreaseSpaces(1), Times.Once);
        }

        [TestMethod]
        public void BookClass_ReturnsSuccess_AndCreatesBooking_WhenAllConditionsMet()
        {
            _mockMemberRepo.Setup(r => r.GetByName("Brian", "Purcell"))
                .Returns(new Member { MemID = 1, Wallet = 50 });
            _mockClassRepo.Setup(r => r.GetById(1))
                .Returns(new GymClass { ClassID = 1, Price = 10, Status = "A", AvailableSpaces = 5 });
            _mockClassRepo.Setup(r => r.DecreaseSpaces(1)).Returns(true);
            _mockBookingRepo.Setup(r => r.DeductWallet("Brian", "Purcell", 1)).Returns(true);

            var result = _bookService.BookClass("Brian", "Purcell", 1);

            Assert.AreEqual(BookingResult.Success, result);
            _mockBookingRepo.Verify(r => r.CreateBooking(1, 1, 10), Times.Once);
        }

        [TestMethod]
        public void CancelBooking_CallsRepository_Once()
        {
            _bookService.CancelBooking(7);
            // sp_CancelBooking handles the full transaction — just verify it was called
            _mockBookingRepo.Verify(r => r.CancelBooking(7), Times.Once);
        }
    }
}
