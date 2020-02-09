using AutoMapper;
using SampleEndpointApp.Authors;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateAuthorCommand, Author>();
            CreateMap<Author, CreateAuthorResult>();
            CreateMap<Author, AuthorListResult>();
        }
    }
}
