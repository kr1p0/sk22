using System.Net;
using System.Net.Mail;

namespace SK.Models
{
    /* wksyqglfnwdvqgma */
    public class Email
    {
        public Email()
        {
        }

        public static string sendMail(List<string> emailAdressList, string subject, string content, dynamic file = null)
        {
            // [important if gmail] https://accounts.google.com/b/0/DisplayUnlockCaptcha

            MailAddressCollection emailAdressCollection = new MailAddressCollection();
            foreach (var email in emailAdressList)
                emailAdressCollection.Add(email);


            var sender = "phpxd2@gmail.com";
            const string senderPassword = "wksyqglfnwdvqgma";
            //var file = new Attachment(@"C:myreport.txt");
            MailMessage mailMsg = new MailMessage();
            MailAddress mailAdress = new MailAddress(sender);
            mailMsg.To.Add(emailAdressCollection.ToString());
            mailMsg.From = mailAdress;
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                    mailMsg.Attachments.Add(att);
                }
            }

            mailMsg.Body = content;
            mailMsg.IsBodyHtml = true;
            mailMsg.Subject = subject;

            var smtp = new SmtpClient();
            {
                smtp.Host = " smtp.gmail.com ";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(sender, senderPassword);
                smtp.Timeout = 20000;
            }
            try
            {
                smtp.SendMailAsync(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("sendMail: " + ex.Message);
                return "(🗙) Wystąpił błąd";
            }
            finally
            {
                //file.Dispose();
            }
            Console.WriteLine("Wysłano");
            return "(✓) Pomyślnie wysłano";
        }
    }
}
