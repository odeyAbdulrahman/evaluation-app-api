using OA.Base.Helpers;
using OA.Data.Models;
using OA.Repo;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OA.Service.Services
{
    class DepartmentService : ICrud<Department>
    {
        private readonly IRepository<Department> Repository;

        public DepartmentService(IRepository<Department> repository)
        {
            Repository = repository;
        }

        public Task<Department> FindAsync(short id)
        {
            return Repository.GetAsync(id);
        }
        public Task<Department> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<Department> FindAsync(long id)
        {
            throw new NotImplementedException();
        }
        public Task<Department> FirstOrDefaultAsync(Expression<Func<Department, bool>> filter = null, params Expression<Func<Department, object>>[] includes)
        {
            return Repository.GetAsync(filter: filter, includes: includes);
        }
        public Task<IEnumerable<Department>> ListAsync(params Expression<Func<Department, object>>[] includes)
        {
            return Repository.GetAllAsync(includes: includes);
        }
        public Task<IEnumerable<Department>> ListAsync(Func<IQueryable<Department>, IOrderedQueryable<Department>> orderBy = null, params Expression<Func<Department, object>>[] includes)
        {
            return Repository.GetAllAsync(orderBy: orderBy, includes: includes);
        }
        public Task<IEnumerable<Department>> ListAsync(Expression<Func<Department, bool>> filter = null, Func<IQueryable<Department>, IOrderedQueryable<Department>> orderBy = null, params Expression<Func<Department, object>>[] includes)
        {
            return Repository.GetAllAsync(filter: filter, orderBy: orderBy, includes: includes);
        }

        public Task<IEnumerable<Department>> ListAsync(int skip, int take, Expression<Func<Department, bool>> filter = null, Func<IQueryable<Department>, IOrderedQueryable<Department>> orderBy = null, params Expression<Func<Department, object>>[] includes)
        {
            return Repository.GetAllAsync(skip, take, filter: filter, orderBy: orderBy, includes: includes);
        }
        public async Task<(FeedBack, Department)> PostAsync(Department model)
        {
            Repository.Insert(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, model) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, IEnumerable<Department>)> PostAsync(IEnumerable<Department> models)
        {
            Repository.Insert(models);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, models) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, Department)> UpdateAsync(Department model)
        {
            Repository.Update(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.EditedSuccess, model) : (FeedBack.EditedFail, null);
        }
        public async Task<FeedBack> DeleteAsync(Department model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }

        public async Task<FeedBack> DeleteAsync(IEnumerable<Department> model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }
    }
}
