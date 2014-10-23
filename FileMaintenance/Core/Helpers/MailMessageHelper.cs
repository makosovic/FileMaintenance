using System.Collections.Generic;
using System.Linq;

namespace FileMaintenance.Core.Helpers
{
    public class MailMessageHelper
    {
        /// <summary>
        /// Aggregates emails in a single string using given delimiter.
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string AggregateEmails(IEnumerable<string> emails, char delimiter = ',')
        {
            return emails.Aggregate((a, b) => a + delimiter + b);
        }
    }
}
