using Moq;
using SeaSharpe_CVGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SeaSharpe_CVGS.Tests
{
    class MockHelpers
    {

        /// <summary>
        /// This helper gives a context that can be assigned to a controller to make it so that 
        /// controller things that this user is logged in.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static ControllerContext GetControllerContext(ApplicationDbContext db, Member member, params string[] roles)
        {
            var userMock = new Mock<IPrincipal>();

            // Return true for "member" and "Member" roles
            foreach (string role in roles)
            {
                userMock.Setup(p => p.IsInRole(role)).Returns(true);
            }

            // Return first username
            userMock.Setup(p => p.Identity.Name).Returns(member.User.UserName);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            return controllerContextMock.Object;
        }

        /// <summary>
        /// This helper gives a context that can be assigned to a controller to make it so that 
        /// controller things that this user is logged in.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static ControllerContext GetControllerContext(ApplicationDbContext db, Employee employee, params string[] roles)
        {
            var userMock = new Mock<IPrincipal>();

            // Return true for "member" and "Member" roles
            foreach (string role in roles)
            {
                userMock.Setup(p => p.IsInRole(role)).Returns(true);
            }

            // Return first username
            userMock.Setup(p => p.Identity.Name).Returns(employee.User.UserName);

            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(ctx => ctx.User)
                       .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(con => con.HttpContext)
                                 .Returns(contextMock.Object);

            return controllerContextMock.Object;
        }
    }
}
