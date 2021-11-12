using Space.ImdbWatchList.Dto;

namespace Space.ImdbWatchList.Infrastructure
{
    public interface IEmailService
    {
        void SendEmail(OfferDataDto offerDataDto);
    }
}