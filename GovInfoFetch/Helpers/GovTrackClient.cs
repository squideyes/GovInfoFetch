// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text.Json;
using System.Text.Json.Serialization;

namespace GovInfoFetch.Helpers;

internal class GovTrackClient
{
    private class Root
    {
        public Meta? Meta { get; set; }

        public Object[]? Objects { get; set; }
    }

    public class Meta
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        [JsonPropertyName("total_count")] public int Total { get; set; }
    }

    public class Object
    {
        public object caucus { get; set; }
        [JsonPropertyName("congress_numbers")] public int[] CongressNumbers { get; set; }
        public bool current { get; set; }
        public string? Address { get; set; }
        public object district { get; set; }
        public string? EndDate { get; set; }
        public Extra? Extra { get; set; }
        [JsonPropertyName("leadership_title")] public object leadership_title { get; set; }
        public string? Party { get; set; }
        public Person? Person { get; set; }
        public string? Phone { get; set; }
        [JsonPropertyName("role_type")] public string? RoleType { get; set; }
        [JsonPropertyName("role_type_label")] public string? RoleTypeLabel { get; set; }
        [JsonPropertyName("senator_class")] public string? SenatorClass { get; set; }
        [JsonPropertyName("senator_class_label")] public string? SenatorClassLabel { get; set; }
        [JsonPropertyName("senator_rank")] public string? SenatorRank { get; set; }
        [JsonPropertyName("senator_rank_label")] public string? SenatorRankLabel { get; set; }
        public string? Startdate { get; set; }
        public string? State { get; set; }
        public string? Title { get; set; }
        [JsonPropertyName("title_long")] public string? TitleLong { get; set; }
        public string? Website { get; set; }
    }

    public class Extra
    {
        public string? Address { get; set; }
        [JsonPropertyName("contact_form")] public string? ContactForm { get; set; }
        public string? Office { get; set; }
        [JsonPropertyName("rss_url")] public string? RssUrl { get; set; }
    }

    public class Person
    {
        public string? BioGuideId { get; set; }
        public string? Birthday { get; set; }
        public int CspanId { get; set; }
        [JsonPropertyName("fediverse_webfinger")] public object fediverse_webfinger { get; set; }
        public string? FirstName { get; set; }
        public string? Gender { get; set; }
        [JsonPropertyName("gender_label")] public string? GenderLabel { get; set; }
        public string? LastName { get; set; }
        public string? Lnk { get; set; }
        public string? MiddleName { get; set; }
        public string? Name { get; set; }
        public string? NameMod { get; set; }
        public string? Nickname { get; set; }
        public string? OsId { get; set; }
        public string? SortName { get; set; }
        public string? TwitterId { get; set; }
        public string? YouTubeId { get; set; }
    }

    private readonly HttpClient httpClient = new();

    public GovTrackClient()
    {
    }

    public async Task GetMembersAsync(int offset)
    {
        var uri = GetUri(offset);

        var json = await httpClient.GetStringAsync(uri);

        var options = GetJsonSerializerOptions();

        var root = JsonSerializer.Deserialize<Root>(json, options);
    }

    private JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private static Uri GetUri(int offset)
    {
        const string BASE_URI = "https://www.govtrack.us";

        return new FluentUrlBuilder(BASE_URI)
            .AppendPathSegment("api")
            .AppendPathSegment("v2")
            .AppendPathSegment("role")
            .WithQueryParam("current", true)
            .WithQueryParam("offset", offset)
            .WithQueryParam("limit", 2)
            .GetUri();
    }

}