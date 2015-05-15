namespace BlogSystem.ConsoleClient
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Net.Http.Headers;
    using System.Collections.Generic;
    using System.Threading;

    using BlogSystem.Data;
    using BlogSystem.Data.Migrations;
    using BlogSystem.Data.Repositories;
    using BlogSystem.Models;

    public class Program
    {
        public static void Main()
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<BlogSystemDbContext, Configuration>());

            var db = new BlogSystemDbContext();

            var user = new User()
            {
                Username = "Mosho",
                RegistrationDate = DateTime.Now,
                Birthday = DateTime.Now.AddDays(-4000),
                Gender = Gender.Male,
                ContactInfo = new UserContactInfo()
            };

            db.Users.Add(user);
            db.SaveChanges();

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:56848/");

            ListStudents(client);
            Thread.Sleep(20);
        }

        static async void ListStudents(HttpClient client)
        {
            var response = client.GetAsync("api/values").Result;
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (response.IsSuccessStatusCode) {
                var usersList = await response.Content.ReadAsAsync<IEnumerable<string>>();
                foreach (var user in usersList) {
                    Console.WriteLine(user);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}