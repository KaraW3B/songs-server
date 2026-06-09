using KaraWeb.Shared.Exceptions;
using KaraWeb.Shared.Models.Libraries;
using KaraWeb.Shared.Models.Songs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.SDK.Helpers;
using KaraWeb.Shared.Helpers;

namespace KaraWeb.SDK.Connectors.Collections
{
    internal sealed class LibrariesConnector : ILibrariesConnector
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _baseUri;

        public LibrariesConnector(HttpClient httpClient, Uri baseUri)
        {
            _httpClient = httpClient;
            _baseUri = baseUri.AppendPath("libraries");
        }

        public async IAsyncEnumerable<LibraryDto> GetLibrariesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_baseUri, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new KaraWebException(
                    $"Unable to get libraries: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var libraries = JsonSerializer.DeserializeAsyncEnumerable<LibraryDto>(
                responseStream, JsonHelper.DefaultJsonSerializerOptions, cancellationToken);
            await foreach (var library in libraries)
            {
                yield return library;
            }
        }

        public async Task<LibraryDto> GetLibraryAsync(Guid libraryId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_baseUri.AppendPath(libraryId.ToString()), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new KaraWebException(
                    $"Unable to get library: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<LibraryDto>(
                responseStream, JsonHelper.DefaultJsonSerializerOptions, cancellationToken);
        }

        public async IAsyncEnumerable<SongDto> GetSongsAsync(Guid libraryId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_baseUri.AppendPath($"{libraryId}/songs"), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new KaraWebException(
                    $"Unable to get songs for library: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var songs = JsonSerializer.DeserializeAsyncEnumerable<SongDto>(
                responseStream, JsonHelper.DefaultJsonSerializerOptions, cancellationToken);
            await foreach (var song in songs)
            {
                yield return song;
            }
        }
    }
}
