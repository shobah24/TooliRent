using Microsoft.Extensions.DependencyInjection;
using static TooliRent.Domain.Enums.Status;
using TooliRent.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace TooliRent.Infrastructure.Data
{
    public class ToolAndRentSeed
    {
        public static async Task SeedCategoriesAndToolsAsync(IServiceProvider services)
        {
            var db = services.GetRequiredService<TooliRentDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            if (!db.Categories.Any())
            {
                var cat1 = new Category { Name = "Handverktyg" };
                var cat2 = new Category { Name = "Elektriskt" };
                var cat3 = new Category { Name = "Mätinstrument" };

                db.Categories.AddRange(cat1, cat2, cat3); 

                db.Tools.AddRange(
                    new Tool
                    {
                        Name = "Skruvmejselset",
                        Description = "Philips + Torx, 10 delar",
                        Category = cat1,
                        Status = ToolStatus.Available
                    },
                    new Tool
                    {
                        Name = "Hammare",
                        Description = "750g snickarhammare",
                        Category = cat1,
                        Status = ToolStatus.Available
                    },
                    new Tool
                    {
                        Name = "Såg",
                        Description = "Handsåg för trä",
                        Category = cat1,
                        Status = ToolStatus.Maintenance
                    },
                    new Tool
                    {
                        Name = "Borrmaskin",
                        Description = "18V batteridriven med 2 batterier",
                        Category = cat2,
                        Status = ToolStatus.Available
                    },
                    new Tool
                    {
                        Name = "Cirkelsåg",
                        Description = "230mm elektrisk såg",
                        Category = cat2,
                        Status = ToolStatus.Damaged
                    },
                    new Tool
                    {
                        Name = "Multimeter",
                        Description = "Digital, upp till 600V",
                        Category = cat3,
                        Status = ToolStatus.Available
                    },
                    new Tool
                    {
                        Name = "Laseravståndsmätare",
                        Description = "Mäter upp till 40m",
                        Category = cat3,
                        Status = ToolStatus.Rented
                    }
                );


                await db.SaveChangesAsync();
            }

            // seed för bokning
            if (!db.Bookings.Any())
            {
                var user = await userManager.FindByNameAsync("member");
                if (user == null)
                {
                    throw new Exception("Member användare kan inte hittas.");
                }

                // Hämta verktyg från databasen
                var borrmaskin = db.Tools.FirstOrDefault(t => t.Name == "Borrmaskin");
                var hammare = db.Tools.FirstOrDefault(t => t.Name == "Hammare");

                if (borrmaskin == null || hammare == null)
                {
                    throw new Exception("Verktyg kan inte hittas.");
                }

                db.Bookings.AddRange(
                    new Booking
                    {
                        UserId = user.Id,
                        ToolId = borrmaskin.Id,
                        LoanDate = DateTime.Today.AddDays(-2),
                        ReturnDate = DateTime.Today.AddDays(1),
                        Status = BookingStatus.Loaned,
                        PickupDate = DateTime.Today.AddDays(-2),
                        ReturnedDate = DateTime.MinValue
                    },
                    new Booking
                    {
                        UserId = user.Id,
                        ToolId = hammare.Id,
                        LoanDate = DateTime.Today,
                        ReturnDate = DateTime.Today.AddDays(3),
                        Status = BookingStatus.Pending,
                        PickupDate = DateTime.MinValue,
                        ReturnedDate = DateTime.MinValue
                    }
                 );

                await db.SaveChangesAsync();
            }
        }

    }
}
