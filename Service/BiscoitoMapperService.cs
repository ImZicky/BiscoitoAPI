using AutoMapper;
using Model.DTO;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service
{
    public interface IBiscoitoMapperService
    {
        IMapper CreateMapper();
    }

    public class BiscoitoMapperService : IBiscoitoMapperService
    {
        public IMapper CreateMapper()
        {
            return new MapperConfiguration(x =>
            {
                _ = x.CreateMap<Password, PasswordDTO>()
                    .ForMember(
                        dest => dest.Nome,
                        opt => opt.MapFrom(src => src.Nome)
                    )
                    .ForMember(
                        dest => dest.Valor,
                        opt => opt.MapFrom(src => src.Valor)
                    );

                _ = x.CreateMap<PasswordDTO, Password>();

            }).CreateMapper();
        }
    }
}
