// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace GovInfoFetch.Models;

public class Member
{
    public required string Address { get; init; }
    public required string BioGuideId { get; init; }
    public required DateOnly Birthday { get; init; }
    public Uri? ContactUri { get; init; }
    public int? District { get; init; }
    public required DateOnly EndDate { get; init; }
    public required string FirstName { get; init; }
    public required Gender Gender { get; init; }
    public required string LastName { get; init; }
    public required Uri GovTrackUri { get; init; }
    public string? MiddleName { get; init; }
    public required string Office { get; init; }
    public required Party Party { get; init; }
    public required string Phone { get; init; }
    public required Role Role { get; init; }
    public Uri? RssUri { get; init; }
    public required DateOnly StartDate { get; init; }
    public required string State { get; init; }
    public required string TwitterId { get; init; }
    public required string Website { get; init; }
    public required string YouTubeId { get; init; }
}