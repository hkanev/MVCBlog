using System.Linq;

namespace SimpleForum.Data.Contracts.Repository
{
    public interface IDeletableEntityRepository<T> : IRepository<T> where T : class
    {
        IQueryable<T> AllWithDeleted();
    }
}
