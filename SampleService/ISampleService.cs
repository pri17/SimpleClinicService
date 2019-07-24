namespace SampleService
{
    using System.ServiceModel;
    using SampleDataContracts;

    /// <summary>
    /// The SampleService interface.
    /// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISampleService" in both code and config file together.
    /// </summary>
    [ServiceContract]
    public interface ISampleService
    {
        /// <summary>
        /// The get sample data.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="SampleDataContract"/>.
        /// </returns>
        [OperationContract]
        SampleDataContract GetSampleData(int id);


    }
}
