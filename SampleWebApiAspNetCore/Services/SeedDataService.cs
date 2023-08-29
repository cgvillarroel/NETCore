using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(FoodDbContext foodContext, DrinkDbContext drinkContext)
        {
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1000, Type = "Starter", Name = "Lasagne", Created = DateTime.Now });
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1100, Type = "Main", Name = "Hamburger", Created = DateTime.Now });
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1200, Type = "Dessert", Name = "Spaghetti", Created = DateTime.Now });
            foodContext.FoodItems.Add(new FoodEntity() { Calories = 1300, Type = "Starter", Name = "Pizza", Created = DateTime.Now });

            foodContext.SaveChanges();

            drinkContext.DrinkItems.Add(new DrinkEntity() { Calories = 150, Type = "Soda", Name = "Root Beer", Created = DateTime.Now });
            drinkContext.DrinkItems.Add(new DrinkEntity() { Calories = 75, Type = "Beverage", Name = "Orange Juice", Created = DateTime.Now });
            drinkContext.DrinkItems.Add(new DrinkEntity() { Calories = 200, Type = "Shake", Name = "Mango Shake", Created = DateTime.Now });
            drinkContext.DrinkItems.Add(new DrinkEntity() { Calories = 100, Type = "Alcohol", Name = "Beer", Created = DateTime.Now });

            drinkContext.SaveChanges();
        }
    }
}
