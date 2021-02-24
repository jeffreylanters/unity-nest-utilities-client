using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// 
  public abstract class RequestMiddleware {

    public class Header {
      public string name { get; set; } = "";
      public string value { get; set; } = "";

      public Header (string name, string value) {
        this.name = name;
        this.value = value;
      }
    }

    public virtual Header[] GetHeaders () =>
      new Header[] { };
  }
}