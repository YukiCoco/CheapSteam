namespace CheapSteam.UI.Services
{
    public class ScraperService
    {
        private ChpStmScraper.Program scriper;
        public Queue<string> Logs { get; private set; }
        public ScraperService()
        {
            Logs = new Queue<string>();
            scriper = new ChpStmScraper.Program(Logs);
        }

       public void Start()
        {
            scriper.Start();
        }

        public void Stop()
        {
            scriper.Stop();
        }
    }
}
