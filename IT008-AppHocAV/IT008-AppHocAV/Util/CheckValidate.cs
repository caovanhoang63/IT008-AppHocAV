using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Util
{
    public class CheckValidate
    {
        /// <summary>
        /// Checks if a string is in email format
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            var emailRegrex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (emailRegrex.IsMatch(email))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if a string is in vietnam phone number format
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string phoneNumber)
        {
            var phoneNumberRegrex = new Regex(@"(84|0[3|5|7|8|9])+([0-9]{8})\b");
            if (phoneNumberRegrex.IsMatch(phoneNumber))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if a string has a length of at least min
        /// </summary>
        /// <param name="str"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static bool CheckMinLength(string str, int min)
        {
            if (str.Length < min)
                return false;
            return true;
        }

        /// <summary>
        /// Checks if a string is in password format
        /// Minimum eight characters, at least one letter, one number and one special character
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            var passwordRegrex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
            if (passwordRegrex.IsMatch(password))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if a UserName string is in format which
        /// must be 8-20 characters long and cannot start with a . or _.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName)
        {
            var userNameRegrex = new Regex(@"^(?=[a-zA-Z0-9._]{8,20}$)(?!.*[_.]{2})[^_.].*[^_.]$");
            if (userNameRegrex.IsMatch(userName))
                return true;
            return false;
        }
    } 
}