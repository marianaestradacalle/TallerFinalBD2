using Application.DTOs.NotesLists;
using FluentValidation;

namespace RpcApi.Validations;
public class NoteListInputValidation : AbstractValidator<NoteListInput>
{
    public NoteListInputValidation()
    {
        RuleFor(x => x.Name).NotNull().WithMessage("The file {PropertyName} is required");
    }
}
