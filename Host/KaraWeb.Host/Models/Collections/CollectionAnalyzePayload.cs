using KaraWeb.Core.Models.Collections;

namespace KaraWeb.Host.Models.Collections
{
    /// <summary>
    /// The payload to set analyze options
    /// </summary>
    public class CollectionAnalyzePayload
    {
        /// <summary>
        /// The type of analyze you want to start
        /// </summary>
        public CollectionAnalyzeType AnalyzeType { get; set; }
    }
}
