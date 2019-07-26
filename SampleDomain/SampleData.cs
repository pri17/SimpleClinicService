using System;
using System.Security.Cryptography;
using Itenso.TimePeriod;

namespace SampleDomain
{
    /// <summary>
    /// The sample data.
    /// </summary>
    public class SampleData : ISampleData
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the sample property.
        /// </summary>
        public virtual string SampleProperty { get; set; }
    }

    public class MHSMessage
    {
        public virtual int SequenceID { get; set; }
        public virtual string State { get; set; }
        public virtual int Attempts { get; set; }
        public virtual int ActionAt { get; set; }
        public virtual string Version { get; set; }
        public virtual string MessageType { get; set; } // messagetype, attempts

        public virtual string MessageID { get; set; }
        public virtual string ReferenceID { get; set; }
        public virtual string ConversationID { get; set; }
        public virtual string Source { get; set; }
        public virtual string Destination { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string TransportMessageId { get; set; }

    }

}
