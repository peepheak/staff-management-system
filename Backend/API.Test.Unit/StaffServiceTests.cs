using API.Constants;
using API.Context;
using API.Entities;
using API.Enum;
using API.Request;
using API.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace API.Test.Unit;

public class StaffServiceTests
{
    [Fact]
    public async Task AddStaffAsync_ReturnSuccess_WhenStaffDoesNotExistAndValidAddRequests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestAddStaff")
            .Options;

        var addStaffRequest = new StaffAddRequest()
        {
            FullName = "Joc",
            StaffId = "12345543",
            Gender = Gender.Female,
            Birthday = new DateOnly(2000, 1, 1)
        };

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<Staff>(It.IsAny<StaffAddRequest>()))
            .Returns(new Staff
            {
                Id = Guid.NewGuid().ToString(),
                FullName = addStaffRequest.FullName,
                StaffId = addStaffRequest.StaffId,
                Gender = addStaffRequest.Gender,
                Birthday = addStaffRequest.Birthday
            });

        using (var context = new AppDbContext(options))
        {
            var staffService = new StaffService(context, mockMapper.Object);

            var result = await staffService.AddAsync(addStaffRequest, CancellationToken.None);
            await context.SaveChangesAsync();

            Assert.True(result.IsSuccess);

            var existingStaff = await context.Staff.FirstOrDefaultAsync(s => s.StaffId == addStaffRequest.StaffId);
            Assert.NotNull(existingStaff);
        }
    }

    [Fact]
    public async Task AddStaffAsync_ReturnFail_WhenStaffExists()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestAddStaffExists")
            .Options;

        var addStaffRequest = new StaffAddRequest()
        {
            FullName = "Joc",
            StaffId = "existingStaffId",
            Gender = Gender.Female,
            Birthday = new DateOnly(2000, 1, 1)
        };

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                Id = Guid.NewGuid().ToString(),
                FullName = addStaffRequest.FullName,
                StaffId = addStaffRequest.StaffId,
                Gender = addStaffRequest.Gender,
                Birthday = addStaffRequest.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);

            var result = await staffService.AddAsync(addStaffRequest, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal(ApplicationConstants.Message.Exists, result.Message);
        }
    }

    // [Fact]
    // public async Task AddStaffAsync_ReturnFail_WhenBirthdateIsInFuture()
    // {
    //     var options = new DbContextOptionsBuilder<AppDbContext>()
    //         .UseInMemoryDatabase(databaseName: "TestBirthdateIsInFuture")
    //         .Options;
    //
    //     var addStaffRequest = new StaffAddRequest()
    //     {
    //         FullName = "Joc",
    //         StaffId = "futureBirthStaffId",
    //         Gender = Gender.Female,
    //         Birthday = DateOnly.FromDateTime(DateTime.Now.AddYears(1)) // Future date
    //     };
    //
    //     var mockMapper = new Mock<IMapper>();
    //     mockMapper.Setup(m => m.Map<Staff>(It.IsAny<StaffAddRequest>()))
    //         .Returns(new Staff
    //         {
    //             Id = Guid.NewGuid().ToString(),
    //             FullName = addStaffRequest.FullName,
    //             StaffId = addStaffRequest.StaffId,
    //             Gender = addStaffRequest.Gender,
    //             Birthday = addStaffRequest.Birthday
    //         });
    //
    //     using (var context = new AppDbContext(options))
    //     {
    //         var staffService = new StaffService(context, mockMapper.Object);
    //
    //         var result = await staffService.AddAsync(addStaffRequest, CancellationToken.None);
    //
    //         Assert.False(result.IsSuccess);
    //         Assert.Equal(ApplicationConstants.Message.birthdayValidate, result.Message);
    //     }
    // }

    [Fact]
    public async Task UpdateStaffAsync_ReturnSuccess_WhenUpdatedStaffAndValidRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "EditStaff")
            .Options;

        var request = new StaffUpdateRequest()
        {
            Id = Guid.NewGuid().ToString(),
            FullName = "Joc Updated",
            StaffId = "oldStaffId",
            Gender = Gender.Female,
            Birthday = new DateOnly(2000, 1, 1)
        };

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                Id = request.Id,
                FullName = "Joc",
                StaffId = request.StaffId,
                Gender = request.Gender,
                Birthday = request.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);

            var result = await staffService.UpdateAsync(request, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApplicationConstants.Message.Updated, result.Message);
        }
    }

    [Fact]
    public async Task DeleteStaffAsync_ReturnTrue_WhenDeletedStaff()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "DeleteStaff")
            .Options;

        var staffId = Guid.NewGuid().ToString();

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                Id = staffId,
                FullName = "Joc",
                StaffId = "staffToDelete",
                Gender = Gender.Female,
                Birthday = new DateOnly(2000, 1, 1)
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);

            var result = await staffService.DeleteAsync(staffId, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApplicationConstants.Message.Deleted, result.Message);
        }
    }

    [Fact]
    public async Task GetStaffsQuery_ReturnTrue_WhenStaffExistOrNotExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestGetStaffs")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);

            var result = await staffService.GetAllAsync(null, null, null, null, 1, 10, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApplicationConstants.Message.Recieved, result.Message);
        }
    }

    [Fact]
    public async Task GetStaffQuery_ReturnTrue_WhenStaffExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestGetStaff")
            .Options;

        var newId = Guid.NewGuid().ToString();

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                Id = newId,
                FullName = "Joc",
                StaffId = "newStaffId",
                Gender = Gender.Female,
                Birthday = new DateOnly(2000, 1, 1)
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);

            var result = await staffService.GetByIdAsync(newId, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(ApplicationConstants.Message.Recieved, result.Message);
        }
    }
}
