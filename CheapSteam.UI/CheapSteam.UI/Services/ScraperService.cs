namespace CheapSteam.UI.Services
{
    public class ScraperService
    {
        private ChpStmScraper.Program scriper;
        public ScraperService()
        {
            scriper = new ChpStmScraper.Program();
        }

       public void Start()
        {
            scriper.Start();
        }
    }
}
