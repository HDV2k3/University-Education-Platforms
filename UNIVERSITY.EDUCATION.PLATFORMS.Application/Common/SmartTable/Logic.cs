namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable
{
    public class Logic
    {
        private Logic(string value) { Value = value; }

        public string Value { get; set; }

        public static Logic or { get { return new Logic("or"); } }

        public static Logic and { get { return new Logic("and"); } }
    }
}
