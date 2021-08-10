using System.Collections;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using System;

#if NEST_UTILITIES_CLIENT_USE_JSON_DOT_NET
// If this compiler flag is provided, we're going to import the Json dot Net 
// module to serialize and deserialize the raw response data.
using Newtonsoft.Json;
#endif

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

    /// <summary>
    /// The request middleware can be set from the initialization of the class.
    /// When set, it's overwritten methods will be invoked when the request is
    /// being or has been sendt. Reference may be null.
    /// </summary>
    private RequestMiddleware requestMiddleware { get; set; } = null;

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
    internal void SetModel (ModelType model) {
#if NEST_UTILITIES_CLIENT_USE_JSON_DOT_NET
      // If this compiler flag is provided, we're going to use the Json dot Net
      // module to serialize the model.
      JsonConvert.SerializeObject (value: model)
#else
      // If no specific deserializer compiler flag is provided, we're going to
      // use the built-in serializer by Unity to serialize the model.
      this.SetRawBody (data: JsonUtility.ToJson (model));
#endif
    }

    /// Sets the body of the request as a raw string, the content type will be
    /// marked as JSON using the Application/JSON flag in the headers.
    internal void SetRawBody (string data) {
      var _dataBytes = Encoding.ASCII.GetBytes (s: data);
      this.uploadHandler = new UploadHandlerRaw (data: _dataBytes);
      this.uploadHandler.contentType = "application/json";
    }

    /// Sets the requestMiddleware for this requestHandler to use when creating
    /// request or manipulating data.
    internal void SetRequestMiddleware (RequestMiddleware requestMiddleware) {
      this.requestMiddleware = requestMiddleware;
    }

    /// Sends and yields the actual request. Both hasError, hasResponseData and 
    /// the raw response data will always be set when invoking. If no error did
    /// occur and response has body, the response data will be set.
    internal IEnumerator StartSendingCoroutine () {
      yield return this.SendWebRequest ();
      this.hasError = this.responseCode >= 400 || this.responseCode == 0;
      this.rawResponseData = this.downloadHandler.text;
      this.hasResponseData = this.rawResponseData.Trim ().Length > 0;
      if (this.hasError == false && this.hasResponseData == true)
#if NEST_UTILITIES_CLIENT_USE_JSON_DOT_NET
        // If this compiler flag is provided, we're going to use the Json dot 
        // Net module to deserialize the raw response data.
        this.responseData = typeof (ModelType).IsArray ?
          JsonConvert.DeserializeObject<JSONArrayWrapper<ResponseType>> (value: $"{{\"array\":{this.rawResponseData}}}").array :
          JsonConvert.DeserializeObject<ResponseType> (value: this.rawResponseData);
#else
        // If no specific deserializer compiler flag is provided, we're going to
        // use the built-in deserializer by Unity to deserialize the raw 
        // response data.
        this.responseData = typeof (ModelType).IsArray ?
          JsonUtility.FromJson<JsonArrayWrapper<ModelType>> (json: $"{{\"array\":{this.rawResponseData}}}").array :
          JsonUtility.FromJson<ModelType> (json: this.rawResponseData);
#endif
    }

    /// Returns a request exception containing meta data about the request.
    internal RequestException GetException () {
      return new RequestException (
        statusCode: (int)this.responseCode,
        rawResponseData: this.rawResponseData,
        url: this.url
      );
    }
  }
}
