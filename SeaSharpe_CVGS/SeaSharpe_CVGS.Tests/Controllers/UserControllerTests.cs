using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SeaSharpe_CVGS.Controllers;
using SeaSharpe_CVGS.Models;
using System.Web.Mvc;
using System.Web;
using System.Security.Principal;
using Moq;
using System.Globalization;

namespace SeaSharpe_CVGS.Tests.Controllers
{
    class UserControllerTests
    {
        ApplicationDbContext db = null;

        [SetUp]
        public void Init()
        {
            // make connection
            db = new ApplicationDbContext(); 

            // seed the database if empty
            if (db.Members.Count() == 0)
            {
                new Migrations.Configuration().SeedDebug(db);
                Console.WriteLine("Seeded DB");
            }
            else
            {
                Console.WriteLine("DB Already seeded");
            }

        }

        [TearDown]
        public void Cleanup()
        {
            //db.Database.Delete();
        }

        [TestCase(1, "O", false, "Ned", "Flanders", "7058675309", "Ned@LeftHandStore.com", "11/17/1990", TestName = "Good Values 1")]
        [TestCase(1, "M", false, "Sandra", "Bulluck", "7058675309", "Sandy@Bulluck.com", "11/17/2990", TestName = "Good Values 2")]
        [TestCase(2, "F", true, null, null, null, null, null, TestName = "Good Values 3")]
        [TestCase(2, "F", null, "Jeorge", "Jetson", null, null, null, TestName = "Good Values 4")]
        [TestCase(3, "M", true, null, null, null, "Just@AnEmail.com", null, TestName = "Good Values 5")]
        public void Edit(int memberIndex, string gender, bool? marketing, string first, string last, string phone, string email, string dob)
        {
            // Arrange
            
            var member = db.Members.OrderBy(m => m.UserId).Skip(memberIndex - 1).FirstOrDefault();
            
            var controller = new UserController { DbContext = db };
            controller.ControllerContext = MockHelpers.GetControllerContext(db, member);
            db.SaveChanges();
            var postedMember = new Member {
                User = new ApplicationUser
                {
                    Id = member.User.Id,
                    UserName = member.User.UserName,
                    Email = email ?? member.User.Email,
                    DateOfBirth = dob == null ? member.User.DateOfBirth : DateTime.ParseExact(dob, "dd/mm/yyyy", CultureInfo.InvariantCulture),
                    PhoneNumber = phone ?? member.User.PhoneNumber,
                    Gender = gender ?? member.User.Gender,
                    LastName = last ?? member.User.LastName,
                    FirstName = first ?? member.User.FirstName,
                },
                Id = member.Id,
                IsEmailMarketingAllowed = marketing ?? member.IsEmailMarketingAllowed,
            };

            // Detact both user and member from context before calling action
            db.Entry(member.User).State = EntityState.Detached;
            db.Entry(member).State = EntityState.Detached;

            // Act

            var result = controller.Edit(postedMember, null, null,null);

            var memberFromDb = db.Members.Find(member.Id); // Get the member back from the db context
            if (gender != null) Assert.AreEqual(gender, memberFromDb.User.Gender, "Gender didn't get set");
            if (marketing != null) Assert.AreEqual(marketing, memberFromDb.IsEmailMarketingAllowed, "Marketing didn't get set");
            if (first != null) Assert.AreEqual(first, memberFromDb.User.FirstName, "First name didn't get set");
            if (last != null) Assert.AreEqual(last, memberFromDb.User.LastName, "Last name didn't get set");
            if (phone != null) Assert.AreEqual(phone, memberFromDb.User.PhoneNumber, "Phone number didn't get set");
            if (email != null) Assert.AreEqual(email, memberFromDb.User.Email, "Email didn't get set");
            if (dob != null) Assert.AreEqual(DateTime.ParseExact(dob, "dd/mm/yyyy", CultureInfo.InvariantCulture), memberFromDb.User.DateOfBirth, "Date of birth didn't get set");

            Console.WriteLine(controller.TempData["message"]);
            Assert.IsInstanceOf<RedirectToRouteResult>(result, "Controller must redirect after successful change");
        }
    }
}
