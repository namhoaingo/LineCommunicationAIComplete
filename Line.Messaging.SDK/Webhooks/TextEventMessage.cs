namespace Line.Messaging.Webhooks
{
    /// <summary>
    /// Message object which contains the text sent from the source.
    /// </summary>
    public class TextEventMessage : EventMessage
    {
        public string Text { get; set;  } // we may remove the set function later

        public TextEventMessage(string id, string text) : base(EventMessageType.Text, id)
        {
            Text = text;
        }
    }
}
