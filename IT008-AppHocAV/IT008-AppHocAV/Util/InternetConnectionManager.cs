using System;
using System.Threading.Tasks;

namespace IT008_AppHocAV.Util
{
    public class InternetConnectionManager
    {
        public event Action<bool> InternetConnectionChanged;

        public bool IsInternetAvailable { get; private set; }

        public async void CheckInternetConnection()
        {
            while (true)
            {
                bool PreStatus = IsInternetAvailable;
                await Task.Delay(5000);
                IsInternetAvailable = Util.InternetAvailability.IsInternetAvailable();
                if (PreStatus != IsInternetAvailable)
                {
                    InternetConnectionChanged?.Invoke(IsInternetAvailable);
                }
            }
        }
    }
}