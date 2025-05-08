namespace ProductClientHub.Communication.Responses
{
    public class ResponseErrorManagerJson
    {
        public List<string> Errors { get; private set; }

        public ResponseErrorManagerJson(string message)
        {
            Errors = [message];
        }

        public ResponseErrorManagerJson(List<string> messages)
        {
            Errors = messages;
        }
    }
}
