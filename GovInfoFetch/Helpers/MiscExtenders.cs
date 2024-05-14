// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text.Json;

namespace GovInfoFetch.Helpers;

internal static class MiscExtenders
{
    public static R Convert<T, R>(this T value, Func<T, R> convert) => convert(value);

    public static int? IntOrNull(this object? value) =>
        value is null ? null : ((JsonElement)value).GetInt32();
}