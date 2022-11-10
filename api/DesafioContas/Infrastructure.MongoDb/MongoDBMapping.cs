using AutoMapper;
using Desafio.Contas.Infrastructure.MongoDb.Account;
using Desafio.Contas.Infrastructure.MongoDb.Entry;

namespace Desafio.Contas.Infrastructure.MongoDb
{
    public class MongoDBMapping : Profile
    {
        public MongoDBMapping()
        {
            CreateMap<Domain.Account, AccountDocument>().ReverseMap();

            CreateMap<Domain.Entry, EntryDocument>()
                .ForMember(doc => doc.AccountId, e => e.MapFrom(map => map.Account.Id))
                .ForMember(doc => doc.Type, e => e.MapFrom(map => map.Type.ToString()));

            CreateMap<EntryDocument, Domain.Entry>()
                .ForMember(doc => doc.Account, e => e.MapFrom(map => new Domain.Account { Id = map.AccountId }))
                .ForMember(doc => doc.Type, e => e.MapFrom(map => Enum.Parse<Domain.EntryType>(map.Type)));
        }
    }
}
