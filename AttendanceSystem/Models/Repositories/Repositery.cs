using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Models.Repositories
{
    public class Repositery<T>: IRepositery<T> where T : class
    {
        public readonly AppDbContext context;
        public readonly DbSet<T> dbSet;
        public Repositery(AppDbContext _Context) { 
            this.context = _Context;
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
            => context.Add(entity);            

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
            => dbSet.ToList();

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
            => context.SaveChanges();

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
