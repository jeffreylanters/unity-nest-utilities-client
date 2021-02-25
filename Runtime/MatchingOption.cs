using System;

namespace ElRaccoone.NestUtilitiesClient {

  /// Flags can be used to alter the behaviour of any Match request option.
  [Flags]
  public enum MatchingOption {

    /// With this flag the search looks for all matches, without it – only the 
    /// first match is returned.
    Global = 0,

    /// The IgnoreCase option, or the i inline option, provides case-insensitive
    /// matching. By default, the casing conventions of the current culture are 
    /// used.
    CaseInsensitive = 1,

    /// Multiline mode. Changes the meaning of ^ and $ so they match at the 
    /// beginning and end, respectively, of any line, and not just the beginning
    /// and end of the entire string.
    MultiLine = 2,

    /// Specifies single-line mode. Changes the meaning of the dot (.) so it
    /// matches every character (instead of every character except \n).
    SingleLine = 3,

    /// Enables full Unicode support. The flag enables correct processing of
    /// surrogate pairs.
    Unicode = 4,
  }
}