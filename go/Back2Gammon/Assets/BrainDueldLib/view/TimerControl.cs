
using System.Collections.Generic; using System;

using System.Text;
using System.Threading;
using BrainDuelsLib.delegates;

namespace BrainDuelsLib.view
{
    public abstract class TimerControl : Control
    {
        protected int interval;
		protected BrainDuelsLib.delegates.Action onTime;
        public TimerControl(int interval)
        {
            this.interval = interval;
        }
		public abstract void Start(BrainDuelsLib.delegates.Action onTime);
        public abstract void Stop();
    }
}
