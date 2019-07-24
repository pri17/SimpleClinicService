namespace SampleService
{
    using Core;
    using SampleBusiness;
    using SampleDataContracts;
    using SampleDomain;
    
    /// <summary>
    /// The sample service.
    /// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SampleService" in code, svc and config file together.
    /// NOTE: In order to launch WCF Test Client for testing this service, please select SampleService.svc or SampleService.svc.cs at the Solution Explorer and start debugging.
    /// </summary>
    public class SampleService : ISampleService
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleService"/> class.
        /// </summary>
        public SampleService()
        {
            this.logger = new Logger();
        }

        /// <summary>
        /// The get sample data.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="SampleDataContract"/>.
        /// </returns>
        public SampleDataContract GetSampleData(int id)
        {
            this.logger.Log("BEGIN - GetSampleData");

            ISampleData domainObject;

            using (var unitOfWork = new UnitOfWork())
            {
                domainObject = new SampleBusiness(unitOfWork).GetSampleData(id);
                unitOfWork.Close();
            }

            var mappedContract = this.MapToDataContract(domainObject);
            return mappedContract;
        }

        /// <summary>
        /// Maps the domain object to the data contract.
        /// </summary>
        /// <param name="domainObject">
        /// The domain object.
        /// </param>
        /// <returns>
        /// The <see cref="SampleDataContract"/>.
        /// </returns>
        private SampleDataContract MapToDataContract(ISampleData domainObject)
        {
            return new SampleDataContract { Id = domainObject.Id, SampleProperty = domainObject.SampleProperty };
        }
    }
}
