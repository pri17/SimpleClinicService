namespace SampleRepository
{
    using SampleDomain;

    /// <summary>
    /// The SampleRepository interface.
    /// </summary>
    public interface ISampleRepository
    {
        /// <summary>
        /// Get the sample object
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ISampleData"/>.
        /// </returns>
        ISampleData GetSampleData(int id);
    }
}
