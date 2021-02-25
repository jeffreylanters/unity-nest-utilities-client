using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// 
  public class RequestException : Exception {

    /// 
    public long statusCode { get; } = 0;

    ///
    public string message { get; } = "";

    ///
    public string url { get; } = "";

    ///
    public string rawResponseData { get; } = "";

    /// 
    public RequestException (long statusCode, string message, string url, string rawResponseData) {
      this.statusCode = statusCode;
      this.message = message;
      this.url = url;
      this.rawResponseData = rawResponseData;
    }

    ///
    public override string ToString () {
      return $"Request Exception with Status Code {this.statusCode} while sending to {this.url} {this.message}\n{this.rawResponseData}";
    }
  }
}