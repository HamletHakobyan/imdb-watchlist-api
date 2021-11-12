namespace Space.ImdbWatchList.RecurringJobs
{
    public class EmailSenderSettings
    {
        public const string SectionName = "EmailSender";
        public string Host { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Password { get; set; }
    }
}