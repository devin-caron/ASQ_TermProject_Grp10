namespace ATTS.Tests {
    [TestClass]
    public class FlightDataUnitTests {

        FlightData fd = new FlightData("TEST01");

        [TestMethod]
        public void CleanName_CanCleanString() {
            String expectedCleanName = "C-FGAX";
            String exampleDirtyName = @"\\C-FGAX.txt";//taken from an actual run of ATTS w/ dirty params 

            Assert.AreEqual(expectedCleanName, fd.cleanName(exampleDirtyName));
        }
    }
}
