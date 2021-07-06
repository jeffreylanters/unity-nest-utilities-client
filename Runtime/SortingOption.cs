using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// <summary>
  /// Flags can be used to alter the behaviour of any Sort request option.
  /// </summary>
  [Flags]
  public enum SortingOption {

    /// <summary>
    //// Sort the data ascendingly from top to bottom.
    /// </summary>
    Ascending = 0,

    /// <summary>
    //// Sort the data ascendingly from bottom to top.
    /// </summary>
    Descending = 1
  }
}