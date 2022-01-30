using Microsoft.EntityFrameworkCore;
using OA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("OA.Api")]
namespace OA.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext Context;
        private readonly DbSet<T> Entitie;
        private IQueryable<T> Query;
        public Repository(AppDbContext context)
        {
            Context = context;
            Entitie = context.Set<T>();
            Query = Entitie;
        }

        public virtual async Task<T> GetAsync(short id)
        {
            return await Entitie.FindAsync(id);
        }
        public virtual async Task<T> GetAsync(int id)
        {
            return await Entitie.FindAsync(id);
        }
        public virtual async Task<T> GetAsync(long id)
        {
            return await Entitie.FindAsync(id);
        }
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            return await QueryCreator(filter, null, includes).FirstOrDefaultAsync();
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            return await QueryCreator(null, null, includes).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            return await QueryCreator(null, orderBy, includes).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            return await QueryCreator(filter, orderBy, includes).ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync(int Skip, int Take, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            return await QueryCreator(filter, orderBy, includes).Skip(Skip).Take(Take).ToListAsync();
        }
        public virtual void Insert(T entity)
        {
            Entitie.Add(entity);
        }
        public virtual void Insert(IEnumerable<T> entity)
        {
            Entitie.AddRange(entity);
        }
        public virtual void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Update(IEnumerable<T> entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            Entitie.Remove(entity);
        }
        public virtual void Delete(IEnumerable<T> entity)
        {
            Entitie.RemoveRange(entity);
        }
        public virtual Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }
        #region Helper Mathod
        private IQueryable<T> QueryCreator(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            foreach (var item in includes)
                Query = Query.Include(item);
            if (filter != null)
                Query = Query.Where(filter);
            if (orderBy != null)
                Query = orderBy(Query);
            return Query;
        }
        #endregion
    }
}
