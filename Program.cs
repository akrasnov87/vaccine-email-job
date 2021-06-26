using System;
using System.Linq;
using System.Collections.Generic;
using Vaccine.Models;
using Vaccine.ReportProvider;
using System.IO;
using System.Net.Mail;

namespace Vaccine
{
    class Program
    { 
        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.Run();
            
        }

        /// <summary>
        /// Запуск
        /// </summary>
        public void Run()
        {
            Mailer mailer = new Mailer();
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = GetAdmins();
                if (users.Length > 0)
                {
                    // общий сводный отчет
                    var orgs = GetUsers();
                    string[][] items = new string[orgs.Length][];
                    for (int i = 0; i < orgs.Length; i++)
                    {
                        int[] stat = GetCount(orgs[i].id);
                        items[i] = new string[] { orgs[i].c_first_name, stat[0].ToString(), stat[1].ToString() };
                    }

                    for (int i = 0; i < users.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(users[i].c_email))
                        {
                            string[] emails = users[i].c_email.Trim().Replace(",", ";").Replace(" ", ";").Split(';');

                            using (AdminReportProvider provider = new AdminReportProvider(items))
                            {
                                Stream stream = provider.GetSteam();

                                MailMessage mailMessage = mailer.GetMailMessage("Сводный отчет для Администратора", "Отчет за " + GetCurrentUserDate(), new ReportItem[] { new ReportItem() { stream = stream, name = "Сводный отчет.xlsx" } }, emails, orgs[i].c_login);
                                mailer.SendMail(mailMessage);
                            }
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
                            
                            // Сводный отчет уровня «Ответственного»
                            string[][] items = new string[1][];
                            int[] stat = GetCount(users[i].id);
                            items[0] = new string[2] { stat[0].ToString(), stat[1].ToString() };

                            var reports = new ReportItem[2];

                            using (UserReportProvider userReportProvider = new UserReportProvider(items))
                            {
                                Stream userReportStream = userReportProvider.GetSteam();
                                reports[0] = new ReportItem() { stream = userReportStream, name = "Сводный отчет.xlsx" };

                                // Отчет уровня «Ответственного»
                                items = GetNames(users[i].id).ItemsToString();

                                using (NamesReportProvider namesReportProvider = new NamesReportProvider(items))
                                {
                                    Stream namesStream = namesReportProvider.GetSteam();
                                    reports[1] = new ReportItem() { stream = namesStream, name = "Отчет.xlsx" };

                                    MailMessage mailMessage = mailer.GetMailMessage("Отчет для Ответственного", "Отчет за " + GetCurrentUserDate(), reports, emails, users[i].c_login);
                                    mailer.SendMail(mailMessage);
                                }
                            }
                        }
                    }
                }
            }
        }

        int[] GetCount(int f_user)
        {
            int[] results = new int[2];

            var names = GetNames(f_user);

            results[0] = names.Sum(t => t.vaccine);
            results[1] = names.Sum(t => t.pcr);

            return results;
        }

        /// <summary>
        /// Дополнительная информация для Отчет уровня «Ответственного»
        /// </summary>
        /// <param name="f_user"></param>
        /// <returns></returns>
        NameItem[] GetNames(int f_user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var query = from d in db.Documents
                            where d.f_user == f_user
                            select new NameItem()
                            {
                                id = d.id,
                                name = d.c_first_name + " " + d.c_last_name + " " + d.c_middle_name,
                                birthDay = d.d_birthday
                            };

                NameItem[] results = query.ToArray();
                foreach(NameItem item in results)
                {
                    var pdf = (from d in db.Documents
                                join f in db.Files on d.id equals f.f_document
                                where d.id == item.id && f.ba_pdf != null
                                orderby f.dx_created descending
                                select new {
                                    dx_created = f.dx_created
                                }).FirstOrDefault();
                    if (pdf != null)
                    {
                        item.vaccine = 1;
                        item.vaccineDate = pdf.dx_created;
                    } else
                    {
                        item.vaccine = 0;
                    }

                    var foto = (from d in db.Documents
                               join f in db.Files on d.id equals f.f_document
                               where d.id == item.id && f.ba_jpg != null
                               orderby f.dx_created descending
                               select new
                               {
                                   dx_created = f.dx_created
                               }).FirstOrDefault();
                    if (foto != null)
                    {
                        item.pcr = 1;
                        item.pcrDate = foto.dx_created;
                    }
                    else
                    {
                        item.pcr = 0;
                    }
                }
                return results;
            }
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
                            select u;

                return users.ToArray();
            }
        }
    }
}
