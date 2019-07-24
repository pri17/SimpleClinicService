using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    using System.IO;

    /// <summary>
    /// The logger.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
        {
            if (!Directory.Exists(@"C:\Temp\"))
            {
                Directory.CreateDirectory(@"C:\Temp\");
            }

            if (!File.Exists(@"C:\Temp\Exercise.log"))
            {
                File.CreateText(@"C:\Temp\Exercise.log").Dispose();
            }
        }

        /// <summary>
        /// Logs the message
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Log(string message)
        {
            File.AppendAllText(@"C:\Temp\Exercise.log", string.Format("{0}: {1}\r\n", DateTime.Now, message));
        }
    }
}
