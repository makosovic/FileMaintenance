using FileMaintenance.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileMaintenance.UnitTest
{
    [TestClass]
    public class ConfigurationHelperShould
    {
        [TestMethod]
        public void ParseKeepFor()
        {
            string keepFor = "365.12:45";
            int days, hours, minutes;

            ConfigurationHelper.TryParseKeepFor(keepFor, out days, out hours, out minutes);

            Assert.AreEqual(days, 365);
            Assert.AreEqual(hours, 12);
            Assert.AreEqual(minutes, 45);
        }
    }
}
