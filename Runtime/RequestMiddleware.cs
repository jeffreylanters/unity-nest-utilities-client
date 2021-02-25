namespace ElRaccoone.NestUtilitiesClient {

  /// Extendable middleware can be provided to any service which implements
  /// the crudservice class. The middleware consists of a set of virtual
  /// methods which can be implemented.
  public abstract class RequestMiddleware {

    /// A request header which can be used for interception via middleware. The
    /// header class consists of a name and value.
    public class Header {

      /// The header's name.
      internal string name { get; set; } = "";

      /// The header's value.
      internal string value { get; set; } = "";

      /// Instanciates a new header consisting of a name and value.
      public Header (string name, string value) {
        this.name = name;
        this.value = value;
      }
    }

    /// Implement this virtual method in order to intercept the headers of every
    /// request made by this service.
    public virtual Header[] OnGetHeaders () => new Header[] { };

    /// Implement this virtual method in order to catch every request which did
    /// run into an HTTP error.
    public virtual void OnRequestDidCatch (RequestException exception) { }
  }
}