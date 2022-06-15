using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PSW;
using PSW.DAL;
using PSW.Model;

namespace PSWIntegrationTests
{
    public class IntegrationTestsFactory<TStartup> : WebApplicationFactory<Startup>
    {
        private readonly string _dbName = $"testDb{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<PSWStoreContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<PSWStoreContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<PSWStoreContext>();

                    try
                    {
                        context.Database.EnsureCreated();

                        SeedData(context);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            });
        }

        private void SeedData(PSWStoreContext context)
        {
            context.Database.EnsureDeleted();

            var admin = new User()
            {
                Id = "1",
                FirstName = "firstname1",
                LastName = "lastname1",
                Username = "username1",
                Password = "password1",
                Role = User.UserRole.Admin.ToString(),
                DateOfBirth = "birthday1",
                Address = "address1",
                Country = "countr1",
                PhoneNumber = "phone1",
                Specialization = "None",
                IsBlocked = false
            };

            var doctor = new User()
            {
                Id = "2",
                FirstName = "firstname2",
                LastName = "lastname2",
                Username = "username2",
                Password = "password2",
                Role = User.UserRole.Doctor.ToString(),
                DateOfBirth = "birthday2",
                Address = "address2",
                Country = "countr2",
                PhoneNumber = "phone2",
                Specialization = "Oncology",
                IsBlocked = false
            };

            var patient = new User()
            {
                Id = "3",
                FirstName = "firstname3",
                LastName = "lastname3",
                Username = "username3",
                Password = "password3",
                Role = User.UserRole.Client.ToString(),
                DateOfBirth = "birthday3",
                Address = "address3",
                Country = "countr3",
                PhoneNumber = "phone3",
                Specialization = "none",
                IsBlocked = false
            };

            var reservedTerm = new Term()
            {
                Id = "1",
                UserId = "3",
                DoctorId = "2",
                DateTimeTerm = "6/15/2022",
                IsRejected = false
            };

            var rejectedTerm = new Term()
            {
                Id = "2",
                UserId = "3",
                DoctorId = "2",
                DateTimeTerm = "6/15/2022",
                IsRejected = true
            };

            var medicine = new Medicine()
            {
                Id = "1",
                Dose = 100,
                Name = "Brufen",
                Quantity = 5
            };

            var privateFeedback = new Feedback()
            {
                Id = "1",
                Content = "Feedback1",
                Grade = 4,
                IsPrivate = true,
                IsVisible = true,
                UserId = "2"
            };

            var publicFeedback = new Feedback()
            {
                Id = "2",
                Content = "Feedback2",
                Grade = 3,
                IsPrivate = false,
                IsVisible = true,
                UserId = "2"
            };

            var comment = new Comment()
            {
                Id = "1",
                Content = "Comment1",
                Grade = 5,
                TermId = "1",
                UserId = "3"
            };



            context.User.Add(admin);
            context.User.Add(doctor);
            context.User.Add(patient);
            context.Term.Add(reservedTerm);
            context.Term.Add(rejectedTerm);
            context.Medicine.Add(medicine);
            context.Feedback.Add(privateFeedback);
            context.Feedback.Add(publicFeedback);
            context.Comment.Add(comment);
        }
    }
}
