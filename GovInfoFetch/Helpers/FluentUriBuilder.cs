// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text;

namespace GovInfoFetch.Helpers;

internal class FluentUrlBuilder
{
    private readonly List<string> segments = [];
    private readonly Dictionary<string, string?> queryParams = new();

    private readonly Uri baseUri;

    public FluentUrlBuilder(string uriString)
        : this(new Uri(uriString))
    {
    }

    public FluentUrlBuilder(Uri baseUri)
    {
        this.baseUri = new Uri(baseUri.GetLeftPart(UriPartial.Authority));

        segments.AddRange(baseUri.LocalPath.Split('/')
            .Where(s => !string.IsNullOrWhiteSpace(s)));
    }

    public FluentUrlBuilder AppendPathSegment<T>(T segment)
    {
        segments.Add(segment!.ToString()!);

        return this;
    }

    public FluentUrlBuilder WithQueryParam(string token, bool value)
    {
        queryParams.Add(token, value!.ToString().ToLower());

        return this;
    }

    public FluentUrlBuilder WithQueryParam<T>(string token, T value)
    {
        queryParams.Add(token, value!.ToString());

        return this;
    }

    public Uri GetUri()
    {
        var sb = new StringBuilder();

        sb.Append(baseUri.AbsoluteUri);
        sb.Append(string.Join("/", segments));

        int count = 0;

        foreach (var key in queryParams.Keys)
        {
            sb.Append(count++ == 0 ? '?' : '&');
            sb.Append(key);

            if (queryParams[key] != null)
            {
                sb.Append('=');
                sb.Append(queryParams[key]);
            }
        }

        return new Uri(sb.ToString());
    }
}