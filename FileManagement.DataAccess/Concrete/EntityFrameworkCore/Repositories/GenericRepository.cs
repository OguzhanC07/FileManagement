using FileManagement.DataAccess.Concrete.EntityFrameworkCore.Context;
using FileManagement.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.DataAccess.Concrete.EntityFrameworkCore.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class, new()
    {
        public async Task AddAsync(T entity)
        {
            using var context = new FilemanagementContext();

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll()
        {
            using var context = new FilemanagementContext();

            return await context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter)
        {
            using var context = new FilemanagementContext();
            return await context.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<T> GetByFilter(Expression<Func<T, bool>> filter)
        {
            using var context = new FilemanagementContext();
            return await context.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task<T> GetById(int id)
        {
            using var context = new FilemanagementContext();
            return await context.FindAsync<T>(id);
        }

        public async Task RemoveAsync(T entity)
        {
            using var context = new FilemanagementContext();
            context.Remove(entity);
            await context.SaveChangesAsync();

        }

        public async Task UpdateAsync(T entity)
        {
            using var context = new FilemanagementContext();
            context.Update(entity);

            await context.SaveChangesAsync();
        }
    }
}
