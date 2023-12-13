using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl =	true;
			Client.Credentials = new NetworkCredential("mostafaahmedmahmoud98@gmail.com", "npfyuxniitdkltnx");
			Client.Send("mostafaahmedmahmoud98@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
