namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Interface for normalizing distributed lock keys.
    /// </summary>
    public interface IDistributedLockKeyNormalizer
    {
        /// <summary>
        /// Normalizes the lock key by adding prefix.
        /// </summary>
        /// <param name="name">The lock name</param>
        /// <returns>Normalized key</returns>
        string NormalizeKey(string name);
    }
}
