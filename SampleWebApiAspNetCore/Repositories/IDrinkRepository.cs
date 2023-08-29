using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IDrinkRepository
    {
        DrinkEntity GetSingle(int id);
        void Add(DrinkEntity item);
        void Delete(int id);
        DrinkEntity Update(int id, DrinkEntity item);
        int Count();
        bool Save();
    }
}
