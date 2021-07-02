using System;
using System.Linq;
using System.Collections.Generic;
using Vaccine.Models;
using System.Net.Mail;

namespace Vaccine
{
    class Program
    {
        private Mailer mailer;
        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.Run();

            if (args.Length > 0 && args[0] == "true")
            {
                prog.SendReportMail();
            }
        }

        /// <summary>
        /// Запуск
        /// </summary>
        public void Run()
        {
            mailer = new Mailer();
            mailer.Log("processing: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = GetAdmins();
                if (users.Length > 0)
                {
                    // общий сводный отчет
                    for (int i = 0; i < users.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(users[i].c_email))
                        {
                            string[] emails = users[i].c_email.Trim().Replace(",", ";").Replace(" ", ";").Split(';');

                            List<PentahoUrlBuilder> reports = new List<PentahoUrlBuilder>();
                            reports.Add(new PentahoUrlBuilder("total-orgs", "Сводный отчет", "f_user=" + users[i].id));
                            reports.Add(new PentahoUrlBuilder("verify", "Сводный отчет о достоверности сертификата", "f_user=" + users[i].id));
                            reports.Add(new PentahoUrlBuilder("total-orgs-types", "Сводный отчет по отраслям", "f_user=" + users[i].id));
                            reports.Add(new PentahoUrlBuilder("main", "Процентное изменение показателей", "f_user=" + users[i].id, PentahoUrlBuilder.ReturnFormat.PDF));

                            MailMessage mailMessage = mailer.GetMailMessage("Сводный отчет для Администратора", "Отчет за " + GetCurrentUserDate(), reports, emails, users[i].c_login);
                            mailer.SendMail(mailMessage);
                        }
                    }
                }

                // отчет уровня Ответственного
                users = GetUsers();
                if (users.Length > 0)
                {
                    for (int i = 0; i < users.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(users[i].c_email))
                        {
                            string[] emails = users[i].c_email.Trim().Replace(",", ";").Replace(" ", ";").Split(';');

                            List<PentahoUrlBuilder> reports = new List<PentahoUrlBuilder>();
                            reports.Add(new PentahoUrlBuilder("total-orgs", "Сводный отчет", "f_user=" + users[i].id));
                            reports.Add(new PentahoUrlBuilder("users", "Список сотрудников", "f_user=" + users[i].id));
                            reports.Add(new PentahoUrlBuilder("verify", "Отчет о достоверности сертификата", "f_user=" + users[i].id));
                            reports.Add(new PentahoUrlBuilder("main", "Процентное изменение показателей", "f_user=" + users[i], PentahoUrlBuilder.ReturnFormat.PDF));

                            MailMessage mailMessage = mailer.GetMailMessage("Сводный отчет для Ответственного", "Отчет за " + GetCurrentUserDate(), reports, emails, users[i].c_login);
                            mailer.SendMail(mailMessage);
                        }
                    }
                }
            }
            mailer.Log("finished " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        }

        public void SendReportMail()
        {
            mailer.SendReportMail();
        }

        /// <summary>
        /// Текущая дата формирования отчета
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserDate()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        /// <summary>
        /// Список адиминистраторов
        /// </summary>
        /// <returns></returns>
        User[] GetAdmins()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = from uir in db.UserInRoles
                            join r in db.Roles on uir.f_role equals r.id
                            join u in db.Users on uir.f_user equals u.id
                            where r.c_name == "admin" && !u.b_disabled && !u.sn_delete && !uir.sn_delete
                            select u;

                return users.ToArray();
            }
        }

        /// <summary>
        /// Список ответственный
        /// </summary>
        /// <returns></returns>
        User[] GetUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = from uir in db.UserInRoles
                            join r in db.Roles on uir.f_role equals r.id
                            join u in db.Users on uir.f_user equals u.id
                            where r.c_name == "user" && !u.b_disabled && !u.sn_delete && !uir.sn_delete
                            orderby u.c_description, u.c_first_name
                            select u;

                return users.ToArray();
            }
        }
    }
}
