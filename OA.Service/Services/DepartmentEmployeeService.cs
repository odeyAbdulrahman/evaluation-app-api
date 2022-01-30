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
    class DepartmentEmployeeService : ICrud<DepartmentEmployee>
    {
        private readonly IRepository<DepartmentEmployee> Repository;

        public DepartmentEmployeeService(IRepository<DepartmentEmployee> repository)
        {
            Repository = repository;
        }

        public Task<DepartmentEmployee> FindAsync(short id)
        {
            throw new NotImplementedException();
        }
        public Task<DepartmentEmployee> FindAsync(int id)
        {
            return Repository.GetAsync(id);
        }
        public Task<DepartmentEmployee> FindAsync(long id)
        {
            throw new NotImplementedException();
        }
        public Task<DepartmentEmployee> FirstOrDefaultAsync(Expression<Func<DepartmentEmployee, bool>> filter = null, params Expression<Func<DepartmentEmployee, object>>[] includes)
        {
            return Repository.GetAsync(filter: filter, includes: includes);
        }
        public Task<IEnumerable<DepartmentEmployee>> ListAsync(params Expression<Func<DepartmentEmployee, object>>[] includes)
        {
            return Repository.GetAllAsync(includes: includes);
        }
        public Task<IEnumerable<DepartmentEmployee>> ListAsync(Func<IQueryable<DepartmentEmployee>, IOrderedQueryable<DepartmentEmployee>> orderBy = null, params Expression<Func<DepartmentEmployee, object>>[] includes)
        {
            return Repository.GetAllAsync(orderBy: orderBy, includes: includes);
        }
        public Task<IEnumerable<DepartmentEmployee>> ListAsync(Expression<Func<DepartmentEmployee, bool>> filter = null, Func<IQueryable<DepartmentEmployee>, IOrderedQueryable<DepartmentEmployee>> orderBy = null, params Expression<Func<DepartmentEmployee, object>>[] includes)
        {
            return Repository.GetAllAsync(filter: filter, orderBy: orderBy, includes: includes);
        }

        public Task<IEnumerable<DepartmentEmployee>> ListAsync(int skip, int take, Expression<Func<DepartmentEmployee, bool>> filter = null, Func<IQueryable<DepartmentEmployee>, IOrderedQueryable<DepartmentEmployee>> orderBy = null, params Expression<Func<DepartmentEmployee, object>>[] includes)
        {
            return Repository.GetAllAsync(skip, take, filter: filter, orderBy: orderBy, includes: includes);
        }
        public async Task<(FeedBack, DepartmentEmployee)> PostAsync(DepartmentEmployee model)
        {
            Repository.Insert(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, model) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, IEnumerable<DepartmentEmployee>)> PostAsync(IEnumerable<DepartmentEmployee> models)
        {
            Repository.Insert(models);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, models) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, DepartmentEmployee)> UpdateAsync(DepartmentEmployee model)
        {
            Repository.Update(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.EditedSuccess, model) : (FeedBack.EditedFail, null);
        }
        public async Task<FeedBack> DeleteAsync(DepartmentEmployee model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }

        public async Task<FeedBack> DeleteAsync(IEnumerable<DepartmentEmployee> model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }
    }
}
