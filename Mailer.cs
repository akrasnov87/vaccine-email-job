using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using Vaccine.ReportProvider;

namespace Vaccine
{
    public class Mailer
    {
        static string dir = "temp";

        public Mailer()
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            Directory.CreateDirectory(dir);
        }

        private void Log(string txt)
        {
            Console.WriteLine(txt);
            string fileName = "log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            using (StreamWriter sw = File.AppendText(dir + "/" + fileName))
            {
                sw.WriteLine(txt);
            }
        }

        /// <summary>
        /// Получение сообщения для отправки
        /// </summary>
        /// <param name="title">Тема письма</param>
        /// <param name="message">Сообщение</param>
        /// <param name="reports">Отчеты</param>
        /// <param name="emails">Список адресов для рассылки</param>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public MailMessage GetMailMessage(string title, string message, ReportItem[] reports, string[] emails, string login)
        {
            MailAddress from = new MailAddress("mysmtp1987@gmail.com", "АРМ \"Вакцинация\"");

            if (emails.Length > 0)
            {
                try
                {
                    MailAddress to = new MailAddress(emails[0]);
                    MailMessage mail = new MailMessage(from, to);

                    foreach (ReportItem item in reports)
                    {
                        mail.Attachments.Add(new Attachment(item.stream, item.name));
                    }

                    mail.Subject = title;
                    mail.Body = message;

                    // письмо представляет код html
                    mail.IsBodyHtml = true;

                    if (emails.Length > 1)
                    {
                        for (int i = 1; i < emails.Length; i++)
                        {
                            MailAddress copy = new MailAddress(emails[i]);
                            mail.CC.Add(copy);
                        }
                    }

                    Log("[" + title + "] " + string.Join(";", emails) + "<" + login + ">: sended " + GetCurrentDate());

                    return mail;
                }
                catch (Exception e)
                {
                    Log("[ERR: " + title + "] " + string.Join(";", emails) + "<" + login + ">: sended " + GetCurrentDate() + " - " + e.ToString());
                }
            }

            return null;
        }

        public void SendMail(MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("mysmtp1987@gmail.com", "Bussine$Perfect");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        private string GetCurrentDate()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}
