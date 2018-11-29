using System;
namespace Architecture.Model.CustomExceptions
{
    public class SwitchMissingCaseException : Exception
    {
        public SwitchMissingCaseException()
            : base("Not supported case for switch.")
        {
        }

        public SwitchMissingCaseException(string message)
            : base(message)
        {
        }

        public SwitchMissingCaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
