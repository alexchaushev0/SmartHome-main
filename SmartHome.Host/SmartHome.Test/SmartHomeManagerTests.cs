using SmartHome.BL.Interfaces;
using SmartHome.Models;
using Moq;
using Xunit;

namespace SmartHome.Test
{
    public class SmartHomeManagerTests
    {
        private Mock<IRoomService> _roomServiceMock;
        private Mock<IDeviceService> _deviceServiceMock;

        [Fact]
        public async Task GetRoomStatus_Return_Ok()
        {
            // Arrange
            _roomServiceMock = new Mock<IRoomService>();
            _deviceServiceMock = new Mock<IDeviceService>();

            var roomId = Guid.NewGuid().ToString();

            
            _roomServiceMock.Setup(x => x.GetById(roomId)).ReturnsAsync(new Room
            {
                Id = roomId,
                Name = "Living Room",
                Temperature = 22
            });

            _deviceServiceMock.Setup(x => x.GetActiveDevicesCount(roomId)).ReturnsAsync(5);

            var manager = new SmartHome.BL.Services.SmartHomeManager(
                _roomServiceMock.Object,
                _deviceServiceMock.Object);

            // Act
            var result = await manager.GetRoomStatus(roomId);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Living Room", result);
        }

        [Fact]
        public async Task GetRoomStatus_When_Room_Missing_Throws()
        {
            // Arrange
            _roomServiceMock = new Mock<IRoomService>();
            _deviceServiceMock = new Mock<IDeviceService>();

            var roomId = Guid.NewGuid().ToString();

            
            _roomServiceMock.Setup(x => x.GetById(roomId)).ReturnsAsync((Room)null);

            var manager = new SmartHome.BL.Services.SmartHomeManager(
                _roomServiceMock.Object,
                _deviceServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => manager.GetRoomStatus(roomId));
        }
    }
}