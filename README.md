# LastRoom

## TODO

+ [x] Create CRUD for booking
+ [ ] GET /booking only return dates (maybe present 30 days and if it is vacant)
+ [ ] Create UserController to make a GET booking for client ID
+ [ ] Create tests for booking
+ [ ] Finish the README

## 1. Description

API for the last hotel in Cancun.

Unfortunately we only have one room available, please check the days already booked in advance.

## 2. API

### 2.1 booking [/booking]

List all bookings for the room.

+ Response 200 (application/json)
  
```json
[
	{
		"ticket": "86a22ae6-a24a-41fa-96a4-19561132cd5f",
		"clientFullName": "user 01",
		"checkInDate": "2022-09-09T00:00:00",
		"checkOutDate": "2022-09-12T23:59:59.9999999"
	},
	{
		"ticket": "4aa2b0f4-c915-46c3-972b-450ad3219c85",
		"clientFullName": "user 02",
		"checkInDate": "2022-09-13T00:00:00",
		"checkOutDate": "2022-09-16T23:59:59.9999999"
	}
]
```

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