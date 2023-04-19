using FluentValidation;
using GrpcNotesService;

namespace GrpcApiService.ServiceName.Validations;
public class NoteInputValidation : AbstractValidator<AddNote>
{
    public NoteInputValidation()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("The file {PropertyName} is required");
    }
}
