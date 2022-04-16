using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Services;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Infrastructure.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Plan2Day.Core.Models;


namespace Plan2DayTest
{
    public class UserServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IUserService userService;
        private IApplicationDbRepository repo;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicationDbRepository>();
            userService = serviceProvider.GetService<IUserService>();
            await SeedDbAsync(repo);
        }

        [Test]
        public async Task GetUserByIdMustReturnCorrectUser()
        {
            //var service = serviceProvider.GetService<UserService>();

            var user = await userService.GetUserById("abcde");

            Assert.AreEqual(user.FirstName, "Pesho");
            Assert.AreEqual(user.LastName, "Ivanov");
        }

        [Test]
        public async Task GetUsersMustReturnCorrectNumberOfUsers()
        {
            //var service = serviceProvider.GetService<UserService>();

            var users = await userService.GetUsers();

            Assert.AreEqual(users.ToList().Count, repo.All<ApplicationUser>().Count());
        }

        [Test]
        public async Task EditUserMustReturnCorrectData()
        {
            //var service = serviceProvider.GetService<UserService>();
            var userModel = await userService.EditUser("abcde");

            await userService.UpdateUser(userModel);

            Assert.AreEqual("Pesho", userModel.FirstName);
            Assert.AreEqual("Ivanov", userModel.LastName);
        }

        [Test]
        public async Task UpdateUserMustChangeUserDataCorrectly()
        {
            //var service = serviceProvider.GetService<UserService>();
            var userModel = new UserEditViewModel()
            {
                Id = "abcde",
                FirstName = "Pesho",
                LastName = "Peshev"
            };

            await userService.UpdateUser(userModel);
            var user = await userService.GetUserById("abcde");

            Assert.AreEqual("Pesho", user.FirstName);
            Assert.AreEqual("Peshev", user.LastName);
        }

        [Test]
        public async Task UsersCountDecreaswsAfterDelete()
        {
            //var service = serviceProvider.GetService<UserService>();
            var usersBefore = await userService.GetUsers();

            var result = await userService.DeleteUser("madfkl"); 
            var usersAfter = await userService.GetUsers();

            Assert.AreEqual(usersBefore.ToList().Count, usersAfter.ToList().Count() + 1);
        }

        [TearDown]
        public void TearDown()
        {
            //Do not forget to dispose the dbContext
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var applicationUser = new ApplicationUser()
            {
                Id = "abcde",
                Email = "mytestuser@abv.bg",
                FirstName = "Pesho",
                LastName = "Ivanov",
            };

            await repo.AddAsync(applicationUser);

            var applicationUser2 = new ApplicationUser()
            {
                Id = "efghi",
                Email = "mysecondtestuser@abv.bg",
                FirstName = "Ivan",
                LastName = "Petrov",
            };

            await repo.AddAsync(applicationUser2);

            var applicationUser3 = new ApplicationUser()
            {
                Id = "madfkl",
                Email = "mysecondtestuser@abv.bg",
                FirstName = "Third",
                LastName = "User",
            };

            await repo.AddAsync(applicationUser3);

            await repo.SaveChangesAsync();
        }
    }
}