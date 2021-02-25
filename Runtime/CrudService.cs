using ElRaccoone.NestUtilitiesClient.Core;

namespace ElRaccoone.NestUtilitiesClient {

  /// Abstract base class for every crud service with generic data type. The data
  /// type will be used as the response type of the network requests. Base class
  /// contains a superset of base requests.
  public abstract class CrudService<ModelType> {

    /// 
    private string hostname { get; } = "";

    /// 
    private string resource { get; } = "";

    /// Defines whether the request should use an insecure protocol. Default set
    /// to false. When enabled, the requests will use the HTTP protoc instead of
    /// the secure HTTPS protocol.
    private bool useInsecureProtocol { get; } = false;

    ///
    private RequestMiddleware requestMiddleware { get; } = null;

    /// 
    public string url {
      get => string.Join (
        string.Empty,
        this.useInsecureProtocol == true ? "http" : "https",
        $"://{this.hostname}/{this.resource}");
    }

    /// 
    public CrudService (string hostname, string resource, bool useInsecureProtocol = false, RequestMiddleware requestMiddleware = null) {
      this.hostname = hostname;
      this.resource = resource;
      this.useInsecureProtocol = useInsecureProtocol;
      this.requestMiddleware = requestMiddleware;
    }

    /// 
    public RequestBuilder<ModelType[]> Get () =>
      new RequestBuilder<ModelType[]> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Get,
        url: string.Join ("/", this.url));

    /// 
    public RequestBuilder<ModelType> Get (string id) =>
      new RequestBuilder<ModelType> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Get,
        url: string.Join ("/", this.url, id));

    /// 
    public RequestBuilder<ModelType[]> Get (params string[] ids) =>
      new RequestBuilder<ModelType[]> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Get,
        url: string.Join ("/", this.url, string.Join (",", ids)));

    /// 
    public RequestBuilder<ModelType> Post (ModelType model) =>
      new RequestBuilder<ModelType> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Post,
        url: string.Join ("/", this.url),
        model: model);

    /// 
    public RequestBuilder<ModelType> Put (ModelType model) =>
      new RequestBuilder<ModelType> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Put,
        url: string.Join ("/", this.url),
        model: model);

    /// 
    public RequestBuilder<ModelType> Delete (string id) =>
      new RequestBuilder<ModelType> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Delete,
        url: string.Join ("/", this.url, id));
  }
}