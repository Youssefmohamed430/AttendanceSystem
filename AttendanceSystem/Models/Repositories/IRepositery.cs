﻿namespace AttendanceSystem.Models.Repositories
{
    public interface IRepositery<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();
    }
}
