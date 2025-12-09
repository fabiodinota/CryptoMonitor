using CryptoMonitor.Domain;

namespace UI_MVC.Models
{
    public class DashboardViewModel
    {
        public int TotalCryptos { get; set; }
        public int TotalExchanges { get; set; }
        public int TotalReviews { get; set; }
        public IEnumerable<Cryptocurrency> RecentCryptos { get; set; } = new List<Cryptocurrency>();
        public IEnumerable<UserReview> RecentReviews { get; set; } = new List<UserReview>();
    }
}
