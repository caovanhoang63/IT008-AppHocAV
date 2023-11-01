using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Util
{
    public class CheckValidate
    {
        
        public static bool IsEmail(string email)
        {
            var emailRegrex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (emailRegrex.IsMatch(email))
                return true;
            return false;
        }

        public static bool IsPhoneNumber(string phoneNumber)
        {
            var phoneNumberRegrex = new Regex(@"(84|0[3|5|7|8|9])+([0-9]{8})\b");
            if (phoneNumberRegrex.IsMatch(phoneNumber))
                return true;
            return false;
        }

        public static bool CheckMinLength(string str, int min)
        {
            if (str.Length < min)
                return false;
            return true;
        }

        public static bool IsValidPassword(string password)
        {
            var passwordRegrex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
            if (passwordRegrex.IsMatch(password))
                return true;
            return false;
        }


        public static bool IsValidUserName(string userName)
        {
            var userNameRegrex = new Regex(@"^(?=[a-zA-Z0-9._]{8,20}$)(?!.*[_.]{2})[^_.].*[^_.]$");
            if (userNameRegrex.IsMatch(userName))
                return true;
            return false;
        }
    } 
}