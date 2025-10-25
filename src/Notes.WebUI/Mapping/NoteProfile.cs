using AutoMapper;
using Notes.Application.Notes;
using Notes.Domain.Notes;

namespace Notes.WebUI.Mapping;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<Note, NoteDto>();
    }
}
