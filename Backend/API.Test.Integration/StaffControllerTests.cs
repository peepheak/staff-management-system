using System.Net;
using API.Constants;
using API.Context;
using API.Controller;
using API.Enum;
using API.Interfaces;
using API.Mapping;
using API.Request;
using API.Response;
using API.Wrapper;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Test.Integration;

public class StaffControllerTests
{
    [Fact]
    public async Task AddStaffAsync_ReturnsOk_WhenStaffAddedSuccessfully()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "STAFF_DB")
            .Options;

        // Arrange
        var request = new StaffAddRequest()
        {
            FullName = "John Doe",
            StaffId = "12345",
            Gender = Gender.Male,
            Birthday = new DateOnly(2000, 1, 1)
        };

        var id = "id";
        var createdStaff = new StaffResponse
        {
            Id = id,
            FullName = request.FullName,
            StaffId = request.StaffId,
            Gender = request.Gender,
            Birthday = request.Birthday,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        using (var context = new AppDbContext(options))
        {
            var mockService = new Mock<IStaffService>();
            mockService.Setup(s => s.AddAsync(request, CancellationToken.None))
                .ReturnsAsync(await Result<string>.SuccessAsync(id, ApplicationConstants.Message.Saved,
                    HttpStatusCode.OK));

            mockService.Setup(s => s.GetByIdAsync(id, CancellationToken.None))
                .ReturnsAsync(
                    await Result<StaffResponse>.SuccessAsync(createdStaff, ApplicationConstants.Message.Recieved,
                        HttpStatusCode.OK));

            var mapperConfig =
                new MapperConfiguration(cfg => { cfg.AddProfile(new StaffProfile()); }, new LoggerFactory());
            var mapper = mapperConfig.CreateMapper();

            var controller = new StaffController(mockService.Object);

            var result = await controller.AddAsync(request, CancellationToken.None);
            var data = await controller.GetByIdAsync(id, CancellationToken.None);
            
            var dataValue = Assert.IsType<Result<StaffResponse>>(((OkObjectResult)data).Value);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(data);
            Assert.True(dataValue.IsSuccess);
        }
    }


    [Fact]
    public async Task UpdateStaffAsync_ReturnsOk_WhenStaffEditedSuccessfully()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Staff")
            .Options;

        // Arrange
        var request = new StaffUpdateRequest
        {
            Id = "id",
            FullName = "John Doe",
            StaffId = "12345",
            Gender = Gender.Male,
            Birthday = new DateOnly(2000, 1, 1)
        };

        var id = "id";
        var createdStaff = new StaffResponse
        {
            Id = id,
            FullName = request.FullName,
            StaffId = request.StaffId,
            Gender = request.Gender,
            Birthday = request.Birthday,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        using (var context = new AppDbContext(options))
        {
            var mockService = new Mock<IStaffService>();
            mockService.Setup(s => s.UpdateAsync(request, CancellationToken.None))
                .ReturnsAsync(await Result<string>.SuccessAsync(id, ApplicationConstants.Message.Saved,
                    HttpStatusCode.OK));

            mockService.Setup(s => s.GetByIdAsync(id, CancellationToken.None))
                .ReturnsAsync(
                    await Result<StaffResponse>.SuccessAsync(createdStaff, ApplicationConstants.Message.Recieved,
                        HttpStatusCode.OK));

            var mapperConfig =
                new MapperConfiguration(cfg => { cfg.AddProfile(new StaffProfile()); }, new LoggerFactory());
            var mapper = mapperConfig.CreateMapper();

            var controller = new StaffController(mockService.Object);

            var data = await controller.GetByIdAsync(id, CancellationToken.None);

            var dataValue = Assert.IsType<Result<StaffResponse>>(((OkObjectResult)data).Value);
            Assert.IsType<OkObjectResult>(data);
            Assert.True(dataValue.IsSuccess);
        }
    }
}