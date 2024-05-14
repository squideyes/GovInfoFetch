// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using GovInfoFetch.Helpers;

var client = new GovTrackClient();

await client.GetMembersAsync(0);

Console.ReadKey();