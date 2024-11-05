# ASP.NET-Core-API-with-JWT-Auth
This sample solution illustrates how to integrate JWT authentication in an API using Microsoft Identity Framework.

## Running the Project Locally
Before running the project, modify the **appSettings.json** file and update the path to the MDF database file under the **ConnectionStrings:DefaultConnection** section.  You can also use a connection string to your SQL Server database instance with Identity Framework support.

## Obtaining a JWT token
Make a POST call to **api/Auth/Login** using the following JSON payload.  This user has already been created in the attached sample MDF database file.
```
    {
      "emailAddress": "testemail@yourdomain.com",
      "password": "P@ssw0rd"
    }
```

## Authenticating API Calls
Once you obtain a JWT token, try making a GET call to the **WeatherForecast** endpoint.  Add a Bearer authorization header entry.
