namespace Application.Interfaces.Infraestructure;
public interface IUnitWork
{
    void CreateTransaction();
    void Save();
    Task SaveAsync();
    void Commit();
    void RollBack();
}
