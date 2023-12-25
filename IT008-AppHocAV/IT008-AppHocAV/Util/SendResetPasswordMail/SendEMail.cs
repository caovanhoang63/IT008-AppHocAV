using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MimeKit;
namespace IT008_AppHocAV.Util
{
    public class SendEMail
    {
        public static int Send(string userEmail)
        {
            try
            {
                Random random = new Random();
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("ELF - Learning English Friend", "elf.learningenglish@gmail.com"));
                email.To.Add(new MailboxAddress("Receiver Name", userEmail));

                email.Subject = "Reset password";

                // Đọc nội dung từ tệp HTML
                string htmlBody;
                using (StreamReader reader = new StreamReader("../../Util/SendResetPasswordMail/Template/index.html"))
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
                    bodyBuilder.LinkedResources.Add(imagePaths[i], File.ReadAllBytes(
                            $"../../Util/SendResetPasswordMail/Template/images/image-{(i + 1).ToString()}.png"),
                        ContentType.Parse("image/png"));
                }
                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Lưu ý: chỉ cần nếu máy chủ SMTP yêu cầu xác thực
                    smtp.Authenticate("elf.learningenglish@gmail.com", "pfdk sfon wzrc dzux");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return code;
                }
            }
            catch
            {
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