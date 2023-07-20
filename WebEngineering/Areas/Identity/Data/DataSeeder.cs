using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using WebEngineering.Areas.Identity.Data;
using WebEngineering.Models;

namespace WebEngineering.Data
{
    public static class DataSeeder
    {
        public static void SeedData(IdentityContext context)
        {
            

            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

            User user1 = new User() { Id = Guid.NewGuid().ToString(), UserName = "demo01@gmail.com" }; // user creation
            user1.NormalizedUserName = user1.UserName.ToUpper();
            user1.PasswordHash = passwordHasher.HashPassword(user1, "Demo01!");
            context.Users.Add(user1); ;

            User user2 = new User() { Id = Guid.NewGuid().ToString(), UserName = "demo02@gmail.com" }; // user creation
            user2.NormalizedUserName = user2.UserName.ToUpper();
            user2.PasswordHash = passwordHasher.HashPassword(user2, "Demo02!");
            context.Users.Add(user2); ;

            User user3 = new User() { Id = Guid.NewGuid().ToString(), UserName = "demo03@gmail.com" }; // user creation
            user3.NormalizedUserName = user3.UserName.ToUpper();
            user3.PasswordHash = passwordHasher.HashPassword(user3, "Demo03!");
            context.Users.Add(user3);

            User user4 = new User() { Id = Guid.NewGuid().ToString(), UserName = "demo04@gmail.com" }; // user creation
            user4.NormalizedUserName = user4.UserName.ToUpper();
            user4.PasswordHash = passwordHasher.HashPassword(user4, "Demo04!");
            context.Users.Add(user4);

            User admin = new User() { Id = Guid.NewGuid().ToString(), UserName = "admin@gmail.com" }; // user creation
            admin.NormalizedUserName = admin.UserName.ToUpper();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin10!");
            context.Users.Add(admin);

            IdentityRole role = new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "Manager" };
            role.NormalizedName = role.Name.ToUpper();
            context.Roles.Add(role);

            IdentityUserRole<string> userRole = new IdentityUserRole<string>() { RoleId = role.Id, UserId = admin.Id };
            context.UserRoles.Add(userRole);
            context.SaveChanges();

            List<string> materials = new List<string>()
            {
                "Stahl",
            "Metall",
            "Glas",
            "Kunststoff",
            "Beton",
            "Aluminium",
            "Keramik",
            "Papier",
            "Textil",
            "Gummi",
            "Stein",
            "Silber",
            "Gold",
            "Kupfer",
            "Bronze",
            "Titan",
            "Plexiglas",
            "Acryl"

            };

            List<string> parts = new List<string>()
            {
                "schrauben",
            "bolzen",
            "platten",
            "muttern",
            "unterlegscheiben",
            "federn",
            "zahnräder",
            "dichtungen",
            "ventile",
            "rohre",
            "kabel",
            "schalter",
            "sensoren",
            "pumpen",
            "filter",
            "riemen",
            "gehäuse",

            };

            List<Produkt> products = new List<Produkt>();
            Random random = new Random();
            int counter = 1; // Initialer Zählerwert

            for (int i = 0; i < 20; i++) // generate random product list
            {
                string productName = $"{materials[random.Next(materials.Count)]}{parts[random.Next(parts.Count)]}";

                Produkt produkt = new Produkt()
                {
                    Id = counter,
                    Name = productName
                };
                products.Add(produkt); // kann nicht direkt auf DbSet zugreifen
                context.Produkte.Add(produkt); // In die Datenbank hinzufügen

                counter++; // Zähler erhöhen
            }


            for (int i = 1; i <= 20; i++) // generate Lieferungen with positive IDs
            {
                DateTime lieferungRandomDate = RandomHelper.GetRandomDate();
                Produkt produkt = products[random.Next(products.Count)];


                Lieferung lieferung = new Lieferung()
                {
                    Id = i,
                    Date = lieferungRandomDate,
                    ProduktId = produkt.Id,
                    Menge = random.Next(500, 5000)
                };

                context.Lieferungen.Add(lieferung); // add to DbSet 'Lieferungen'
            }

            for (int i = 1; i <= 80; i++) // generate Bestellungen with positive IDs
            {
                Produkt produkt = products[random.Next(products.Count)];
                DateTime bestellungRandomDate = RandomHelper.GetRandomDate();
                Bestellung bestellung = new Bestellung()
                {
                    Id = i,
                    Date = bestellungRandomDate,
                    ProduktId = produkt.Id,
                    Menge = random.Next(1, 300)
                };

                context.Bestellungen.Add(bestellung); // add to DbSet 'Bestellungen'
            }
            context.SaveChanges(); // Speichern der Änderungen in der Datenbank
        }

        public static DateTime getRandomDate(int startYear = 2022, int endYear = 2023)
        {
            Random random = new Random();
            DateTime startDate = new DateTime(startYear, 1, 1);
            DateTime endDate = new DateTime(endYear, 12, 31);

            TimeSpan timeSpan = endDate - startDate;
            TimeSpan randomTimeSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);

            return (startDate + randomTimeSpan);
        }
    }
}