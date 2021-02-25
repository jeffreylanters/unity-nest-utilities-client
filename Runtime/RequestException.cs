using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// Request exceptions can be thrown when getting the response of a request
  /// which did error or when providing middleware which implements the
  /// onRequestDidCatch method.
  public class RequestException : Exception {

    /// The status code of the request.
    public long statusCode { get; } = 0;

    /// A human readable error message containing details about the problem.
    public string message { get; } = "";

    /// The URL of which the request was trying to reach.
    public string url { get; } = "";

    /// The raw response data of the catched request, may contain information.
    public string rawResponseData { get; } = "";

    /// Instanciates a new request exception which may be thrown.
    public RequestException (long statusCode, string message, string url, string rawResponseData) {
      this.statusCode = statusCode;
      this.message = message;
      this.url = url;
      this.rawResponseData = rawResponseData;
    }

    /// Turns the request exception into a readable error.
    public override string ToString () {
      return $"Request Exception with Status Code {this.statusCode} while sending to {this.url} {this.message}\n{this.rawResponseData}";
    }
  }
}