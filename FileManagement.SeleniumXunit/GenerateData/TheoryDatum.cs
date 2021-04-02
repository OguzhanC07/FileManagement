using FileManagement.SeleniumXunit.Utilities;

namespace FileManagement.SeleniumXunit.GenerateData
{
    //https://stackoverflow.com/questions/22093843/pass-complex-parameters-to-theory/36638664#36638664
    public abstract class TheoryDatum : ITheoryDatum
    {
        public abstract object[] ToParameterArray();

        public static ITheoryDatum Factory<TSystemUnderTest>(BrowserType browserType,TSystemUnderTest sut)
        {
            var datum = new TheoryDatum<TSystemUnderTest>
            {
                BrowserType=browserType,
                SystemUnderTest = sut,
            };
            return datum;
        }
    }

    public class TheoryDatum<TSystemUnderTest> : TheoryDatum
    {
        public BrowserType BrowserType { get; set; }

        //you should pass model data in this property for implementation, look authTest
        public TSystemUnderTest SystemUnderTest { get; set; }

        public override object[] ToParameterArray()
        {
            var output = new object[2];
            output[0] = BrowserType;
            output[1] = SystemUnderTest;
            return output;
        }
    }
}
