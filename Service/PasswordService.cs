using AutoMapper;
using Model.DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPasswordService
    {
        Task<List<PasswordDTO>> GetSenhas();
    }

    public class PasswordService : IPasswordService
    {

        private readonly IMapper _mapper;

        public PasswordService()
        {
            var biscoitoMapper = new BiscoitoMapperService();
            _mapper = biscoitoMapper.CreateMapper();
        }

        public async Task<List<PasswordDTO>> GetSenhas()
        {   

            var lstPasswordsDTO = new List<PasswordDTO>();

            var rep = new PasswordRepository();

            var lstPasswordsEtt = await rep.GetSenhas();


            foreach (var p in lstPasswordsEtt)
            {
                lstPasswordsDTO.Add(_mapper.Map<PasswordDTO>(p));
            }



            return lstPasswordsDTO;
        }





    }
}
