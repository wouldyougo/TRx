namespace TRx.Base
{
    /// <summary>
    /// Initialize()
    /// SetupStrategy(string[] args)
    /// </summary>
    public interface ISetupStrategy
    {
        void Initialize();
        void SetupStrategy(string[] args);
    }
}