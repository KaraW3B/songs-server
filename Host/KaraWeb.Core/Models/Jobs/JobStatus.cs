namespace KaraWeb.Core.Models.Jobs
{
    /// <summary>
    /// The status of a job
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// The job is currently in the queue for execution
        /// </summary>
        Pending,
        /// <summary>
        /// The job is currently running
        /// </summary>
        Processing,
        /// <summary>
        /// The job ended with success
        /// </summary>
        Success,
        /// <summary>
        /// The job ended with error
        /// </summary>
        Error
    }
}
