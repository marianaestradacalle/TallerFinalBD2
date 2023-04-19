namespace Application.Interfaces.Services;
public interface INoteCleaningService
{
    public Task DeleteAllChecked();
    public Task InitializationWithList();
}
