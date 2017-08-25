
using System.Collections;

namespace BrainDuelsLib.threads
{
    public class ThreadedJob
    {
        private bool m_IsDone = false;
        private object m_Handle = new object();
        private System.Threading.Thread thread = null;

        public bool IsDone
        {
            get
            {
                bool tmp;
                lock (m_Handle)
                {
                    tmp = m_IsDone;
                }
                return tmp;
            }
            set
            {
                lock (m_Handle)
                {
                    m_IsDone = value;
                }
            }
        }

        public virtual void Start()
        {
            thread = new System.Threading.Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }

        protected virtual void ThreadFunction() { }

        protected virtual void OnFinished() { }

        public virtual bool Update()
        {
            if (IsDone)
            {
                OnFinished();
                return true;
            }
            return false;
        }


        public void Stop()
        {
            thread.Abort();
        }

        IEnumerator WaitFor()
        {
            while (!Update())
            {
                yield return null;
            }
        }

        private void Run()
        {
            ThreadFunction();
            IsDone = true;
        }
    }
}