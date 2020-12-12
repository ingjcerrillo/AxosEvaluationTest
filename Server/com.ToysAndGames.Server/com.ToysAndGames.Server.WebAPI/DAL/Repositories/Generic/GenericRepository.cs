using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.ToysAndGames.Server.WebAPI.DAL.Repositories.Generic
{
    //Generic Repository implementation

    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(object id, T entity);
        Task<bool> DeleteAsync(object id);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await unitOfWork.Context.Set<T>().AddAsync(entity);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            try
            {
                T dbEntity = await unitOfWork.Context.Set<T>().FindAsync(id);
                unitOfWork.Context.Set<T>().Remove(dbEntity);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await unitOfWork.Context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await unitOfWork.Context.Set<T>().FindAsync(id);
        }

        public async Task<bool> UpdateAsync(object id, T entity)
        {
            try
            {
                T dbEntity = await unitOfWork.Context.Set<T>().FindAsync(id);
                unitOfWork.Context.Entry(dbEntity).CurrentValues.SetValues(entity);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
