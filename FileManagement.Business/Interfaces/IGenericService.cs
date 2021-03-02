using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.Interfaces
{
    public interface IGenericService<T> where T : class, new()
    {
        public Task<List<T>> GetAll();
        public Task<T> GetById(int id);
        public Task AddAsync(T entity);
        public Task RemoveAsync(T entity);
        public Task UpdateAsync(T entity);
    }
}
