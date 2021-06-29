using ElRaccoone.NestUtilitiesClient.Core;

namespace ElRaccoone.NestUtilitiesClient {

  /// Abstract base class for every crud service with generic data type. The data
  /// type will be used as the response type of the network requests. Base class
  /// contains a superset of base requests.
  public abstract class CrudService<ModelType> {

    /// The requests's hostname will be used to define the destination.
    public string hostname { get; private set; } = "";

    /// The requests's resource will be used to define the destination.
    public string resource { get; private set; } = "";

    /// Defines whether the request should use an insecure protocol. Default set
    /// to false. When enabled, the requests will use the HTTP protoc instead of
    /// the secure HTTPS protocol.
    public bool useInsecureProtocol { get; private set; } = false;

    /// The request middleware can be set from the initialization of the class.
    /// When set, the middleware will be passed down to the request handlers,
    /// where the actual invoking will happen. Reference may be null.
    public RequestMiddleware requestMiddleware { get; private set; } = null;

    /// The destination of the request, this is a combination of the protocol
    /// preferences, the hostname and the resource of the request.
    public string url {
      get => string.Join (
        string.Empty,
        this.useInsecureProtocol == true ? "http" : "https",
        $"://{this.hostname}/{this.resource}");
    }

    /// Instanciates a new instance of the crud service with a hostname, resource,
    /// an optional flag to use an insecure protocole and optional middleware.
    public CrudService (string hostname, string resource, bool useInsecureProtocol = false, RequestMiddleware requestMiddleware = null) {
      this.hostname = hostname;
      this.resource = resource;
      this.useInsecureProtocol = useInsecureProtocol;
      this.requestMiddleware = requestMiddleware;
    }

    /// Returns a new builder requesting many documents.
    public RequestBuilder<ModelType[]> Read () =>
      new RequestBuilder<ModelType[]> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Get,
        url: string.Join ("/", this.url));

    /// Returns a new builder requesting one or more specific documents.
    public RequestBuilder<ModelType[]> Read (params string[] ids) =>
      new RequestBuilder<ModelType[]> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Get,
        url: string.Join ("/", this.url, string.Join (",", ids)));

    /// Returns a new builder requesting the creation of a document.
    public RequestBuilder<ModelType> Create (ModelType model) =>
      new RequestBuilder<ModelType> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Post,
        url: string.Join ("/", this.url),
        model: model);

    /// Returns a new builder requesting the altering of a document.
    public RequestBuilder<ModelType> Update (ModelType model) =>
      new RequestBuilder<ModelType> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Put,
        url: string.Join ("/", this.url),
        model: model);

    /// Returns a new builder requesting the deletion of a document.
    public RequestBuilder<ModelType> Delete (string id) =>
      new RequestBuilder<ModelType> (
        requestMiddleware: this.requestMiddleware,
        requestMethod: RequestMethod.Delete,
        url: string.Join ("/", this.url, id));
  }
}