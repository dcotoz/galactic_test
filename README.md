# galactic_test
Code for the Galactic Advisors test by Daniel Coto: A couple of notes

1. As stated in the requirements, there is no authentication/authorization for the endpoints, maybe in a future version, add Authorization Headers to the requests.
2. For data persistence, I am using MySQL since it's the only technology I currently have readily available. A more professional solution like Sql Server or Oracle could be used. 
   Also a NoSQL solution like Cassandra or MongoDB
3. We could also use Entity Framework with LINQ, that way lots of work can be saved.
4. Using .NET Core 3.1
5. Tested with Postman and CURL
6. I added the data schema sql dump to the repository.
7. For MySQl related operation, the MySQLConnector NuGet package was installed.
8. For data pagination, the assumed page size is 25 entries per page
9. Ran out of time to work on SwaggerUI.
