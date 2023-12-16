using System.Runtime.InteropServices;

namespace IT008_AppHocAV.Util
{
    public class InternetAvailability
    {
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);
 
        public static bool IsInternetAvailable( )
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
    }
}