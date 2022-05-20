using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Model;

namespace MongoDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello NoSQL World!");

            string connectionString = "mongodb://localhost:27017";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);
            var db = client.GetDatabase("publisher");
            var collection = db.GetCollection<Book>("books");
            //InitData(db,collection);
            UpdateData(collection);
            FetchData(collection);
            //FetchDataLinq(collection);

            var index = Builders<Book>.IndexKeys.Ascending(nameof(Book.Year));
            collection.Indexes.CreateOne(index);

            Console.ReadLine();

        }


        private static void UpdateData(IMongoCollection<Book> collection)
        {

            var update = Builders<Book>.Update.Set(nameof(Book.Title), "new name");
            var filter = Builders<Book>.Filter.Eq(nameof(Book.Title), 100);
            collection.UpdateMany(filter, update);
        }

        private static void FetchDataLinq(IMongoCollection<Book> collection)
        {


            var docs = collection.AsQueryable().Where(p => p.Year == 2022 && p.Price >= 10_000).OrderByDescending(p=>p.Price).ToList();

            foreach (var book in docs)
            {
                Console.WriteLine(book.Title + " By " + book.Authors[0].Name);
            }
        }

        private static void FetchData(IMongoCollection<Book> collection)
        {
            //var filter = new BsonDocument();
            //var filter = Builders<Book>.Filter.Eq(nameof(Book.Title), 1);
            var filterBuilder = Builders<Book>.Filter;
            var sort = Builders<Book>.Sort.Descending(nameof(Book.Price)).Descending(nameof(Book.Title));
            var filter = filterBuilder.Eq(nameof(Book.Year), 2022) & filterBuilder.Gte("Price", 10_000);

            var docs = collection.Find(filter).Sort(sort).ToList();

            foreach (var book in docs)
            {
                Console.WriteLine(book.Title + " By " + book.Authors[0].Name);
            }
        }

        private static void InitData(IMongoDatabase db,IMongoCollection<Book> collection)
        {
            
            db.DropCollection("books");
           

            var books = new List<Book>();

            for (int i = 0; i < 1_000; i++)
            {
                books.Add(new Book
                {
                    ISBN = i.ToString(),
                    Price = i*500,
                    Title = i.ToString(),
                    Year = 2022,
                    LastStock = DateTime.Now,
                    Language = new Language
                    {
                       Name = "English"
                    },
                    Authors = new List<Author>
                    {
                        new Author
                        {
                            Name = "Alex"
                        }
                    }
                    
                    
                });
            }

            collection.InsertManyAsync(books);
        }
    }
}
