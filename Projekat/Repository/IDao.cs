namespace Projekat.Repository
{
    public interface IDao<T>
    {
        T FindByID(int id);
        T Add(T item);
        T Delete(int id);
        T Update(T updated);

    }
}
