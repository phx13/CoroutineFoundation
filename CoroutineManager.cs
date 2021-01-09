using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoroutineFoundation
{
    public class CoroutineManager
    {
        private static CoroutineManager m_Instance = null;

        public static CoroutineManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new CoroutineManager();

                return m_Instance;
            }
        }

        private LinkedList<CoroutineBase> m_CoroutineList = new LinkedList<CoroutineBase>();

        private LinkedList<CoroutineBase> m_ExitCoroutineList = new LinkedList<CoroutineBase>();

        public CoroutineBase Start(IEnumerator iEnumerator)
        {
            CoroutineBase coroutine = new CoroutineBase(iEnumerator);
            m_CoroutineList.AddLast(coroutine);
            return coroutine;
        }

        public void Stop(IEnumerator iEnumerator)
        {

        }

        public void Stop(CoroutineBase coroutine)
        {
            m_ExitCoroutineList.AddLast(coroutine);
        }

        public void UpdateCoroutine()
        {
            LinkedListNode<CoroutineBase> node = m_CoroutineList.First;
            while (node != null)
            {
                CoroutineBase coroutine = node.Value;

                bool result = false;
                if (coroutine != null)
                {
                    bool isStop = m_ExitCoroutineList.Contains(coroutine);
                    if (!isStop)
                    {
                        result = coroutine.MoveNext();
                    }
                }

                if (!result)
                {
                    m_CoroutineList.Remove(node);
                }

                node = node.Next;
            }
        }
    }
}
