namespace SampleBusiness
{
    using SampleDomain;

    /// <summary>
    /// The SampleBusiness interface.
    /// </summary>
    public interface ISampleBusiness
    {
        /// <summary>
        /// Get the sample data.
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
