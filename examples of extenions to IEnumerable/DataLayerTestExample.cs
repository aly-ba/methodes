using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Business.Bootstrapper;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Core.Common.Core;
using Core.Common.Contracts;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Data.RepositoryInterfaces;


namespace CarRenatalData.Test
{
    [TestClass]
    public class DataLayerTests
    {
        [TestInitialize]
        public  void Initialize()
        {
            ObjectBase.Container = MEFLoader.Init();
        }

        [TestMethod]
        public void test_repository_usage()
        {
            RepositoryTestClass repositoryTest = new RepositoryTestClass();

            IEnumerable<Car> cars = repositoryTest.GetCars();

            Assert.IsTrue(cars != null);
        }

        [TestMethod]
        public void test_repository_factory_usage()
         {
            RepositoryFactoryTestClass factoryTest = new RepositoryFactoryTestClass();

            IEnumerable<Car> cars = factoryTest.GetCars();

            Assert.IsTrue(cars == null);
        }


        [TestMethod]
        public void test_repository_mocking()
        {
            List<Car> cars = new List<Car>()
            {
                new Car(){ CarId=1, Description ="Mustang" },
                new Car { CarId=2, Description = "Corvet"}
            };
            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars);

            RepositoryTestClass repositoryTest = new RepositoryTestClass(mockCarRepository.Object);

            IEnumerable<Car> ret = repositoryTest.GetCars();

            Assert.IsTrue(ret ==cars);
        }

        [TestMethod]
        public void test_factory_mocking2()
        {
            List<Car> cars = new List<Car>()
            {
                new Car() { CarId = 1, Description = "Mustang" },
                new Car() { CarId = 2, Description = "Corvette" }
            };

            Mock<ICarRepository> mockCarRepository = new Mock<ICarRepository>();
            mockCarRepository.Setup(obj => obj.Get()).Returns(cars);

            Mock<IDataRepositoryFactory> mockDataRepository = new Mock<IDataRepositoryFactory>();
            mockDataRepository.Setup(obj => obj.GetDataRepository<ICarRepository>()).Returns(mockCarRepository.Object);

            RepositoryFactoryTestClass factoryTest = new RepositoryFactoryTestClass(mockDataRepository.Object);

            IEnumerable<Car> ret = factoryTest.GetCars();

            Assert.IsTrue(ret == cars);
        }
    }



    public class RepositoryTestClass
    {
        public RepositoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryTestClass(ICarRepository carRepository)
        {
            _CarRepository = carRepository;
        }

        [Import]
        ICarRepository _CarRepository;

        
        public IEnumerable<Car> GetCars() 
        {
            IEnumerable<Car> cars = _CarRepository.Get();

            return cars;
        }
    }


    public class RepositoryFactoryTestClass
    {
        public RepositoryFactoryTestClass()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public RepositoryFactoryTestClass(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public IEnumerable<Car> GetCars()
        {
            ICarRepository carRepository = _DataRepositoryFactory.GetDataRepository<ICarRepository>();

            IEnumerable<Car> cars = carRepository.Get();

            return cars;
        }
    }


}
