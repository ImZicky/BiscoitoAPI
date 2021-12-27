using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Entity
{
    [BsonDiscriminator("Password")]
    public class Password
    {

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string _id { get; set; }

        [BsonElement("name")]
        public string Nome { get; set; }

        [BsonElement("value")]
        public string Valor { get; set; }

        [BsonElement("icon")]
        public string Icone { get; set; }
    }
}
