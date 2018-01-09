using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBConnector
{
    public class SuperHeroDBCollection : DBCollection
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public bool superpower { get; set; }
        public string extracolumn { get; set; }

        public List<SuperHeroDBCollection> _lstDBCollection { get; set; }

        public override void DisplayRecords(IMongoCollection<BsonDocument> collection)
        {
            var query = collection.AsQueryable<BsonDocument>().ToList();

            foreach (var item in query)
            {
                var myObj = BsonSerializer.Deserialize<SuperHeroDBCollection>(item);
                Console.WriteLine(myObj.name + " aka " + myObj.alias + " " + (myObj.superpower ? "has super power" : ""));
            }
        }
        public override void DisplayRecord(List<BsonDocument> records)
        {
            foreach (var record in records)
            {
                var myObj = BsonSerializer.Deserialize<SuperHeroDBCollection>(record);
                Console.WriteLine();
                Console.WriteLine(" Found record for : " + myObj.name + " aka " + myObj.alias + " " + (myObj.superpower ? "has super power" : ""));
            }
        }
        
    }
}
