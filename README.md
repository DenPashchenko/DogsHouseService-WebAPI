# DogsHouseService WebAPI

DogsHouseService is an ASP.Net Core Web API (REST) for managing a dogs service.

It has such features:
* "Ping" action for getting a string with the app's name and its actual version.
* Action for querying all dogs. 
  You can specify desired parameters for ordering in a query string(e.g., 'attribute="Name"', 'order="desc"' or 'order="descending"').
  Default values - "Id" and "ascending" respectively.
  Pagination is also supported. Specify desired parameters for it in a query string(e.g., 'pageNumber=2', 'pageSize'=3).
  Default values - 1 and 10 respectively. There is a maximum value for the pageSize - 50.
  Feel free to use sorting and pagination parameters together.
* Action for querying a dog by its Id.
* Action for adding a new dog to DB.
* Rate limiting. Specify needed values in appsettings.json to limit request frequency from the same IP address.
  In case of exceeding the limit, HTTP status code 429 is returned.
  
Note:
As it is an educational project, consider the following:
* DB EnsureCreated method was used instead of applying Migrations.
* 2 entities were initially seeded to the DB.
* Despite there are some libraries for rate limiting, custom logic was implemented to show such possibility.