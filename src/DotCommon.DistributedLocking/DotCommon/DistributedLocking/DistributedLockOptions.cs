namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Options for distributed locking.
    /// </summary>
    public class DistributedLockOptions
    {
        /// <summary>
        /// Key prefix for all locks.
        /// Default: "DistributedLock:"
        /// </summary>
        public string KeyPrefix { get; set; } = "DistributedLock:";
    }
}
