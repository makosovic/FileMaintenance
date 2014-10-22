using System.Collections.Generic;
using FileMaintenance.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileMaintenance.UnitTest
{
    [TestClass]
    public class MailMessageHelperShould
    {
        [TestMethod]
        public void AggregateEmails()
        {
            ICollection<string> emails = new List<string> { "test@test.com", "test2@test.com", "test3@test.com", "test4@test.com", "test5@test.com" };

            string result = MailMessageHelper.AggregateEmails(emails, ',');
            string expectedResult = "test@test.com,test2@test.com,test3@test.com,test4@test.com,test5@test.com";

            Assert.AreEqual(result, expectedResult);
        }
    }
}
