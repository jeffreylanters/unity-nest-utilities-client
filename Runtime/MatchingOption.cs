using System;

namespace ElRaccoone.NestUtilitiesClient {
  /// 
  [Flags]
  public enum MatchingOption {

    ///
    Global = 0,

    /// 
    CaseInsensitive = 1,

    /// 
    MultiLine = 2,

    /// 
    SingleLine = 3,

    /// 
    Unicode = 4,

    /// 
    Sticky = 5,
  }
}