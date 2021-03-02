using FileManagement.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.DataAccess.Interfaces
{
    public interface IGenericDal<T> where T: class,new()
    {
        public Task<List<T>> GetAll();
        public Task<T> GetById(int id);
        public Task AddAsync(T entity);
        public Task RemoveAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter);
        public Task<T> GetByFilter(Expression<Func<T,bool>> filter);
    }
}
