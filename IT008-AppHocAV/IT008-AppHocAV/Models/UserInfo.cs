using System;

namespace IT008_AppHocAV.Models
{
    public class UserInfo
    {
        // Thuộc tính (properties)
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public UserInfo()
        {
        }

        public UserInfo(string fullName, DateTime dateOfBirth, string email, string phoneNumber, string gender, string userName, string password)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            UserName = userName;
            Password = password;
        }
        
    }
}