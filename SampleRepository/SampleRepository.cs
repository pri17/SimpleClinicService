namespace SampleRepository
{
    using Core;
    
    using SampleDomain;

    /// <summary>
    /// The sample repository.
    /// </summary>
    public class SampleRepository
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        public SampleRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get sample object.
        /// </summary>
        /// <param name="id">
        /// The object id.
        /// </param>
        /// <returns>
        /// The <see cref="ISampleData"/>.
        /// </returns>
        public ISampleData GetSampleData(int id)
        {
            return this.unitOfWork.Session.Get<SampleData>(id);
        }
    }
}
