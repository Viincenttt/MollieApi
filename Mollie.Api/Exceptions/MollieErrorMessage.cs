namespace Mollie.Api.Exceptions
{
    public class MollieErrorMessage
    {
        public string Type { get; set; }

        public string Message { get; set; }

        public string Field { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}