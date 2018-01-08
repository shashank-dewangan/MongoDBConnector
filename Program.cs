using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;

namespace MongoDBConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "mongodb://localhost:27017";

            MongoClient mClient = null;

            try
            {
                mClient = new MongoClient(connectionString);
                var database = mClient.GetDatabase("MyDatabase");
                var collection = database.GetCollection<BsonDocument>("Avengers");
                                
                DisplayRecords(collection);

                //Display the count
                var query = collection.AsQueryable<BsonDocument>().ToList();
                Console.WriteLine(query.Count);

                InsertOneOptions option = new InsertOneOptions();
                
                //Insert a record
                BsonDocument doc = new BsonDocument();
                doc.Add(new BsonElement("name", "Wonda Maximoff"));
                doc.Add(new BsonElement("alias", "Scarlet Witch"));
                doc.Add(new BsonElement("superpower", true));
                
                collection.InsertOneAsync(doc);

                //Insert Many record
                BsonDocument doc1 = new BsonDocument();
                doc1.Add(new BsonElement("name", "Peter Parker"));
                doc1.Add(new BsonElement("alias", "Spiderman"));
                doc1.Add(new BsonElement("superpower", true));

                BsonDocument doc2 = new BsonDocument();
                doc2.Add(new BsonElement("name", "Clint Barton"));
                doc2.Add(new BsonElement("alias", "Hawkeye"));
                doc2.Add(new BsonElement("superpower", false));

                BsonDocument doc3 = new BsonDocument();
                doc3.Add(new BsonElement("name", "Bucky Barnes"));
                doc3.Add(new BsonElement("alias", "Winter Soldier"));
                doc3.Add(new BsonElement("superpower", true));

                BsonDocument[] multiData = { doc1,doc2,doc3};

                collection.InsertManyAsync(multiData);

                //Update existing record
                var filter = Builders<BsonDocument>.Filter.Eq("name", "Tony Stark");
                var update = Builders<BsonDocument>.Update.Set("superpower", false);

                collection.UpdateOneAsync(filter, update);
                
                DisplayRecords(collection);

                var filterToDelete = Builders<BsonDocument>.Filter.Eq("name", "Wonda Maximoff");
                
                //Delete a record
                collection.DeleteOneAsync(filterToDelete);

                DisplayRecords(collection);
            }
            catch (MongoClientException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (MongoServerException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }

        static void DisplayRecords(IMongoCollection<BsonDocument> collection)
        {
            var query = collection.AsQueryable<BsonDocument>().ToList();

            foreach (var item in query)
            {
                var myObj = BsonSerializer.Deserialize<MyClass>(item);
                Console.WriteLine(myObj.name + " aka " + myObj.alias + " " + (myObj.superpower ? "with super power" : ""));
            }
        }
    }
    class MyClass
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string alias { get; set; }
        public bool superpower { get; set; }
    }
}
