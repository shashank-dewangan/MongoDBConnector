using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBConnector
{
    public interface IDBCollection
    {
        void DisplayRecords(IMongoCollection<BsonDocument> collection);

        void DisplayRecord(List<BsonDocument> records);
        List<BsonDocument> GetRecordByName(string name, IMongoCollection<BsonDocument> _collection);

        List<BsonDocument> GetRecordByNameAlias(string name, string alias, IMongoCollection<BsonDocument> _collection);
    }
}
