namespace infrastructure.repositories;

public interface IRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    T Create(T entity);
    T Update(T entity);
    T Delete(int id);
}