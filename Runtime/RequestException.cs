using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// Request exceptions can be thrown when getting the response of a request
  /// which did error or when providing middleware which implements the
  /// onRequestDidCatch method.
  public class RequestException : Exception {

    /// The status code of the request.
    public long statusCode { get; } = 0;

    /// The URL of which the request was trying to reach.
    public string url { get; } = "";

    /// The raw response data of the catched request, may contain information.
    public string rawResponseData { get; } = "";

    /// The definition of a status code contains a human readable notation of 
    /// the servers response status code.
    public StatusCodeDefinition statusCodeDefinition { get; } = StatusCodeDefinition.Undefined;

    /// Instanciates a new request exception which may be thrown.
    public RequestException (int statusCode, string rawResponseData, string url) {
      this.statusCode = statusCode;
      if (StatusCodeDefinition.IsDefined (typeof (StatusCodeDefinition), statusCode))
        this.statusCodeDefinition = (StatusCodeDefinition)statusCode;
      this.rawResponseData = rawResponseData;
      this.url = url;
    }

    /// Turns the request exception into a readable error.
    public override string ToString () {
      return $"Request Exception server responded {this.statusCodeDefinition} {this.statusCode} while sending to {this.url}\n{this.rawResponseData}";
    }
  }
}