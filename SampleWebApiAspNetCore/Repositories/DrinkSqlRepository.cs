using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class DrinkSqlRepository : IDrinkRepository
    {
        private readonly DrinkDbContext _drinkDbContext;

        public DrinkSqlRepository(DrinkDbContext drinkDbContext)
        {
            _drinkDbContext = drinkDbContext;
        }

        public DrinkEntity GetSingle(int id)
        {
            return _drinkDbContext.DrinkItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(DrinkEntity item)
        {
            _drinkDbContext.DrinkItems.Add(item);
        }

        public void Delete(int id)
        {
            DrinkEntity drinkItem = GetSingle(id);
            _drinkDbContext.DrinkItems.Remove(drinkItem);
        }

        public DrinkEntity Update(int id, DrinkEntity item)
        {
            _drinkDbContext.DrinkItems.Update(item);
            return item;
        }

        public IQueryable<DrinkEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<DrinkEntity> _allItems = _drinkDbContext.DrinkItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Calories.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _drinkDbContext.DrinkItems.Count();
        }

        public bool Save()
        {
            return (_drinkDbContext.SaveChanges() >= 0);
        }

        public ICollection<DrinkEntity> GetRandomMeal()
        {
            List<DrinkEntity> toReturn = new List<DrinkEntity>();

            toReturn.Add(GetRandomItem("Starter"));
            toReturn.Add(GetRandomItem("Main"));
            toReturn.Add(GetRandomItem("Dessert"));

            return toReturn;
        }

        private DrinkEntity GetRandomItem(string type)
        {
            return _drinkDbContext.DrinkItems
                .Where(x => x.Type == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
