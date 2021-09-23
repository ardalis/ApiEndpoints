using AutoMapper;
using SampleEndpointApp.DomainModel;
using SampleEndpointApp.Endpoints.Authors;

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
