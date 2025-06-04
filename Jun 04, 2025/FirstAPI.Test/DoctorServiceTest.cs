using System;
using FirstAPI.Contexts;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Services;
using FirstAPI.Misc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using FirstAPI.Repositories;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using AutoMapper;
using Moq;

namespace FirstAPI.Test
{
    public class DoctorServiceTest
    {
        public ClinicContext _context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            _context = new ClinicContext(options);
        }
        [TestCase("General")]
        public async Task TestGetDoctorBySpeciality(string speciality)
        {
            Mock<DoctorRepository> doctorRepositoryMock = new Mock<DoctorRepository>(_context);
            Mock<SpecialityRepository> specialityRepositoryMock = new(_context);
            Mock<DoctorSpecialityRepository> doctorSpecialityRepositoryMock = new(_context);
            Mock<UserRepository> userRepositoryMock = new(_context);
            Mock<IOtherContextFunctionities> otherContextFunctionitiesMock = new();
            Mock<EncryptionService> encryptionServiceMock = new();
            Mock<IMapper> mapperMock = new();

            otherContextFunctionitiesMock.Setup(ocf => ocf.GetDoctorsBySpeciality(It.IsAny<string>()))
                                        .ReturnsAsync((string specilaity) => new List<DoctorsBySpecialityResponseDto>{
                                    new DoctorsBySpecialityResponseDto
                                            {
                                                Dname = "test",
                                                yoe = 2,
                                                Id=1
                                            }
                                });
            IDoctorService doctorService = new DoctorService(doctorRepositoryMock.Object,
                                                            specialityRepositoryMock.Object,
                                                            doctorSpecialityRepositoryMock.Object,
                                                            userRepositoryMock.Object,
                                                            otherContextFunctionitiesMock.Object,
                                                            encryptionServiceMock.Object,
                                                            mapperMock.Object);

            //Action
            var result = await doctorService.GetDoctorsBySpeciality(speciality);
            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }
        [TestCase("Exception")]
        public void TestGetDoctorBySpecialityException(string speciality)
        {
            Mock<DoctorRepository> doctorRepositoryMock = new Mock<DoctorRepository>(_context);
            Mock<SpecialityRepository> specialityRepositoryMock = new(_context);
            Mock<DoctorSpecialityRepository> doctorSpecialityRepositoryMock = new(_context);
            Mock<UserRepository> userRepositoryMock = new(_context);
            Mock<IOtherContextFunctionities> otherContextFunctionitiesMock = new();
            Mock<EncryptionService> encryptionServiceMock = new();
            Mock<IMapper> mapperMock = new();

            otherContextFunctionitiesMock.Setup(ocf => ocf.GetDoctorsBySpeciality(It.IsAny<string>()))
                                        .Throws(new Exception("Error fetching doctors"));

            IDoctorService doctorService = new DoctorService(doctorRepositoryMock.Object,
                                                            specialityRepositoryMock.Object,
                                                            doctorSpecialityRepositoryMock.Object,
                                                            userRepositoryMock.Object,
                                                            otherContextFunctionitiesMock.Object,
                                                            encryptionServiceMock.Object,
                                                            mapperMock.Object);

            //Action & Assert
            Assert.ThrowsAsync<Exception>(async () => await doctorService.GetDoctorsBySpeciality(speciality));
        }

        [TestCase("General")]

        public async Task CancelAppointmentTest(string speciality)
        {
            Mock<DoctorRepository> doctorRepositoryMock = new Mock<DoctorRepository>(_context);
            Mock<SpecialityRepository> specialityRepositoryMock = new(_context);
            Mock<DoctorSpecialityRepository> doctorSpecialityRepositoryMock = new(_context);
            Mock<UserRepository> userRepositoryMock = new(_context);
            Mock<IOtherContextFunctionities> otherContextFunctionitiesMock = new();
            Mock<EncryptionService> encryptionServiceMock = new();
            Mock<IMapper> mapperMock = new();

            otherContextFunctionitiesMock.Setup(ocf => ocf.CancelAppointment(It.IsAny<int>()))
                                        .ReturnsAsync(true);

            IDoctorService doctorService = new DoctorService(doctorRepositoryMock.Object,
                                                            specialityRepositoryMock.Object,
                                                            doctorSpecialityRepositoryMock.Object,
                                                            userRepositoryMock.Object,
                                                            otherContextFunctionitiesMock.Object,
                                                            encryptionServiceMock.Object,
                                                            mapperMock.Object);

            //Action
            var result = await doctorService.CancelAppointment(1);
            //Assert
            Assert.IsTrue(result);
        }
        [TestCase("Exception")]
        public void CancelAppointmentExceptionTest(string speciality)
        {
            Mock<DoctorRepository> doctorRepositoryMock = new Mock<DoctorRepository>(_context);
            Mock<SpecialityRepository> specialityRepositoryMock = new(_context);
            Mock<DoctorSpecialityRepository> doctorSpecialityRepositoryMock = new(_context);
            Mock<UserRepository> userRepositoryMock = new(_context);
            Mock<IOtherContextFunctionities> otherContextFunctionitiesMock = new();
            Mock<EncryptionService> encryptionServiceMock = new();
            Mock<IMapper> mapperMock = new();

            otherContextFunctionitiesMock.Setup(ocf => ocf.CancelAppointment(It.IsAny<int>()))
                                        .Throws(new Exception("Error cancelling appointment"));

            IDoctorService doctorService = new DoctorService(doctorRepositoryMock.Object,
                                                            specialityRepositoryMock.Object,
                                                            doctorSpecialityRepositoryMock.Object,
                                                            userRepositoryMock.Object,
                                                            otherContextFunctionitiesMock.Object,
                                                            encryptionServiceMock.Object,
                                                            mapperMock.Object);

            //Action & Assert
            Assert.ThrowsAsync<Exception>(async () => await doctorService.CancelAppointment(1));
        }
        
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}