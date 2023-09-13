using Core.Entities;

namespace Application.Interfaces.Infrastructure;
public interface INotasRepository 
{
    Task<dynamic> GetAllNotes();
}
