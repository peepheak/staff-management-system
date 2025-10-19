using API.Entities;
using API.Request;
using API.Response;
using AutoMapper;

namespace API.Mapping;

public class StaffProfile : Profile
{
    public StaffProfile()
    {
        CreateMap<Staff, StaffAddRequest>().ReverseMap();
        CreateMap<Staff, StaffUpdateRequest>().ReverseMap();
        CreateMap<Staff, StaffResponse>().ReverseMap();
    }
}