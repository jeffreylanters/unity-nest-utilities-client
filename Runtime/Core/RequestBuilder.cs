using System.Collections;
using System.Threading.Tasks;

namespace ElRaccoone.NestUtilitiesClient.Core {

  /// <summary>
  /// The request builder is responsible for instanciating and providing data to
  /// the request handler. The builder consists of a set of chainable methods which
  /// can be used to construct requests.
  /// </summary>
  /// <typeparam name="ModelType">Type of in and outgoing models.</typeparam>
  public class RequestBuilder<ModelType> {

    /// <summary>
    /// The request handler is responsible for making the actual request.
    /// </summary>
    private RequestHandler<ModelType> requestHandler { get; } = new RequestHandler<ModelType> ();

    /// <summary>
    /// The request middleware can be set from the initialization of the class.
    /// When set, it's overwritten methods will be invoked when the request is
    /// being or has been sendt. Reference may be null.
    /// </summary>
    private RequestMiddleware requestMiddleware { get; } = null;

    /// <summary>
    /// Instanciates a new request builder with provided (nullable) request
    /// middleware, a request method and url which will all be passed down to 
    /// the request handler.
    /// </summary>
    /// <param name="requestMiddleware">The request middleware.</param>
    /// <param name="requestMethod">The request method.</param>
    /// <param name="url">The destination URL.</param>
    public RequestBuilder (RequestMiddleware requestMiddleware, RequestMethod requestMethod, string url) {
      this.requestMiddleware = requestMiddleware;
      this.requestHandler.SetRequestMiddleware (requestMiddleware: requestMiddleware);
      this.requestHandler.SetRequestMethod (requestMethod: requestMethod);
      this.requestHandler.SetUrl (url: url);
    }

    /// <summary>
    /// Instanciates a new request builder with provided (nullable) request
    /// middleware, a request method, url en model as the request body which 
    /// will all be passed down to the request handler.
    /// </summary>
    /// <param name="requestMiddleware">The request middleware.</param>
    /// <param name="requestMethod">The request method.</param>
    /// <param name="url">The destination URL.</param>
    /// <param name="model">The request body model.</param>
    public RequestBuilder (RequestMiddleware requestMiddleware, RequestMethod requestMethod, string url, ModelType model) {
      this.requestMiddleware = requestMiddleware;
      this.requestHandler.SetRequestMiddleware (requestMiddleware: requestMiddleware);
      this.requestHandler.SetRequestMethod (requestMethod: requestMethod);
      this.requestHandler.SetUrl (url: url);
      this.requestHandler.SetModel (model);
    }

    /// <summary>
    /// Instanciates a new request builder with provided (nullable) request
    /// middleware, a request method, url and raw body as the request body which 
    /// will all be passed down to the request handler.
    /// </summary>
    /// <param name="requestMiddleware">The request middleware.</param>
    /// <param name="requestMethod">The request method.</param>
    /// <param name="url">The destination URL.</param>
    /// <param name="rawBody">The raw request body.</param>
    public RequestBuilder (RequestMiddleware requestMiddleware, RequestMethod requestMethod, string url, string rawBody) {
      this.requestMiddleware = requestMiddleware;
      this.requestHandler.SetRequestMiddleware (requestMiddleware: requestMiddleware);
      this.requestHandler.SetRequestMethod (requestMethod: requestMethod);
      this.requestHandler.SetUrl (url: url);
      this.requestHandler.SetRawBody (rawBody);
    }

    /// <summary>
    /// This parameter allows you to populate references to other collections in
    /// the response.
    /// </summary>
    /// <param name="fields">The fields which should be populated.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Populate (params string[] fields) {
      foreach (var _field in fields)
        this.requestHandler.AddQueryParameter (name: "populate[]", value: _field);
      return this;
    }

