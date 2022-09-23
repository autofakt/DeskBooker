using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using Moq;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private readonly DeskBookingRequestProcessor _processor;
        private readonly DeskBookingRequest _request;
        private readonly List<Desk> _availableDesks;
        private readonly Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private readonly Mock<IDeskRepository> _deskRepositoryMock;


        public DeskBookingRequestProcessorTests()
        {

            _request = new DeskBookingRequest
            {
                FirstName = "MThomas",
                LastName = "Huber",
                Email = "thomas@thomasclaudiushuber.com",
                Date = new DateTime(2020, 1, 28)
            };

            _availableDesks = new List<Desk> { new Desk() };

            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _deskRepositoryMock = new Mock<IDeskRepository>();
            _deskRepositoryMock.Setup(x => x.GetAvailableDesks(_request.Date)).Returns(_availableDesks);
            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object, _deskRepositoryMock.Object);
        }

        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            //arrange
            

            //act
            DeskBookingResult result = _processor.BookDesk(_request);

            //assert
            Assert.NotNull(_request);
            Assert.Equal(result.FirstName, _request.FirstName);
            Assert.Equal(result.LastName, _request.LastName);
            Assert.Equal(result.Email, _request.Email);
            Assert.Equal(result.Date, _request.Date);
        }

        [Fact]
        public void ShouldReturnNullException_Pass()
        {
            //arrange

            //act

            //assert

            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));
            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldReturnNullException_Fail()
        {
            //arrange

            //act
            var exception = Record.Exception(() => _processor.BookDesk(_request));

            //assert
            Assert.Null(exception);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            //arrange
            DeskBooking savedDeskBoooking = null;
            //DeskBooking sample = new DeskBooking()
            //{
            //    FirstName = "Mike",
            //    LastName = "Peters",
            //    Email = "mike@aol.com",
            //    Date = DateTime.Now
            //};
            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>())).Callback<DeskBooking>(x =>
            {
                savedDeskBoooking = x;
            });
            //_deskBookingRepositoryMock.Setup(x => x.Save(sample)).Callback<DeskBooking>(deskBooking =>
            //{
            //    savedDeskBoooking = deskBooking;
            //});

            //act
            _processor.BookDesk(_request);
            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);

            string my = savedDeskBoooking.FirstName;
            Console.WriteLine(my);

            //assert
            Assert.NotNull(savedDeskBoooking);
            Assert.Equal(_request.FirstName, savedDeskBoooking.FirstName);
            Assert.Equal(_request.LastName, savedDeskBoooking.LastName);
            Assert.Equal(_request.Email, savedDeskBoooking.Email);
            Assert.Equal(_request.Date, savedDeskBoooking.Date);
        }

        [Fact]
        public void No_DeskAvailableDeskBooking()
        {
            //arrange
            _availableDesks.Clear();
          
        
            //act
            _processor.BookDesk(_request);

            //_deskRepositoryMock.Verify(x => x.DeskAvailable(), Times.Once);
            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);

            //assert
           // Assert.True(deskAvailable.Count > 0);
        }
    }
}
