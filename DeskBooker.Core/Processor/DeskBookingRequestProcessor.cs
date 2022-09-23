using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;


namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private IDeskBookingRepository deskBookingRepository;
        private IDeskRepository deskRepository;

        public DeskBookingRequestProcessor(IDeskBookingRepository _deskBookingRepositoryMock, IDeskRepository _deskRepositoryMock)
        {
            deskBookingRepository = _deskBookingRepositoryMock;
            deskRepository = _deskRepositoryMock;

        }

        public void BindRequestToBooking(DeskBookingRequest request, DeskBookingBase booking)
        {
            booking.FirstName = request.FirstName;
            booking.LastName = request.LastName;
            booking.Email = request.Email;
            booking.Date = request.Date;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request != null)
            {


                DeskBookingResult result = new DeskBookingResult();
                BindRequestToBooking(request, result);

                DeskBooking deskbooking = new DeskBooking();
                BindRequestToBooking(request, deskbooking);

                if (deskRepository.GetAvailableDesks(request.Date).Count() > 0)
                {
                    deskBookingRepository.Save(deskbooking);
                }

                return result;


            }
            else
            {
                throw new ArgumentNullException(nameof(request));
            }

        }
    }
}