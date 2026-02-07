using Microsoft.Extensions.Options;

namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Default implementation of <see cref="IDistributedLockKeyNormalizer"/>.
    /// </summary>
    public class DistributedLockKeyNormalizer : IDistributedLockKeyNormalizer
    {
        /// <summary>
        /// Gets the distributed lock options.
        /// </summary>
        protected DistributedLockOptions Options { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedLockKeyNormalizer"/> class.
        /// </summary>
        /// <param name="options">The distributed lock options</param>
        public DistributedLockKeyNormalizer(
            IOptions<DistributedLockOptions> options)
        {
            Options = options.Value;
        }

        /// <summary>
        /// Normalizes the lock key by adding prefix.
        /// </summary>
        /// <param name="name">The lock name</param>
        /// <returns>Normalized key</returns>
        public virtual string NormalizeKey(string name)
        {
            return $"{Options.KeyPrefix}{name}";
        }
    }
}
