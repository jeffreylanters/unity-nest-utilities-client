using System.Collections;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using System;

namespace ElRaccoone.NestUtilitiesClient.Core {

  /// The request handler is responsible for sending the actual request over to
  /// the server and consists of a set of methods allowing for the request to be
  /// build including headers, query parameters and payloads.
  public class RequestHandler<ModelType> : UnityWebRequest {

    /// Defines whether the request did run into an error.
    internal bool hasError { get; private set; } = false;

    /// Defines whether the request did run any response data.
    internal bool hasResponseData { get; private set; } = false;

    /// The request's response.
    internal ModelType responseData { get; private set; } = default;

    /// The raw response data in text format.
    internal string rawResponseData { get; private set; } = "";

    /// Defines whether the request has one or more query parameters.
    private bool hasQueryParameters { get; set; } = false;

    /// Instanciates a new request handler. When the handler is instanciated the
    /// corresponding handlers will be instantiated as well.
    public RequestHandler () {
      this.downloadHandler = new DownloadHandlerBuffer ();
    }

    /// A class to wrap top level JSON arrays. When the Model type is a top level
    /// array, it will be wrapped into this empty model in order for Unity to
    /// parse it contents since top level arrays are not supported in Unity.
    [Serializable]
    private class JsonArrayWrapper<ArrayType> {
      public ArrayType array = default;
    }

    ///
    internal void SetRequestMiddleware (RequestMiddleware requestMiddleware) {
      if (requestMiddleware != null)
        foreach (var _header in requestMiddleware.GetHeaders ())
          this.AddHeader (name: _header.name, value: _header.value);
    }

    /// Adds a header to the request.
    internal void AddHeader (string name, string value) {
      this.SetRequestHeader (name: name, value: value);
    }

    /// Adds a query paramter to the request url.
    internal void AddQueryParameter (string name, string value) {
      this.url += string.Join (string.Empty, this.hasQueryParameters == true ? "&" : "?", name, "=", value);
      this.hasQueryParameters = true;
    }

    /// Sets the target URL of this request.
    internal void SetUrl (string url) {
      this.url = url;
    }

    /// Sets the request method of this request handler. This includes a custom
    /// http method override header required for some API's since Unity might not
    /// send the right Verb.
    internal void SetRequestMethod (RequestMethod requestMethod) {
      this.method = requestMethod.ToString ().ToUpper ();
      this.AddHeader (name: "X-HTTP-Method-Override", value: requestMethod.ToString ().ToUpper ());
    }

    /// Sets the body of the request as a raw string, the content type will be
    /// marked as JSON using the Application/JSON flag in the headers.
    internal void SetModel (ModelType bodyData) {
      var _bodyDataBytes = Encoding.ASCII.GetBytes (s: JsonUtility.ToJson (bodyData));
      this.uploadHandler = new UploadHandlerRaw (data: _bodyDataBytes);
      this.uploadHandler.contentType = "application/json";
    }

    ///
    internal IEnumerator SendRequest () {
      yield return this.SendWebRequest ();
      this.hasError = this.responseCode >= 400 || this.responseCode == 0;
      this.rawResponseData = this.downloadHandler.text;
      this.hasResponseData = this.rawResponseData.Trim ().Length > 0;
      if (this.hasError == false && this.hasResponseData == true)
        this.responseData = typeof (ModelType).IsArray ?
          JsonUtility.FromJson<JsonArrayWrapper<ModelType>> (json: $"{{\"array\":{this.rawResponseData}}}").array :
          JsonUtility.FromJson<ModelType> (json: this.rawResponseData);
    }

    /// 
    internal RequestException GetException () {
      return new RequestException (
        statusCode: this.responseCode,
        message: this.error,
        url: this.url
      );
    }
  }
}
