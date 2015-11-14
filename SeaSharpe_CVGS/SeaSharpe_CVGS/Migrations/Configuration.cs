namespace SeaSharpe_CVGS.Migrations
{
    using SeaSharpe_CVGS.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SeaSharpe_CVGS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Log some text to %USERPROFILE%\seedlog.txt
        /// </summary>
        /// <param name="value">The text to log.</param>
        public void Log(string value)
        {
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\seedlog.txt", true);
            file.WriteLine(value);

            file.Close();
        }

        protected override void Seed(SeaSharpe_CVGS.Models.ApplicationDbContext context)
        {
            var store = new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(store);
            Member[] members = {new Member
                {
                    IsEmailMarketingAllowed = false,
                    IsEmailVerified = true,
                    User = new ApplicationUser
                    {
                        UserName = "Greg",
                        FirstName = "Greg",
                        LastName = "Greg",
                        Email = "Greg@Greg.Greg",
                        Gender = "G", // TODO: Fix gender constraints so this fails
                        DateOfBirth = System.DateTime.Now,
                        DateOfRegistration = System.DateTime.Now
                    }
                },new Member
                {
                    IsEmailMarketingAllowed = true,
                    IsEmailVerified = false,
                    User = new ApplicationUser
                    {
                        UserName = "Tim",
                        FirstName = "Tim",
                        LastName = "Tim",
                        Email = "Tim@Tim.Tim",
                        Gender = "T",
                        DateOfBirth = System.DateTime.Now,
                        DateOfRegistration = System.DateTime.Now 
                    }
                }};

            foreach (var member in members)
            {
                userManager.CreateAsync(member.User, "thisP@ssw0rdIsAmazing").Wait();
                context.Members.AddOrUpdate(member);
            }

            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {

                Log("DB Validation Exception");
                foreach (var a in e.EntityValidationErrors)
                {
                    foreach (var b in a.ValidationErrors)
                    {
                        Log(String.Format("{0} - {1}", b.ErrorMessage, a.Entry.Entity.GetType()));
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Launch();
                Log("Exception");
                Log(e.Message);
            }
        }
    }
}

