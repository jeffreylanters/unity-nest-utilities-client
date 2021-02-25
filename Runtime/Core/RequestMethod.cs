namespace ElRaccoone.NestUtilitiesClient.Core {

  /// Defines the method or verb a request will be send as.
  public enum RequestMethod {

    /// Sends the request using a GET verb.
    Get = 1,

    /// Sends the request using a POST verb.
    Post = 2,

    /// Sends the request using a PATCH verb.
    Patch = 3,

    /// Sends the request using a PUT verb.
    Put = 4,

    /// Sends the request using a DELETE verb.
    Delete = 5,
  }
}