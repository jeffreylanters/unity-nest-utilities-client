﻿using System;

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
    public RequestException (long statusCode, string message, string url) {
      this.statusCode = statusCode;
      this.message = message;
      this.url = url;
    }
  }
}