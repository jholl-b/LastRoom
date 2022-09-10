# LastRoom

## TODO

+ [x] Create CRUD for booking
+ [x] GET /booking only return dates (maybe present 30 days and if it is vacant)
+ [ ] Create UserController to make a GET booking for client ID
+ [x] Create tests for booking
+ [ ] Create tests for controller
+ [ ] Finish the README

## 1. Description

API for the last hotel in Cancun.

Unfortunately we only have one room available, please check the days already booked in advance.

## 2. API

### 2.1. GET (/bookings)

Listing the next 30 days and if the room is vacant.

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

+ Response 204

## 998. Extensions for tests

+ [.NET Core Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer)
+ [Coverage Gutters](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters)

## 999. NuGet Packages:

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