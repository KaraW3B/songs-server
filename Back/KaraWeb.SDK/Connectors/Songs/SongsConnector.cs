using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using KaraWeb.SDK.Helpers;
using KaraWeb.Shared.Exceptions;
using KaraWeb.Shared.Helpers;
using KaraWeb.Shared.Models.Songs;
using KaraWeb.Shared.Models.Songs.Files;
using KaraWeb.Shared.Models.Songs.Messages;
using KaraWeb.Shared.Models.Songs.Notes;

namespace KaraWeb.SDK.Connectors.Songs
{
    internal sealed class SongsConnector : ISongsConnector
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _baseUri;

        public SongsConnector(HttpClient httpClient, Uri baseUri)
        {
            _httpClient = httpClient;
            _baseUri = baseUri.AppendPath("songs");
        }

        public async Task<SongDto> GetSongAsync(Guid songId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(_baseUri.AppendPath($"{songId}"), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new KaraWebException(
                    $"Unable to get song details: {await response.Content.ReadAsStringAsync()}");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<SongDto>(responseStream
                , JsonHelper.DefaultJsonSerializerOptions, cancellationToken);
        }

        public async IAsyncEnumerable<SongNoteDto> GetSongNotesAsync(Guid songId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(_baseUri.AppendPath($"{songId}/notes"), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new KaraWebException(
                    $"Unable to get song details: {await response.Content.ReadAsStringAsync()}");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            await foreach (var note in JsonSerializer.DeserializeAsyncEnumerable<SongNoteDto>(responseStream
                         , JsonHelper.DefaultJsonSerializerOptions, cancellationToken))
            {
                yield return note;
            }
        }

        public async IAsyncEnumerable<SongAlertDto> GetSongAlertsAsync(Guid songId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(_baseUri.AppendPath($"{songId}/alerts"), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new KaraWebException(
                    $"Unable to get song details: {await response.Content.ReadAsStringAsync()}");
            }

            using var responseStream = await response.Content.ReadAsStreamAsync();
            await foreach (var alert in JsonSerializer.DeserializeAsyncEnumerable<SongAlertDto>(responseStream
                               , JsonHelper.DefaultJsonSerializerOptions, cancellationToken))
            {
                yield return alert;
            }
        }

        public async Task<Stream> GetSongFileStreamAsync(Guid songId, FileType fileType,
            CancellationToken cancellationToken)
        {
            var response =
                await _httpClient.GetAsync(_baseUri.AppendPath($"{songId}/streams/{fileType}"), cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new KaraWebException(
                    $"Unable to get song file stream {fileType}: {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsStreamAsync();
        }
    }
}