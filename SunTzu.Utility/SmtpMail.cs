
namespace SunTzu.Utility
{
    public class SmtpMail
    {
        public static bool SendSMTPEMail(string strSmtpServer, string strSmtpLoginName, string strSmtpPassword, string strFrom, string strTo, string strSubject, string strBody)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(strSmtpServer);
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(strSmtpLoginName, strSmtpPassword);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(strFrom, strTo, strSubject, strBody);
            message.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");
            message.IsBodyHtml = true;

            try
            {
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
