using System;
using Itenso.TimePeriod;

namespace SampleDataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The sample data contract.
    /// </summary>
    [DataContract]
    public class SampleDataContract
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sample property.
        /// </summary>
        [DataMember]
        public string SampleProperty { get; set; }
    }

    [DataContract]
    public class MessageContract
    {
        [DataMember]
        public int sequenceId { get; set; }

        [DataMember]
        public int attempts { get; set; }

        [DataMember]
        public  string state { get; set; }

        [DataMember]
        public  int actionAt { get; set; }

        [DataMember]
        public  string version { get; set; }

        [DataMember]
        public  string messageType { get; set; } // messagetype, attempts

        [DataMember]
        public  string messageID { get; set; }

        [DataMember]
        public  string referenceID { get; set; }

        [DataMember]
        public string conversationID { get; set; }

        [DataMember]
        public  string source { get; set; }

        [DataMember]
        public  string destination { get; set; }

        [DataMember]
        public  DateTime createdAt { get; set; }

        [DataMember]
        public  string transportMessageId { get; set; }
    }

}