    /// <summary>
    /// This parameter allows you to define which fields you want the results to
    /// contain. If one or more fields have been selected for a layer, the 
    /// remaining layers will be omitted from the response. You can deep select 
    /// fields by separating fields using a dot (f.e. brewers.name).
    /// </summary>
    /// <param name="fields">The fields which should be selected.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Select (params string[] fields) {
      foreach (var _field in fields)
        this.requestHandler.AddQueryParameter (name: "select[]", value: _field);
      return this;
    }

    /// <summary>
    /// This parameter allows you to sort the response data on one or more 
    /// fields in the desired order.
    /// </summary>
    /// <param name="fields">The fields which should be sorted.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Sort (params string[] fields) {
      foreach (var _field in fields)
        this.requestHandler.AddQueryParameter (name: "sort[]", value: _field);
      return this;
    }

    /// <summary>
    /// This parameter allows you to sort the response data on one or more 
    /// fields in the desired order. Define one or more sorting options.
    /// </summary>
    /// <param name="field">The fields which should be sorted.</param>
    /// <param name="sortingOptions">The sorting options.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Sort (string field, SortingOption sortingOptions) {
      if (sortingOptions.HasFlag (SortingOption.Descending) == true)
        field = $"-{field}";
      this.requestHandler.AddQueryParameter (name: "sort[]", value: field);
      return this;
    }

    /// <summary>
    /// This parameter allows you to match the response data on a specific 
    /// exact value on a specific field including the casing.
    /// </summary>
    /// <param name="field">The field which should be matched.</param>
    /// <param name="value">The value to match the field against.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> MatchExact (string field, string value) {
      this.requestHandler.AddQueryParameter (name: $"match[{field}]", value: value);
      return this;
    }

    /// <summary>
    /// This parameter allows you to match the response data based on a regex
    /// filter on a specific field.
    /// </summary>
    /// <param name="field">The field which should be matched.</param>
    /// <param name="value">The regex to match the field against.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> MatchRegex (string field, string value) {
      this.requestHandler.AddQueryParameter (name: $"match[{field}][$regex]", value: value);
      return this;
    }

    /// <summary>
    /// This parameter allows you to match the response data based on a regex
    /// filter on a specific field. One or more matching options will be 
    /// translated into regex options.
    /// </summary>
    /// <param name="field">The field which should be matched.</param>
    /// <param name="value">The regex to match the field against.</param>
    /// <param name="matchingOptions">The regex options.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> MatchRegex (string field, string value, MatchingOption matchingOptions) {
      var _regexOptions = "";
      if (matchingOptions.HasFlag (MatchingOption.Global) == true)
        _regexOptions += "g";
      if (matchingOptions.HasFlag (MatchingOption.CaseInsensitive) == true)
        _regexOptions += "i";
      if (matchingOptions.HasFlag (MatchingOption.MultiLine) == true)
        _regexOptions += "m";
      if (matchingOptions.HasFlag (MatchingOption.SingleLine) == true)
        _regexOptions += "s";
      if (matchingOptions.HasFlag (MatchingOption.Unicode) == true)
        _regexOptions += "u";
      this.requestHandler.AddQueryParameter (name: $"match[{field}][$options]", value: _regexOptions);
      this.requestHandler.AddQueryParameter (name: $"match[{field}][$regex]", value: value);
      return this;
    }

    /// <summary>
    /// This parameter allows you to skip the first n number of results.
    /// </summary>
    /// <param name="amount">The number of skips.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Offset (int amount) {
      this.requestHandler.AddQueryParameter (name: "offset", value: $"{amount}");
      return this;
    }

    /// <summary>
    /// This parameter allows you to limit the response to only show the next n
    /// number of results.
    /// </summary>
    /// <param name="amount">The amount of results.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Limit (int amount) {
      this.requestHandler.AddQueryParameter (name: "limit", value: $"{amount}");
      return this;
    }

    /// <summary>
    /// This parameter allows you to find the distinct values for a specified 
    /// field. The returned models will each contain unique values for that 
    /// field. When multiple models in the actual response would have the same 
    /// value, the first encountered model will be chosen based on the sort 
    /// attribute.
    /// </summary>
    /// <param name="field">The field which should be distincted.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Distinct (string field) {
      this.requestHandler.AddQueryParameter (name: "distinct", value: field);
      return this;
    }

    /// <summary>
    /// This parameter allows you to randomize the order of the response data. 
    /// This parameter holds priority over the sort parameter which means that 
    /// the sort will be omitted when random is defined.
    /// </summary>
    /// <returns>The request builder.</returns>
    public RequestBuilder<ModelType> Random () {
      this.requestHandler.AddQueryParameter (name: "random", value: "true");
      return this;
    }

    /// <summary>
    /// Makes the actual request to the server. Yielded by the request handler,
    /// the enumerator ends when the request was send. When something went wrong
    /// while fetching the data, an exception will be thrown. When the request
    /// middleware is defined, these methods will also be invoked.
    /// </summary>
    /// <returns>The response.</returns>
    public async Task<ModelType> Send () {
      var _didCoroutineComplete = false;
      if (this.requestMiddleware != null)
        foreach (var _header in this.requestMiddleware.OnGetHeaders ())
          this.requestHandler.AddHeader (name: _header.name, value: _header.value);
      RoutineTicker.StartCompletableCoroutine (
        this.requestHandler.StartSendingCoroutine (),
        () => _didCoroutineComplete = true);
      while (_didCoroutineComplete == false)
        await Task.Yield ();
      if (this.requestHandler.hasError == true) {
        var _exception = this.requestHandler.GetException ();
        if (this.requestMiddleware != null)
          this.requestMiddleware.OnRequestDidCatch (_exception);
        throw _exception;
      }
      return this.requestHandler.responseData;
    }
  }
}