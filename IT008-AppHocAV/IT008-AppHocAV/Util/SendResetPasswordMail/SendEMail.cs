using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
namespace IT008_AppHocAV.Util.SendResetPasswordMail
{
    public class SendEMail
    {
        public static async Task<int> Send(string userEmail)
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string htmlPath = Path.Combine(baseDirectory, "MailTemplate/index.html");
                Random random = new Random();
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("ELF - Learning English Friend", "elf.learningenglish@gmail.com"));
                email.To.Add(new MailboxAddress("Receiver Name", userEmail));

                email.Subject = "Reset password";

                // Đọc nội dung từ tệp HTML
                string htmlBody;
                using (StreamReader reader = new StreamReader(htmlPath))
                {
                    htmlBody = reader.ReadToEnd();
                }

                int code = random.Next(100000, 999999);
                htmlBody = htmlBody.Replace("{PINCODE}", code.ToString());


                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;

                // Tìm và nhúng tất cả các hình ảnh trong HTML
                var imagePaths = ExtractImagePathsFromHtml(htmlBody);
                imagePaths.Reverse();
                for (int i = imagePaths.Count - 1; i >= 0; i--)
                {
                    string imagePath = Path.Combine(baseDirectory, $"MailTemplate/images/image-{(i + 1).ToString()}.png");
                    bodyBuilder.LinkedResources.Add(imagePaths[i], File.ReadAllBytes(
                            imagePath),
                        ContentType.Parse("image/png"));
                }
                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);
                    
                    smtp.Authenticate("elf.learningenglish@gmail.com", "pfdk sfon wzrc dzux");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return code;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
        private static List<string> ExtractImagePathsFromHtml(string html)
        {
            var imagePaths = new List<string>();
            const string pattern = "src\\s*=\\s*\"(?<url>.*?)\"";
            var matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);

            foreach (Match match in matches)
            {
                var urlGroup = match.Groups["url"];
                if (urlGroup.Success)
                {
                    imagePaths.Add(urlGroup.Value);
                }
            }

            return imagePaths;
        }
    }
}