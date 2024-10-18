using CombatCrittersSharp;

namespace Combat_Critters_2._0.Services
{
    public class ClientSingleton
    {
        private static IClient? _instance;
        private static readonly object _lock = new object();
        private static string? _apiUri;

        private ClientSingleton(){}

        public static IClient GetInstance(string apiUri)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        //Save for future reference
                        _apiUri = apiUri;

                        //intialize the client instance 
                        _instance = new Client(_apiUri);
                    }
                }
            }
            return _instance;
        }
    }
}