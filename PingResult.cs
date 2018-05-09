namespace pingdingding
{
    public class PingResult
    {
        public readonly string Message;
        public readonly bool StateChanged;

        public PingResult(string message, bool stateChanged)
        {
            this.Message = message;
            this.StateChanged = stateChanged;
        }
    }
}