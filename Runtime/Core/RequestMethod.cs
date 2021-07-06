namespace ElRaccoone.NestUtilitiesClient.Core {

  /// <summary>
  /// Defines the method or verb a request will be send as.
  /// </summary>
  public enum RequestMethod {

    /// <summary>
    /// Sends the request using a GET verb.
    /// </summary>
    Get = 1,

    /// <summary>
    /// Sends the request using a POST verb.
    /// </summary>
    Post = 2,

    /// <summary>
    /// Sends the request using a PATCH verb.
    /// </summary>
    Patch = 3,

    /// <summary>
    /// Sends the request using a PUT verb.
    /// </summary>
    Put = 4,

    /// <summary>
    /// Sends the request using a DELETE verb.
    /// </summary>
    Delete = 5,
  }
}