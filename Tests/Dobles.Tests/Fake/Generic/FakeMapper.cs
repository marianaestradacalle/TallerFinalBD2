using Application.DTOs;
using AutoMapper;

namespace Dobles.Tests.Fake.Generic;
public class FakeMapper
{
    private readonly IMapper _fakeMapper;
    public FakeMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _fakeMapper = config.CreateMapper();
    }

    public IMapper GetFakeMapper()
    {
        return _fakeMapper;
    }
}
