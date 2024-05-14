// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using GovInfoFetch.Helpers;
using System.Text.Json;

var client = new GovTrackClient();

var members = await client.GetMembersAsync();

var json = JsonSerializer.Serialize(members);

File.WriteAllText("Members.json", json);

Console.ReadKey();