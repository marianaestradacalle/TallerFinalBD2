using Core.Entities;

namespace Application.Interfaces.Infrastructure;
public interface INotasRepository 
{
    //Task<string> AddNote(Notes notes);
    Task<dynamic> GetAllNotes();
}
