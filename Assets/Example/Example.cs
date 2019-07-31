using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Mich.ConsecutiveCoroutine
{
    public class Example : MonoBehaviour
    {
        public Text text;
        // Start is called before the first frame update
        void Start()
        {
            //one cycle only
            ConsecutiveCoroutineNormal();

            //loop
            //StartCoroutine(ConsecutiveCoroutineLoop());
        }


        private void ConsecutiveCoroutineNormal()
        {
            ConsecutiveCoroutine cc = new ConsecutiveCoroutine(this);
            //USE DELEGATE TO ADD A FUNCTION
            cc.Add(delegate { ChangeTextColorInstant(Color.blue); });
            //NOTHING TO ADD AN IENUMERATOR
            cc.Add(SetTextAndWait("1"));
            cc.Add(SetTextColorAndWait(Color.red));
            cc.Add(SetTextAndWait("2"));
            cc.Add(SetTextColorAndWait(Color.blue));
            cc.Invoke();
        }


        private IEnumerator ConsecutiveCoroutineLoop()
        {
            while (true)
            {
                ConsecutiveCoroutine cc = new ConsecutiveCoroutine(this);
                //USE DELEGATE TO ADD A FUNCTION
                cc.Add(delegate { ChangeTextColorInstant(Color.blue); });
                //NOTHING TO ADD AN IENUMERATOR
                cc.Add(SetTextAndWait("1"));
                cc.Add(SetTextAndWait("2"));
                cc.Add(SetTextColorAndWait(Color.blue));
                yield return cc.InvokeLoop();
            }
        }

        private void ChangeTextColorInstant(Color c)
        {
            text.color = c;
        }

        private IEnumerator SetTextAndWait(string s)
        {
            text.text = s;
            yield return new WaitForSeconds(1);
        }

        private IEnumerator SetTextColorAndWait(Color c)
        {
            text.color = Color.red;
            yield return new WaitForSeconds(1.5f);
        }
    }

}