using System.Linq;
using System.Collections.Generic;
using MyD3.Data.Entities;

namespace MyD3.Data.Interface
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : DbEntity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRanges(List<T> entities);
        void UpdateRanges(List<T> entities);
        void AddRanges(List<T> entities);
        void ExcuteSp(string sp_name);
        IQueryable<T> Table { get; }
    }
}
