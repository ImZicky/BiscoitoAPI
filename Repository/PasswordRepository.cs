using Model;
using Model.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PasswordRepository
    {
        private readonly string _connMongo;

        public PasswordRepository()
        {
            _connMongo = "";
        }

        public async Task<List<Password>> GetSenhas()
        {
            var client = new MongoClient(_connMongo);
            var db = client.GetDatabase("boavenda");
            var passwordCollection = db.GetCollection<Password>("Biscoito");
            
            return await passwordCollection.FindAsync(Builders<Password>.Filter.Empty).Result.ToListAsync();
        }


        public async Task InsertSenha(Password password)
        {
            var client = new MongoClient(_connMongo);
            var db = client.GetDatabase("boavenda");
            var passwordCollection = db.GetCollection<Password>("Biscoito");

            await passwordCollection.InsertOneAsync(password);
        }



    }
}
