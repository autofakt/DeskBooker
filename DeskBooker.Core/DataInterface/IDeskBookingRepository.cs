using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataInterface
{
    public interface IDeskBookingRepository
    {
        void Save(DeskBooking deskBooking)
        {
            //if (deskBooking != null)
            //    return "Booking Saved:";
            //else
            //    return null;
        }

    }
}
