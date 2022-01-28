using ChpStmScraper;

namespace CheapSteam.UI.Services
{
    public class ScraperService
    {
        private ChpStmScraper.Program scriper;
        public static object ContextLock = new object();
        public Queue<string> Logs { get; private set; }
        public ScraperService(ScraperDbContext dbContext)
        {
            Logs = new Queue<string>();
            scriper = new ChpStmScraper.Program(Logs, dbContext, ContextLock);
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
