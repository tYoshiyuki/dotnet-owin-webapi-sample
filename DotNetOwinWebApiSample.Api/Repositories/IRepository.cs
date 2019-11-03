using System.Collections.Generic;

namespace DotNetOwinWebApiSample.Api.Repositories
{
    public interface IRepository<TEntity>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        IEnumerable<TEntity> Get();
        void Remove(TEntity entity);
    }
}
