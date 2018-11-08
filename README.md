# ServiceGovernance.Repository

[![Build status](https://ci.appveyor.com/api/projects/status/a4v2eidom433xrys/branch/master?svg=true)](https://ci.appveyor.com/project/twenzel/servicegovernance-repository/branch/master)
[![NuGet Version](http://img.shields.io/nuget/v/ServiceGovernance.Repository.svg?style=flat)](https://www.nuget.org/packages/ServiceGovernance.Repository/)
[![License](https://img.shields.io/badge/license-Apache-blue.svg)](LICENSE)

ServiceRepository is a combination of middleware and services.
All configuration is done in your startup class.

## Usage

You add the ServiceRepository services to the DI system by calling:

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddServiceRepository();
}

public void Configure(IApplicationBuilder app)
{
    ...
    app.UseServiceRepository();
    ...
    app.UseMvc();
}
```

Optionally you can pass in options into this call.

This will return you a builder object that in turn has a number of convenience methods to wire up additional services.

## In-Memory stores

The "in-memory" configuration APIs allow for configuring ServiceRepository from an in-memory list of configuration objects.

Use of these configuration APIs are designed for use when prototyping, developing, and/or testing where it is not necessary to dynamically consult database at runtime for the configuration data.

* `AddInMemoryApiStore`
    Registers `IApiStore` implementation storing apis as in-memory list. Optional arguments are api documents which will be added to the store.

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddServiceRepository()
        .AddInMemoryApiStore();
}
```

## Other stores

Available persistence libraries are:

* [Entity Framework](https://github.com/ServiceGovernance/ServiceGovernance.Repository.EntityFramework)
* [Redis](https://github.com/ServiceGovernance/ServiceGovernance.Repository.Redis)

### Custom stores

Use one of the following extension method to register your custom store:

* `AddApiStore`
    Adds `IApiStore` implementation for reading and storing api descriptions.


## APIs

Following APIs are provided by this library:

### Publish api description

This endpoint publishes an api description in the service repository.

|Url|Method|Type
|-|-|-|
|/v1/api/{serviceId}|POST|application/json

#### Parameter
`serviceId` The unique service identitier to publish the document to.

body:
```json
{
    OpenApiDocument (v3)    
}
```

Specification for the OpenApi v3 see [here](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md).

#### Response

`text/plain` HTTP 200


### Get Api

This endpoint returns the published api description.

|Url|Method|Type
|-|-|-|
|/v1/api/{serviceId}|Get|

#### Parameter
`serviceid` The service you want to retrieve

#### Response

`application/json` HTTP 200

The endpoint returns the published api description.

```json
{
    "serviceId": "UniqueServiceId",
    "apiDocument": OpenApiDocument (v3)    
}
```

Specification for the OpenApi v3 see [here](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md).

### Get all Apis

This endpoint returns all published api descriptions.

|Url|Method|Type
|-|-|-|
|/v1/api|Get|

### Parameter

### Response

`application/json` HTTP 200

The endpoint returns all published api descriptions.

```json
[
    {
        "serviceId": "UniqueServiceId",
        "apiDocument": OpenApiDocument (v3) 
    },
    {
        ...
    }
]
```

Specification for the OpenApi v3 see [here](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md).