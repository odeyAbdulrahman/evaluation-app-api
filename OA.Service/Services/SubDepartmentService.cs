using OA.Base.Helpers;
using OA.Data.Models;
using OA.Repo;
using OA.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OA.Service.Services
{
    internal class SubDepartmentService : ICrud<SubDepartment>
    {
        private readonly IRepository<SubDepartment> Repository;

        public SubDepartmentService(IRepository<SubDepartment> repository)
        {
            Repository = repository;
        }

        public Task<SubDepartment> FindAsync(short id)
        {
            return Repository.GetAsync(id);
        }
        public Task<SubDepartment> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<SubDepartment> FindAsync(long id)
        {
            throw new NotImplementedException();
        }
        public Task<SubDepartment> FirstOrDefaultAsync(Expression<Func<SubDepartment, bool>> filter = null, params Expression<Func<SubDepartment, object>>[] includes)
        {
            return Repository.GetAsync(filter: filter, includes: includes);
        }
        public Task<IEnumerable<SubDepartment>> ListAsync(params Expression<Func<SubDepartment, object>>[] includes)
        {
            return Repository.GetAllAsync(includes: includes);
        }
        public Task<IEnumerable<SubDepartment>> ListAsync(Func<IQueryable<SubDepartment>, IOrderedQueryable<SubDepartment>> orderBy = null, params Expression<Func<SubDepartment, object>>[] includes)
        {
            return Repository.GetAllAsync(orderBy: orderBy, includes: includes);
        }
        public Task<IEnumerable<SubDepartment>> ListAsync(Expression<Func<SubDepartment, bool>> filter = null, Func<IQueryable<SubDepartment>, IOrderedQueryable<SubDepartment>> orderBy = null, params Expression<Func<SubDepartment, object>>[] includes)
        {
            return Repository.GetAllAsync(filter: filter, orderBy: orderBy, includes: includes);
        }

        public Task<IEnumerable<SubDepartment>> ListAsync(int skip, int take, Expression<Func<SubDepartment, bool>> filter = null, Func<IQueryable<SubDepartment>, IOrderedQueryable<SubDepartment>> orderBy = null, params Expression<Func<SubDepartment, object>>[] includes)
        {
            return Repository.GetAllAsync(skip, take, filter: filter, orderBy: orderBy, includes: includes);
        }
        public async Task<(FeedBack, SubDepartment)> PostAsync(SubDepartment model)
        {
            Repository.Insert(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, model) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, IEnumerable<SubDepartment>)> PostAsync(IEnumerable<SubDepartment> models)
        {
            Repository.Insert(models);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, models) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, SubDepartment)> UpdateAsync(SubDepartment model)
        {
            Repository.Update(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.EditedSuccess, model) : (FeedBack.EditedFail, null);
        }
        public async Task<FeedBack> DeleteAsync(SubDepartment model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }

        public async Task<FeedBack> DeleteAsync(IEnumerable<SubDepartment> model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }
    }
}
