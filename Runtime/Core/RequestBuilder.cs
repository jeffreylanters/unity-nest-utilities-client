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

    ///
    public RequestBuilder<ModelType> Authorize (string token) {
      this.requestHandler.AddHeader (name: "Authorization", value: token);
      return this;
    }

    ///
    public RequestBuilder<ModelType> Authenticate (string token) {
      this.requestHandler.AddHeader (name: "Authentication", value: token);
      return this;
    }

    /// 
    public RequestBuilder<ModelType> Populate (params string[] fields) {
      this.requestHandler.AddQueryParameter (name: "populate", value: string.Join (",", fields));
      return this;
    }

    /// 
    public RequestBuilder<ModelType> Limit (int limit) {
      this.requestHandler.AddQueryParameter (name: "limit", value: limit.ToString ());
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