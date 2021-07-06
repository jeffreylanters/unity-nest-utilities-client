using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// <summary>
  /// Flags can be used to alter the behaviour of any Match request option.
  /// </summary>
  [Flags]
  public enum MatchingOption {

    /// <summary>
    /// With this flag the search looks for all matches, without it – only the 
    /// first match is returned.
    /// </summary>
    Global = 0,

    /// <summary>
    /// The IgnoreCase option, or the i inline option, provides case-insensitive
    /// matching. By default, the casing conventions of the current culture are 
    /// used.
    /// </summary>
    CaseInsensitive = 1,

    /// <summary>
    /// Multiline mode. Changes the meaning of ^ and $ so they match at the 
    /// beginning and end, respectively, of any line, and not just the beginning
    /// and end of the entire string.
    /// </summary>
    MultiLine = 2,

    /// <summary>
    /// Specifies single-line mode. Changes the meaning of the dot (.) so it
    /// matches every character (instead of every character except \n).
    /// </summary>
    SingleLine = 3,

    /// <summary>
    /// Enables full Unicode support. The flag enables correct processing of
    /// surrogate pairs.
    /// </summary>
    Unicode = 4,
  }
}