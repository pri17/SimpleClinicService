namespace SampleBusiness
{
    using Core;

    using SampleDomain;

    using SampleRepository;

    /// <summary>
    /// The sample business.
    /// </summary>
    public class SampleBusiness : ISampleBusiness
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleBusiness"/> class.
        /// </summary>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        public SampleBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// The get sample data.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ISampleData"/>.
        /// </returns>
        public ISampleData GetSampleData(int id)
        {

            return new SampleRepository(this.unitOfWork).GetSampleData(id);

        }
    }
}
