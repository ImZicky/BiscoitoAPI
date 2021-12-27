using Microsoft.Extensions.Configuration;
using Model;
using Model.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mongo
{

    public interface IPasswordRepository
    {
        Task<List<Password>> GetSenhas();
        Task InsertSenha(Password password);
    }

    public class PasswordRepository : IPasswordRepository
    {
        private readonly string _connMongo;
        private readonly IConfiguration _conf;

        public PasswordRepository(IConfiguration conf)
        {
            _conf = conf;
            _connMongo = _conf["ConnectionStrings:MongoDB"];
        }

        public async Task<List<Password>> GetSenhas()
        {
            var client = new MongoClient(_connMongo);
            var db = client.GetDatabase("Biscoito");
            var passwordCollection = db.GetCollection<Password>("Passwords");
            
            return await passwordCollection.FindAsync(Builders<Password>.Filter.Empty).Result.ToListAsync();
        }


        public async Task InsertSenha(Password password)
        {
            var client = new MongoClient(_connMongo);
            var db = client.GetDatabase("Biscoito");
            var passwordCollection = db.GetCollection<Password>("Passwords");

            await passwordCollection.InsertOneAsync(password);
        }



    }
}
