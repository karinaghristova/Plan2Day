using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Plan2Day.Core.Contracts;
using Plan2Day.Core.Services;
using Plan2Day.Infrastructure.Data.DbModels.Exercises;
using Plan2Day.Infrastructure.Data.Identity;
using Plan2Day.Infrastructure.Data.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plan2DayTest
{
    public class ExerciseServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IExerciseService service;
        private IApplicationDbRepository repo;

        private Guid exercise1Guid = new Guid("D98BE384-0CDA-46EF-3E3B-08DA1CE5EC9F");
        private Guid exercise2Guid = new Guid("36C58319-20DA-43DB-3E3C-08DA1CE5EC9F");

        private Guid equipment1Guid = new Guid("3265C347-F6A0-4467-AB75-08DA20036629");
        private Guid equipment2Guid = new Guid("CCB41C7F-059A-421C-AB77-08DA20036629");

        private Guid level1Guid = new Guid("CE516766-F137-4238-6EAB-08DA2002DC17");
        private Guid level2Guid = new Guid("D8CEB9A8-65B2-4174-6EAD-08DA2002DC17");

        private Guid mechanicsType1Guid = new Guid("6A77D13C-C3F1-4677-8CDA-08DA2002ED65");
        private Guid mechanicsType2Guid = new Guid("1F7CCDDF-A2E9-4F7D-8CDB-08DA2002ED65");

        private Guid targetMuscle1Guid = new Guid("730B2CCB-3F4E-468A-9EFA-08DA2002F62C");
        private Guid targetMuscle2Guid = new Guid("A1924E93-C333-4A87-9EFF-08DA2002F62C");

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IExerciseService, ExerciseService>()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IApplicationDbRepository>();
            service = serviceProvider.GetService<IExerciseService>();
            await SeedDbAsync(repo);
        }

        [Test]
        public async Task GetAllExercisesMustReturnCorrectNumberOfExercises()
        {
            var allExercises = await service.GetAllExercises();

            Assert.AreEqual(allExercises.ToList().Count(), repo.All<Exercise>().Count());
        }

        [Test]
        public async Task GetTargetMuscleByIdAsyncReturnCorrectData()
        {
            var result = await service.GetTargetMuscleByIdAsync(targetMuscle1Guid.ToString());

            Assert.AreEqual("FirstTargetMuscle", result.Name);
        }

        [Test]
        public async Task GetTargetMuscleByIdAsyncThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetTargetMuscleByIdAsync(null), "Target muscle could not be found");
        }

        [Test]
        public async Task GetMechanicsTypeByIdAsyncReturnCorrectData()
        {
            var result = await service.GetMechanicsTypeByIdAsync(mechanicsType1Guid.ToString());

            Assert.AreEqual("FirstMechanicsType", result.Name);
        }

        [Test]
        public async Task GetMechanicsTypeByIdAsyncThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetMechanicsTypeByIdAsync(null), "Mechanics Type could not be found");
        }

        [Test]
        public async Task GetLevelByIdAsyncReturnCorrectData()
        {
            var result = await service.GetLevelByIdAsync(level1Guid.ToString());

            Assert.AreEqual("FirstLevel", result.Name);
        }

        [Test]
        public async Task GetLevelByIdAsyncThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetLevelByIdAsync(null), "Level could not be found");
        }


        [Test]
        public async Task GetExerciseByIdAsyncReturnCorrectData()
        {
            var result = await service.GetExerciseByIdAsync(exercise1Guid.ToString());

            Assert.AreEqual("Exercise1", result.Name);
        }

        [Test]
        public async Task GetExerciseByIdAsyncThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetExerciseByIdAsync(null), "Exercise could not be found");
        }

        [Test]
        public async Task GetEquipmentByIdAsyncReturnCorrectData()
        {
            var result = await service.GetEquipmentByIdAsync(equipment1Guid.ToString());

            Assert.AreEqual("FirstEquipment", result.Name);
        }

        [Test]
        public async Task GetEquipmentByIdAsyncThrowsErrorWithNull()
        {
            Assert.CatchAsync<ArgumentException>(async () => await service.GetEquipmentByIdAsync(null), "Equipment could not be found");
        }

        [Test]
        public async Task GetDetailsForExerciseReturnCorrectData()
        {
            var result = await service.GetDetailsForExercise("D98BE384-0CDA-46EF-3E3B-08DA1CE5EC9F");

            Assert.AreEqual("Exercise1", result.Name);
            Assert.AreEqual("FirstTargetMuscle", result.TargetMuscle);
            Assert.AreEqual("FirstEquipment", result.Equipment);
            Assert.AreEqual("FirstMechanicsType", result.MechanicsType);
            Assert.AreEqual("FirstLevel", result.Level);

        }

        [Test]
        public async Task GetDetailsForExerciseThrowsErrorWithNull()
        {
            Assert.CatchAsync<Exception>(async () => await service.GetDetailsForExercise(null), "Excerise could not be found");
        }


        [Test]
        public async Task GetAllTargetMusclesReturnCorrectNumber()
        {
            var targetMuscles = await service.GetAllTargetMuscles();

            Assert.AreEqual(targetMuscles.ToList().Count(), repo.All<TargetMuscle>().Count());
        }

        [Test]
        public async Task GetAllMechanicsTypesReturnCorrectNumber()
        {
            var mechTypes = await service.GetAllMechanicsTypes();

            Assert.AreEqual(mechTypes.ToList().Count(), repo.All<MechanicsType>().Count());
        }

        [Test]
        public async Task GetAllLevelsReturnCorrectNumber()
        {
            var levels = await service.GetAllLevels();

            Assert.AreEqual(levels.ToList().Count(), repo.All <Level>().Count());
        }

        [Test]
        public async Task GetAllEquipmentsReturnCorrectNumber()
        {
            var equipments = await service.GetAllEquipments();

            Assert.AreEqual(equipments.ToList().Count(), repo.All<Equipment>().Count());
        }

        [Test]
        public async Task CreateExerciseIncreasesNumberOfExercises()
        {
            var exBefore = await service.GetAllExercises();

            await service.CreateExercise("Exercise3", targetMuscle1Guid.ToString(),
                equipment1Guid.ToString(), mechanicsType2Guid.ToString(), level2Guid.ToString());

            var exAfter = await service.GetAllExercises();


            Assert.AreEqual(exBefore.ToList().Count(), exAfter.Count());
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

            var equipment1 = new Equipment()
            {
                Id = equipment1Guid,
                Name = "FirstEquipment"
            };

            await repo.AddAsync(equipment1);

            var equipment2 = new Equipment()
            {
                Id = equipment2Guid,
                Name = "SecondEquipment"
            };

            await repo.AddAsync(equipment2);

            var level1 = new Level()
            {
                Id = level1Guid,
                Name = "FirstLevel"
            };

            await repo.AddAsync(level1);

            var level2 = new Level()
            {
                Id = level2Guid,
                Name = "SecondLevel"
            };

            await repo.AddAsync(level2);

            var targetMuscle1 = new TargetMuscle()
            {
                Id = targetMuscle1Guid,
                Name = "FirstTargetMuscle"
            };

            await repo.AddAsync(targetMuscle1);

            var targetMuscle2 = new TargetMuscle()
            {
                Id = targetMuscle2Guid,
                Name = "SecondTargetMuscle"
            };

            await repo.AddAsync(targetMuscle2);

            var mechanicsType1 = new MechanicsType()
            {
                Id = mechanicsType1Guid,
                Name = "FirstMechanicsType"
            };

            await repo.AddAsync(mechanicsType1);

            var mechanicsType2 = new MechanicsType()
            {
                Id = mechanicsType2Guid,
                Name = "SecondMechanicsType"
            };

            await repo.AddAsync(mechanicsType2);

            var exercise1 = new Exercise()
            {
                Id = exercise1Guid,
                Name = "Exercise1",
                TargetMuscle = targetMuscle1,
                Equipment = equipment1,
                MechanicsType = mechanicsType1,
                Level = level1
            };

            await repo.AddAsync(exercise1);

            var exercise2 = new Exercise()
            {
                Id = exercise2Guid,
                Name = "Exercise2",
                TargetMuscle = targetMuscle2,
                Equipment = equipment2,
                MechanicsType = mechanicsType2,
                Level = level2
            };

            await repo.AddAsync(exercise2);

            await repo.SaveChangesAsync();
        }
    }
}
