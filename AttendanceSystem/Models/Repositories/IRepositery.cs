namespace AttendanceSystem.Models.Repositories
{
    public interface IRepositery<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);
        T GetById(string id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(string id);
        void Save();
    }
}
