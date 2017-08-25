using System;
using System.Collections.Generic;
using System.Text;
using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using BrainDuelsLib.view;

namespace BrainDuelsLib.widgets
{
    public class ChatWidget : Widget
    {
        private Server.TokenAndId tai;
        private ChatSocketThreadedJob ctj;

        public class ChatWidgetCallbackStore : CallbackStore
        {
            public BrainDuelsLib.delegates.Action<int, string> onMessageReceived;
        }

        public class ChatWidgetControlsStore : CallbackStore
        {
            public TimerControl timer;
        }

        private ChatWidgetControlsStore controlsStore = new ChatWidgetControlsStore();
        public ChatWidgetControlsStore Controls
        {
            get
            {
                return controlsStore;
            }
        }

        private ChatWidgetCallbackStore store = new ChatWidgetCallbackStore();

        public ChatWidgetCallbackStore Callbacks
        {
            get
            {
                return store;
            }
        }
 

        public ChatWidget(Server.TokenAndId tai)
            : base()
        {
            this.tai = tai;
        }

        public override void Go()
        {
            base.Go();
            ctj = new ChatSocketThreadedJob(tai);
            ctj.Start();
            Controls.timer.Start(OnTimer);
            ctj.SetMessageReceivedCallback(MessageReceived);
        }

        private List<KeyValuePair<int, string>> receivedMessages = new List<KeyValuePair<int, string>>();

        public void MessageReceived(int receiver, string message)
        {
            receivedMessages.Add(new KeyValuePair<int, string>(receiver, message));
        }

        public void SendMessage(int receiver, string message)
        {
            ctj.SendMessage(receiver, message);
        }

        public void OnTimer()
        {
            for (int i = 0; i<receivedMessages.Count; i++)
            {
                KeyValuePair<int, string> rm = receivedMessages[i];
                Callbacks.onMessageReceived(rm.Key, rm.Value);
            }
            receivedMessages = new List<KeyValuePair<int, string>>();
        }

        public void Stop()
        {
            ctj.Stop();
            this.Controls.timer.Stop();
        }
    }
}
