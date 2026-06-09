using KaraWeb.SDK.Connectors.Collections;
using KaraWeb.SDK.Connectors.Songs;

namespace KaraWeb.SDK.Connectors
{
    public interface IKaraWebConnector
    {
        ILibrariesConnector Libraries { get; }
        ISongsConnector Songs { get; }
    }
}
