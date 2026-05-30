using System;

namespace KaraWeb.Core.Models.Jobs
{
    /// <summary>
    /// A background job
    /// </summary>
    public sealed class Job
    {
        /// <summary>
        /// The ID of the job
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// The current status of the job
        /// </summary>
        public JobStatus Status { get; set; }

        /// <summary>
        /// Details about the result
        /// </summary>
        public string ResultMessage { get; set; }
    }
}
