using System;
using System.Collections;

namespace CoroutineFoundation
{
    public sealed class CoroutineBase
    {
        private IEnumerator m_Coroutine;

        public CoroutineBase(IEnumerator coroutine)
        {
            m_Coroutine = coroutine;
        }

        public bool MoveNext()
        {
            if (m_Coroutine == null)
                return false;

            IWait iwait = m_Coroutine.Current as IWait;
            bool isMoveNext = true;
            if (iwait != null)
            {
                isMoveNext = iwait.Tick();
            }

            if (!isMoveNext)
            {
                return true;
            }
            else
            {
                return m_Coroutine.MoveNext();
            }
        }

        public void Stop()
        {
            m_Coroutine = null;
        }
    }
}
