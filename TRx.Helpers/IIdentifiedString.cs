using System;
namespace TRx.Helpers
{
    /// <summary>
    /// Identifier for underlying data
    /// </summary>
    public interface IIdentifiedString
    {
        /// <summary>
        /// Identifier for underlying data
        /// </summary>
        string Id
        {
            get;
            set;
        }
    }
}
