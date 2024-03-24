namespace Mollie.Api.Models {
    public class TestmodeModel {
        public static TestmodeModel? Create(bool testmode) {
            return testmode ? new TestmodeModel { Testmode = testmode } : null;
        }
        
        /// <summary>
        ///	Oauth only - Optional
        /// </summary>
        public bool? Testmode { get; set; }
    }
}