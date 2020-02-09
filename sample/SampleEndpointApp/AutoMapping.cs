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
            CreateMap<UpdateAuthorCommand, Author>();

            CreateMap<Author, CreateAuthorResult>();
            CreateMap<Author, UpdatedAuthorResult>();
            CreateMap<Author, AuthorListResult>();
            CreateMap<Author, AuthorResult>();
        }
    }
}
