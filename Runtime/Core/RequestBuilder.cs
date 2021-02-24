using System;
using System.Collections;
using UnityEngine;

namespace ElRaccoone.NestUtilitiesClient.Core {

  /// 
  public class RequestBuilder<ModelType> {

    /// 
    private RequestHandler<ModelType> requestHandler { get; } = new RequestHandler<ModelType> ();

    /// 
    public RequestBuilder (RequestMethod requestMethod, string url) {
      this.requestHandler.SetRequestMethod (requestMethod: requestMethod);
      this.requestHandler.SetUrl (url: url);
    }

    /// 
    public RequestBuilder (RequestMethod requestMethod, string url, ModelType model) {
      this.requestHandler.SetRequestMethod (requestMethod: requestMethod);
      this.requestHandler.SetUrl (url: url);
      this.requestHandler.SetBody (JsonUtility.ToJson (model));
    }

    /// Sets the authorization header allow the request to authorize itself on
    /// the server.
    public RequestBuilder<ModelType> Authorize (string token) {
      this.requestHandler.AddHeader (name: "Authorization", value: token);
      return this;
    }

    /// This parameter allows you to populate references to other collections in
    /// the response.
    public RequestBuilder<ModelType> Populate (params string[] fields) {
      this.requestHandler.AddQueryParameter (name: "populate", value: string.Join (",", fields));
      return this;
    }

    /// This parameter allows you to filter the response data on one or more 
    /// fields on specific values. You can filter on multiple fields by chaing
    /// the filter method.
    [Obsolete ("The Filter parameter is no longer supported by future Nest Utilities versions.")]
    public RequestBuilder<ModelType> Filter (string field, string value) {
      this.requestHandler.AddQueryParameter (name: $"filter[{field}]", value: value);
      return this;
    }

    /// This parameter allows you to search through all fields of the response 
    /// using the same value. Results matching the value in at least one of the 
    /// fields will be shown in the response.
    [Obsolete ("The Search parameter is no longer supported by future Nest Utilities versions.")]
    public RequestBuilder<ModelType> Search (string query) {
      this.requestHandler.AddQueryParameter (name: "search", value: query);
      return this;
    }

    /// This parameter allows you to search through all fields of the response 
    /// using the same value. Results matching the value in at least one of the 
    /// fields will be shown in the response. You can limit the fields which 
    /// fields in which the algorithm searches using the search scope parameter.
    [Obsolete ("The Search parameter is no longer supported by future Nest Utilities versions.")]
    public RequestBuilder<ModelType> Search (string query, params string[] fields) {
      this.requestHandler.AddQueryParameter (name: "searchScope", value: string.Join (",", fields));
      return this.Search (query);
    }

    /// This parameter allows you to define which fields you want the results to
    /// contain. If one or more fields have been picked for a layer, the 
    /// remaining layers will be omitted from the response. You can deep pick 
    /// fields by separating fields using a dot (f.e. brewers.name).
    public RequestBuilder<ModelType> Pick (params string[] fields) {
      this.requestHandler.AddQueryParameter (name: "pick", value: string.Join (",", fields));
      return this;
    }

    /// This parameter allows you to sort the response data on one or more 
    /// fields in the desired order.
    public RequestBuilder<ModelType> Sort (params string[] fields) {
      this.requestHandler.AddQueryParameter (name: "sort", value: string.Join (",", fields));
      return this;
    }

    /// This parameter allows you to sort the response data on one or more 
    /// fields in the desired order. Define descending in order to by it in 
    /// descending order.
    public RequestBuilder<ModelType> Sort (string field, bool descending) {
      this.requestHandler.AddQueryParameter (name: "sort", value: descending == true ? $"-{field}" : field);
      return this;
    }

    /// This parameter allows you to skip the first n number of results.
    public RequestBuilder<ModelType> Offset (int amount) {
      this.requestHandler.AddQueryParameter (name: "offset", value: $"{amount}");
      return this;
    }

    /// This parameter allows you to limit the response to only show the next n
    /// number of results.
    public RequestBuilder<ModelType> Limit (int amount) {
      this.requestHandler.AddQueryParameter (name: "limit", value: $"{amount}");
      return this;
    }

    /// This parameter allows you to find the distinct values for a specified 
    /// field. The returned models will each contain unique values for that 
    /// field. When multiple models in the actual response would have the same 
    /// value, the first encountered model will be chosen based on the sort 
    /// attribute.
    public RequestBuilder<ModelType> Distinct (string field) {
      this.requestHandler.AddQueryParameter (name: "distinct", value: field);
      return this;
    }

    /// This parameter allows you to randomize the order of the response data. 
    /// This parameter holds priority over the sort parameter which means that 
    /// the sort will be omitted when random is defined.
    public RequestBuilder<ModelType> Random () {
      this.requestHandler.AddQueryParameter (name: "random", value: "true");
      return this;
    }

    /// 
    public IEnumerator Send () {
      yield return this.requestHandler.SendRequest ();
    }

    /// 
    public ModelType GetResponse () {
      if (this.requestHandler.hasError == false)
        return this.requestHandler.responseData;
      throw this.requestHandler.GetException ();
    }

    /// 
    public string GetRawResponse () {
      if (this.requestHandler.hasError == false)
        return this.requestHandler.rawResponseData;
      throw this.requestHandler.GetException ();
    }
  }
}