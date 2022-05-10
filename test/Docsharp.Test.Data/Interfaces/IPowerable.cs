using Docsharp.Test.Data.Enumerations;

namespace Docsharp.Test.Data.Interfaces
{
    /// <summary>
    /// Represents an entity that features engine power.
    /// </summary>
    public interface IPowerable
    {
        /// <summary>
        /// Size of a boat's engine(s).
        /// </summary>
        public EngineSize Engine { get; set; }
        /// <summary>
        /// Quantity of engines.
        /// </summary>
        public int EngineCount { get; set; }
    }
}
