using System.Collections.Generic;
using System.Linq;

namespace FileMaintenance.Core.Helpers
{
    public class MailMessageHelper
    {
        public static string AggregateEmails(IEnumerable<string> emails, char delimiter = ',')
        {
            return emails.Aggregate((a, b) => a + delimiter + b);
        }
    }
}
