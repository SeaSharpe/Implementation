namespace SeaSharpe_CVGS.Migrations
{
    using CsvHelper.Configuration;
    using SeaSharpe_CVGS.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
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
            System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\seedlog.txt", true);
            file.WriteLine(value);

            file.Close();
        }

        private void Log(string p, params object[] args)
        {
            Log(string.Format(p, args));
        }

        protected override void Seed(SeaSharpe_CVGS.Models.ApplicationDbContext context)
        {
            var store = new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(store);

            int i = 0;

            try
            {

                foreach (var person in new MockData().People)
                {
                    i++;
                    try
                    {
                        Member member = new Member
                        {
                            IsEmailMarketingAllowed = false,
                            IsEmailVerified = false,
                            User = new ApplicationUser
                            {
                                UserName = person.GivenName + person.Surname + person.Sin.Substring(3, 6),
                                FirstName = person.GivenName,
                                LastName = person.Surname,
                                Email = person.Email,
                                Gender = "O",
                                DateOfBirth = System.DateTime.Now,
                                DateOfRegistration = System.DateTime.Now
                            }
                        };
                        userManager.CreateAsync(member.User, "thisP@ssw0rdIsAmazing").Wait();
                        context.Members.AddOrUpdate(member);
                    }
                    catch (DbEntityValidationException e)
                    {
                        Log("Inner Validation Exception");
                        foreach (var a in e.EntityValidationErrors)
                        {
                            foreach (var b in a.ValidationErrors)
                            {
                                Log("{0} - {1}", b.ErrorMessage, a.Entry.Entity.GetType());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debugger.Launch();
                        Log("Inner Exception");
                        Log(e.Message);
                    }
                }

                Log("Wrote {0} things", i);

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                Log("Outer Validation Exception");
                foreach (var a in e.EntityValidationErrors)
                {
                    foreach (var b in a.ValidationErrors)
                    {
                        Log("{0} - {1}", b.ErrorMessage, a.Entry.Entity.GetType());
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Launch();
                Log("Outer Exception");
                Log(e.Message);
            }
        }
    }
}

