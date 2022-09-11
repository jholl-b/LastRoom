# LastRoom

## 1. Description

API for the last hotel in Cancun.

Unfortunately we only have one room available, please check the days already booked in advance.

### 1.1. Running the project

+ dotnet ef --project .\LastRoom.Api\ database update
+ dotnet run --project .\LastRoom.Api\

## 2. API

### 2.1. GET (/bookings/days)

List the next 30 days and if the room is vacant.

+ Response
  
```json
[
	{
		"vacant": false,
		"date": "2022-09-11"
	},
	...
]
```

### 2.2. GET (bookings/{ticket})

The user can retrieve the booking with the ticket.

+ Response

```json
{
	"ticket": "65475ce5-3293-4f96-8c1a-dbb6350420c1",
	"clientFullName": "User 01",
	"checkInDate": "2022-09-11",
	"checkOutDate": "2022-09-13"
}
```

### 2.3. POST (bookings)

The user can create a new booking.

+ Request
```json
{
  "clientIdentification": "id",
  "clientFullName": "User 01",
  "checkInDate": "2022-09-11",
  "checkOutDate": "2022-09-13"
}
```
+ Response
```json
{
	"ticket": "65475ce5-3293-4f96-8c1a-dbb6350420c1",
	"clientFullName": "User 01",
	"checkInDate": "2022-09-11",
	"checkOutDate": "2022-09-13"
}
```

### 2.4. PUT (bookings/{ticket})

The user can update the booking.

+ Request
```json
{
  "clientIdentification": "id",
  "clientFullName": "User 01",
  "checkInDate": "2022-09-11",
  "checkOutDate": "2022-09-13"
}
```
+ Response
```json
{
	"ticket": "65475ce5-3293-4f96-8c1a-dbb6350420c1",
	"clientFullName": "User 01",
	"checkInDate": "2022-09-11",
	"checkOutDate": "2022-09-13"
}
```

### 2.5. DELETE (bookings/{ticket})

The user can undo the booking.

+ Response 204

## 3. Improvements

+ We can remove the manual mapping from the controller with:

  + [AutoMapper](https://www.nuget.org/packages/AutoMapper)

+ We can separate the controllers from the services using CQRS with:

  + [MediatR](https://www.nuget.org/packages/MediatR)

+ MediatR can also be good for separating validation from service.

+ Improve the error object in the return.
+ Create a docker file for deployment.

## 4. Extensions used for tests

+ [.NET Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer)
+ [Coverage Gutters](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters)

## 5. NuGet Packages:

+ LastRoom.Api
  + [FluentResults](https://www.nuget.org/packages/FluentResults/)
  + [Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/)
  + [EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
  + [EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
  + [EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
  + [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore)
+ LastRoom.Api.Tests
  + [coverlet.msbuild](https://www.nuget.org/packages/coverlet.msbuild)
  + [EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory)
  + [Moq](https://www.nuget.org/packages/Moq)
  + [xunit](https://www.nuget.org/packages/xunit)