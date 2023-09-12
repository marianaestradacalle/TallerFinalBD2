using Application.Interfaces.Infrastructure;
using AutoMapper;
using Core.Entities;
using Dobles.Tests.Core.Tests.Entities;
using Dobles.Tests.Fake.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dobles.Tests.Fake.InfrastructureServices;
public class FakeNotasRepository : INotasRepository
{
    private readonly IMapper _mapper;
    private readonly FakeMapper _fakeMapper = new FakeMapper();

    public List<Notes> notes { get; set; }
    public Notes notesBuild { get; set; }
    public FakeNotasRepository()
    {
        notes = new List<Notes>();
        notesBuild = new NotesBuilder().Notes();
        notes.Add(notesBuild);

        _mapper = (IMapper)_fakeMapper.GetFakeMapper();
    }
    public async Task<dynamic> GetAllNotes()
    {
        return Task.Run(() =>
        {
            return _mapper.Map<dynamic>(notes);
        });

    }
}
