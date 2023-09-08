namespace Application.Interfaces.Services;
public interface IClearAllUseCase
{
    public Task<bool> Apply();
}
