using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// Flags can be used to alter the behaviour of any Sort request option.
  [Flags]
  public enum SortingOption {

    ///
    Ascending = 0,

    /// 
    Descending = 1
  }
}