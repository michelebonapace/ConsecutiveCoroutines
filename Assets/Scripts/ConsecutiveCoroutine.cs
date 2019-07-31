using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mich.ConsecutiveCoroutine
{
    enum ActionType {COROUTINE, FUNCTION};

    [System.Serializable]
    class Couple
    {
        public object action;
        public ActionType type;
        public Couple(object action, ActionType type){
            this.action = action;
            this.type = type;
        }
    }



    public class ConsecutiveCoroutine
    {
        private Queue<Couple> actions = new Queue<Couple>();
        private MonoBehaviour invoker;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoker">The object on which the coroutine will be called, ususally "this"</param>
        /// <param name="cyclic"></param>
        public ConsecutiveCoroutine(MonoBehaviour invoker)
        {
            this.invoker = invoker;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoker">The object on which the coroutine will be called, ususally "this"</param>
        /// <param name="cyclic"></param>
        public ConsecutiveCoroutine(MonoBehaviour invoker, IEnumerator[] actions)
        {
            this.invoker = invoker;
            foreach (IEnumerator a in actions)
            {
                Add(a);
            }
        }

        /// <summary>
        /// Add a coroutine at the end of the queue
        /// </summary>
        /// <param name="action"></param>
        public void Add(IEnumerator action)
        {
            actions.Enqueue(new Couple(action, ActionType.COROUTINE));
        }

        /// <summary>
        /// Add a function at the end of the queue, use "delegate {function();"}
        /// </summary>
        /// <param name="action"></param>
        public void Add(UnityAction action)
        {
            actions.Enqueue(new Couple(action, ActionType.FUNCTION));
        }

        /// <summary>
        /// Starts the coroutines one after the other
        /// </summary>
        public void Invoke()
        {
            invoker.StartCoroutine(Begin());
        }

        public IEnumerator InvokeLoop()
        {
            yield return invoker.StartCoroutine(Begin());
        }

        private IEnumerator Begin()
        {
            while (actions.Count > 0)
            {
                Couple couple = actions.Dequeue();
                if (couple.type == ActionType.FUNCTION)
                {
                    ((UnityAction)couple.action).Invoke();
                }
                else if(couple.type == ActionType.COROUTINE)
                {
                    yield return invoker.StartCoroutine((IEnumerator)couple.action);
                }
            }
        }
    }
}
