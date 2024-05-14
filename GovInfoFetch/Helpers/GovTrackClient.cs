// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using GovInfoFetch.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GovInfoFetch.Helpers;

internal class GovTrackClient
{
    private class Root
    {
        public Meta? Meta { get; set; }
        [JsonPropertyName("objects")] public Item[]? Items { get; set; }
    }

    private class Meta
    {
        public required int Limit { get; init; }
        public required int Offset { get; init; }
        [JsonPropertyName("total_count")] public int Count { get; init; }
    }

    private class Item
    {
        public required Person? Person { get; init; }
        [JsonPropertyName("role_type")] public required Role Role { get; init; }
        public required Extra Extra { get; init; }
        public required Party Party { get; init; }
        public required string State { get; init; }
        public required string Phone { get; init; }
        public required string Website { get; init; }
        public object? District { get; init; }
        public required DateOnly StartDate { get; init; }
        public required DateOnly EndDate { get; init; }
    }

    private class Extra
    {
        public string? Address { get; init; }
        [JsonPropertyName("contact_form")] public string? ContactForm { get; init; }
        public string? Office { get; init; }
        [JsonPropertyName("rss_url")] public string? RssUrl { get; init; }
    }

    private class Person
    {
        public required string BioGuideId { get; init; }
        public required DateOnly Birthday { get; init; }
        public required string FirstName { get; init; }
        public required Gender Gender { get; init; }
        public required string LastName { get; init; }
        public required string Link { get; init; }
        public string? MiddleName { get; init; }
        public required string TwitterId { get; init; }
        public required string YouTubeId { get; init; }
    }

    private readonly HttpClient httpClient = new();

    public GovTrackClient()
    {
    }

    private record Page(int Count, Member[] Members);

    public async Task<Member[]> GetMembersAsync()
    {
        const int LIMIT = 100;

        var members = new List<Member>();

        async Task<Page> GetPageAsync(int offset)
        {
            var uri = GetUri(offset, LIMIT);

            var json = await httpClient.GetStringAsync(uri);

            var options = GetJsonSerializerOptions();

            var root = JsonSerializer.Deserialize<Root>(json, options);

            var page = new List<Member>();

            foreach (var item in root!.Items!)
            {
                members.Add(new Member()
                {
                    Address = item.Extra!.Address!,
                    BioGuideId = item.Person!.BioGuideId!,
                    Birthday = item.Person!.Birthday!,
                    ContactUri = item.Extra?.ContactForm?.Convert(
                        v => v is null ? null : new Uri(v)),
                    District = item!.District.IntOrNull(),
                    EndDate = item!.EndDate,
                    FirstName = item.Person!.FirstName!,
                    Gender = item.Person!.Gender!,
                    GovTrackUri = new Uri(item.Person!.Link!),
                    LastName = item.Person!.LastName!,
                    MiddleName = item.Person!.MiddleName!,
                    Office = item.Extra!.Office!,
                    Party = item.Party!,
                    Phone = item.Phone!,
                    Role = item.Role!,
                    RssUri = item.Extra?.RssUrl?.Convert(
                        v => v is null ? null : new Uri(v)),
                    StartDate = item!.StartDate,
                    State = item!.State,
                    TwitterId = item.Person!.TwitterId!,
                    Website = item.Website!,
                    YouTubeId = item.Person!.YouTubeId!
                });
            }

            return new Page(root.Meta!.Count, [.. page]);
        }

        var offset = 0;

        while (true)
        {
            var page = await GetPageAsync(offset);

            members.AddRange(page.Members);

            offset += LIMIT;

            if (members.Count >= page.Count)
                break;
        }

        return [.. members];
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        options.Converters.Add(new JsonStringEnumConverter());

        return options;
    }

    private static Uri GetUri(int skip, int take)
    {
        const string BASE_URI = "https://www.govtrack.us";

        return new FluentUrlBuilder(BASE_URI)
            .AppendPathSegment("api")
            .AppendPathSegment("v2")
            .AppendPathSegment("role")
            .WithQueryParam("current", true)
            .WithQueryParam("offset", skip)
            .WithQueryParam("limit", take)
            .GetUri();
    }
}