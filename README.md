<div align="center">

![readme splash](https://raw.githubusercontent.com/jeffreylanters/unity-nest-utilities-client/master/.github/WIKI/repository-readme-splash.png)

[![license](https://img.shields.io/badge/mit-license-red.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-nest-utilities-client/blob/master/LICENSE.md)
[![openupm](https://img.shields.io/npm/v/nl.elraccoone.nest-utilities-client?label=UPM&registry_uri=https://package.openupm.com&style=for-the-badge&color=232c37)](https://openupm.com/packages/nl.elraccoone.nest-utilities-client/)
[![build](https://img.shields.io/badge/build-passing-brightgreen.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-nest-utilities-client/actions)
[![deployment](https://img.shields.io/badge/state-success-brightgreen.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-nest-utilities-client/deployments)
[![stars](https://img.shields.io/github/stars/jeffreylanters/unity-nest-utilities-client.svg?style=for-the-badge&color=fe8523&label=stargazers)](https://github.com/jeffreylanters/unity-nest-utilities-client/stargazers)
[![awesome](https://img.shields.io/badge/listed-awesome-fc60a8.svg?style=for-the-badge)](https://github.com/jeffreylanters/awesome-unity-packages)
[![size](https://img.shields.io/github/languages/code-size/jeffreylanters/unity-nest-utilities-client?style=for-the-badge)](https://github.com/jeffreylanters/unity-nest-utilities-client/blob/master/Runtime)
[![sponsors](https://img.shields.io/github/sponsors/jeffreylanters?color=E12C9A&style=for-the-badge)](https://github.com/sponsors/jeffreylanters)
[![donate](https://img.shields.io/badge/donate-paypal-F23150?style=for-the-badge)](https://paypal.me/jeffreylanters)

Providing a set of tools allowing your application to interface with servers running rest-full API's build using the [Nest Utilities](https://github.com/MartinDrost/nest-utilities) package with custom services and chainable options for complete flexibility.

[**Installation**](#installation) &middot;
[**Documentation**](#documentation) &middot;
[**License**](./LICENSE.md)

**Made with &hearts; by Jeffrey Lanters**

</div>

# Installation

### Using the Unity Package Manager

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity.

```json
"nl.elraccoone.nest-utilities-client": "git+https://github.com/jeffreylanters/unity-nest-utilities-client"
```

### Using OpenUPM

The module is availble on the OpenUPM package registry, you can install the latest stable release using the OpenUPM Package manager's Command Line Tool using the following command.

```sh
openupm add nl.elraccoone.nest-utilities-client
```

# Documentation

The package revolves around the CrudService. Extending this service will grant you methods to call upon the endpoints generated by [Nest Utilities](https://github.com/MartinDrost/nest-utilities) as well as the available options provided by the chainable request builder. Each service revolves around it's respective serializable resource model.

**Compatible with Nest Utilties 8.0.0 or greater.**

## Getting started

Start off by creating a service for each of your resources. This is as simple as creating a new class and extending the CRUD Service provided by the Namespace. Extend the constructor with some basic parameters and your Model's Type. The Model's Type is responsible for transfering data in our resource's format.

```csharp
using ElRaccoone.NestUtilitiesClient;

public class UserService : CrudService<User> {
  public UserService () : base (
    hostname: "my-awesome-api.com",
    resource: "user"
  ) { }
}
```

To use your newly created Service, simply create an instance somewhere in your application. Preferably in some sort of Network Component. To get started with your first request, use one for the CRUD methods such as Get, Post, Delete or Patch provided by your Service. Once instanciated, a newly created Request Builder will be returned.

```csharp
public class TestComponent : MonoBehaviour {
  private UserService userService = new UserService ();

  private void Test () {
    this.userService.Read ();
    this.userService.Read (id: "...");
    this.userService.Create (model: new User (...));
    this.userService.Update (model: new User (...));
    this.userService.Delete (id: "...");
  }
}
```

Once created your Request using the Builder, it's time to send it over to your API. To do this, invoke or await the Send method provided by the builder. This will start the request and send over your data over to the server.

```csharp
public class TestComponent : MonoBehaviour {
  private UserService userService = new UserService ();

  private async void Test () {
    await this.userService.Create (new User ()).Send ();
  }
}
```

To confirm a request was successful, simply wrap it within a Try Catch closure. When something went wrong during the request, a request exception will be thrown containing information to debug the problem such as the status code and the raw response of the server.

```csharp
public class TestComponent : MonoBehaviour {
  private UserService userService = new UserService ();

  private async void Test () {
    try {
      var users = await this.userService.Read ().Send ();
      foreach (var user in users)
        Debug.Log ($"Hello {user.firstName}!")
    }
    catch (RequestException exception) {
      Debug.Log ($"Something went wrong! Error {exception.statusCode}");
    }
  }
}
```

## Chainable Options

Chainable options can be used for altering the way your API is handling your requests.

```csharp
public class TestComponent : MonoBehaviour {
  private UserService userService = new UserService ();

  private async void Test () {
    var users = await this.userService
      .Read ()
      .Limit (amount: 10)
      .Sort (field: "firstName", sortingOptions: SortingOption.Descending)
      .MatchRegex (field: "email", value: "hulan", matchingOptions: MatchingOption.CaseInsensitive | MatchingOption.Global)
      .Send ();
  }
}
```

#### Populate

> This option is only available on Get / Read requests.

This parameter allows you to populate references to other collections in the response.

```csharp
public RequestBuilder<ModelType> Populate (params string[] fields);
```

#### Select

> This option is only available on Get / Read requests.

This parameter allows you to define which fields you want the results to contain. If one or more fields have been selected for a layer, the remaining layers will be omitted from the response. You can deep select fields by separating fields using a dot (f.e. brewers.name).

```csharp
public RequestBuilder<ModelType> Select (params string[] fields);
```

#### Sort

> This option is only available on Get / Read requests.

This parameter allows you to sort the response data on one or more fields in the desired order.

```csharp
public RequestBuilder<ModelType> Sort (params string[] fields);
```

> This option is only available on Get / Read requests.

This parameter allows you to sort the response data on one or more fields in the desired order. Define one or more sorting options.

```csharp
public RequestBuilder<ModelType> Sort (string field, SortingOption sortingOptions);
```

#### Match Exact

> This option is only available on Get / Read requests.

This parameter allows you to match the response data on a specific exact value on a specific field including the casing.

```csharp
public RequestBuilder<ModelType> MatchExact (string field, object value);
```

#### Match Regex

> This option is only available on Get / Read requests.

This parameter allows you to match the response data based on a regex filter on a specific field.

```csharp
public RequestBuilder<ModelType> MatchRegex (string field, object value);
```

> This option is only available on Get / Read requests.

This parameter allows you to match the response data based on a regex filter on a specific field. One or more matching options will be translated into regex options.

```csharp
public RequestBuilder<ModelType> MatchRegex (string field, object value, MatchingOption matchingOptions);
```

#### Match Range

> This option is only available on Get / Read requests.

This parameter allows you to match the response data on a specific range value on a specific field.

```csharp
public RequestBuilder<ModelType> MatchRange (string field, object minimumValue, object maximumValue);
```

#### Offset

> This option is only available on Get / Read requests.

This parameter allows you to skip the first n number of results.

```csharp
public RequestBuilder<ModelType> Offset (int amount);
```

#### Limit

> This option is only available on Get / Read requests.

This parameter allows you to limit the response to only show the next n number of results.

```csharp
public RequestBuilder<ModelType> Limit (int amount);
```

#### Distinct

> This option is only available on Get / Read requests.

This parameter allows you to find the distinct values for a specified field. The returned models will each contain unique values for that field. When multiple models in the actual response would have the same value, the first encountered model will be chosen based on the sort attribute.

```csharp
public RequestBuilder<ModelType> Distinct (string field);
```

#### Random

> This option is only available on Get / Read requests.

This parameter allows you to randomize the order of the response data. This parameter holds priority over the sort parameter which means that the sort will be omitted when random is defined.

```csharp
public RequestBuilder<ModelType> Random ();
```

## Writing Middleware

Out of the box the CrudService will supplement your extended class with a basic methods to execute your calls. In a lot of cases you'll want to and define headers to send along with your requests.

You can extend this behaviour by creating a new MiddleWare class which exposes a set of build in middleware methods. In this new class it is possible to override abstract methods which help you tailor the middleware with ease.

```csharp
using ElRaccoone.NestUtilitiesClient;

public class CustomMiddleware : RequestMiddleware {
  public override Header[] OnGetHeaders () => new Header[] {
    new Header (name: "Authorization", value: "...")
  };

  public override void OnRequestDidCatch (RequestException exception) {
    if (exception.statusCodeDefinition == StatusCodeDefinition.Unauthorized)
      Debug.Log ("Return to login-screen!");
  }
}
```

```csharp
using ElRaccoone.NestUtilitiesClient;

public class UserService : CrudService<User> {
  public UserService () : base (
    hostname: "my-awesome-api.com",
    resource: "user",
    requestMiddleware: new CustomMiddleware()
  ) { }
}
```

#### OnGetHeaders

Implement this virtual method in order to intercept the headers of every request made by this service.

```csharp
public virtual RequestMiddleware.Header[] OnGetHeaders ();
```

#### OnRequestDidCatch

Implement this virtual method in order to catch every request which did run into an HTTP error.

```csharp
public virtual void OnRequestDidCatch (RequestException exception);
```

## Custom Service Methods

When the default CRUD service does not cover all your needs, then it is possible to extend your class with some custom methods as shown below.

```csharp
using ElRaccoone.NestUtilitiesClient;
using ElRaccoone.NestUtilitiesClient.Core;

public class UserService : CrudService<User> {
  public UserService () : base (
    hostname: "my-awesome-api.com",
    resource: "user"
  ) { }

  public RequestBuilder<User> GetUserInfo (string value) =>
    new RequestBuilder<User> (
      requestMiddleware: this.requestMiddleware,
      requestMethod: RequestMethod.Post,
      url: string.Join ("/", this.url, "info", value));

  public RequestBuilder<User> CreateUserInfo () =>
    new RequestBuilder<User> (
      requestMiddleware: this.requestMiddleware,
      requestMethod: RequestMethod.Post,
      url: string.Join ("/", this.url, "info"),
      model: new User ());

  public RequestBuilder<User> GetUserInfo (string email, string password) =>
    new RequestBuilder<User> (
      requestMiddleware: this.requestMiddleware,
      requestMethod: RequestMethod.Post,
      url: string.Join ("/", this.url, "info", value),
      rawBody: JsonUtility.ToJson (new AuthenticationRequest (email, password)));
}

// And user your newly created end-points later on!
public class TestComponent : MonoBehaviour {
  private UserService userService = new UserService ();

  private async void Login () {
    var user = await this.userService.SomethingCustom ("someValue").Send ();
  }
}
```

## Using a different JSON (De)Serializer

Nest Utilities Client for Unity used the built-in JSON (De)Serializer. The module allows for using other (De)Serializer using compiler flags. Keep in mind that these JSON (De)Serializers need to be added to your project manually in order to work.

#### Json dot Net

Add the following compiler flag to your project settings to use the JSON dot Net (De)Serializer.

```
NEST_UTILITIES_CLIENT_USE_JSON_DOT_NET
```
