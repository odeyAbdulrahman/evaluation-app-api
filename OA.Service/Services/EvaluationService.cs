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
    class EvaluationService : ICrud<Evaluation>
    {
        private readonly IRepository<Evaluation> Repository;

        public EvaluationService(IRepository<Evaluation> repository)
        {
            Repository = repository;
        }

        public Task<Evaluation> FindAsync(short id)
        {
            throw new NotImplementedException();
        }
        public Task<Evaluation> FindAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<Evaluation> FindAsync(long id)
        {
            return Repository.GetAsync(id);
        }
        public Task<Evaluation> FirstOrDefaultAsync(Expression<Func<Evaluation, bool>> filter = null, params Expression<Func<Evaluation, object>>[] includes)
        {
            return Repository.GetAsync(filter: filter, includes: includes);
        }
        public Task<IEnumerable<Evaluation>> ListAsync(params Expression<Func<Evaluation, object>>[] includes)
        {
            return Repository.GetAllAsync(includes: includes);
        }
        public Task<IEnumerable<Evaluation>> ListAsync(Func<IQueryable<Evaluation>, IOrderedQueryable<Evaluation>> orderBy = null, params Expression<Func<Evaluation, object>>[] includes)
        {
            return Repository.GetAllAsync(orderBy: orderBy, includes: includes);
        }
        public Task<IEnumerable<Evaluation>> ListAsync(Expression<Func<Evaluation, bool>> filter = null, Func<IQueryable<Evaluation>, IOrderedQueryable<Evaluation>> orderBy = null, params Expression<Func<Evaluation, object>>[] includes)
        {
            return Repository.GetAllAsync(filter: filter, orderBy: orderBy, includes: includes);
        }

        public Task<IEnumerable<Evaluation>> ListAsync(int skip, int take, Expression<Func<Evaluation, bool>> filter = null, Func<IQueryable<Evaluation>, IOrderedQueryable<Evaluation>> orderBy = null, params Expression<Func<Evaluation, object>>[] includes)
        {
            return Repository.GetAllAsync(skip, take, filter: filter, orderBy: orderBy, includes: includes);
        }
        public async Task<(FeedBack, Evaluation)> PostAsync(Evaluation model)
        {
            Repository.Insert(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, model) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, IEnumerable<Evaluation>)> PostAsync(IEnumerable<Evaluation> models)
        {
            Repository.Insert(models);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.AddedSuccess, models) : (FeedBack.AddedFail, null);
        }
        public async Task<(FeedBack, Evaluation)> UpdateAsync(Evaluation model)
        {
            Repository.Update(model);
            return await Repository.SaveChangesAsync() > 0 ? (FeedBack.EditedSuccess, model) : (FeedBack.EditedFail, null);
        }
        public async Task<FeedBack> DeleteAsync(Evaluation model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }

        public async Task<FeedBack> DeleteAsync(IEnumerable<Evaluation> model)
        {
            Repository.Delete(model);
            return await Repository.SaveChangesAsync() > 0 ? FeedBack.DeletedSuccess : FeedBack.DeletedFail;
        }
    }
}
