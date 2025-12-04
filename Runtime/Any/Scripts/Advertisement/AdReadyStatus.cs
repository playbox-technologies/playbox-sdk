namespace Playbox
{
    /// <summary>
    /// <param name="Ready">
    /// The commercials are loaded.
    /// </param>>
    /// <param name="NotReady">
    /// The ads are not loaded.
    /// </param>>
    /// <param name="NullStatus">
    /// The unitId of the advertisement is equal to Null.
    /// </param>>
    /// <param name="NotInitialized">
    /// MaxSdk is not initialized.
    /// </param>>
    /// </summary>
    public enum AdReadyStatus
    {
        Ready,
        NotReady,
        NullStatus,
        NotInitialized
    }
}