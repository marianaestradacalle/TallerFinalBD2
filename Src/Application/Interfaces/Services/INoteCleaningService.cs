namespace Application.Interfaces.Services;
public interface INoteCleaningService
{
    public Task<bool> DeleteAllChecked();
    public Task<bool> InitializationWithList();
}
