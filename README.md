# galactic_test
Code for the test that Galactic Advisors requested: A couple of notes

1. I personally think the test listed a lot of requirements but allows very little time to fulfill 100% of them.
2. As written in the requirements, there is no authentication/authorization for the endpoints, maybe in a future version, add Authorization Headers to the requests.
3. For data persistence, I am using MySQL since it's the only technology I currently have readily available. A more professional solution like Sql Server or Oracle could be used. 
   Also a NoSQL solution like Cassandra or MongoDB
4. We could also use Entity Framework with LINQ, that way lots of work can be saved.
5. Using .NET Core 3.1
6. Tested with Postman and CURL
7. I added the data schema to the repository.
8. For MySQl related operation, the MySQLConnector NuGet package was installed.
