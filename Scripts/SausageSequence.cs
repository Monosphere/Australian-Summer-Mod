using System.Collections;
using UnityEngine;

namespace SummerInAustralia.Scripts
{
    public class SausageSequence : MonoBehaviour
    {
        //in life, you're either a GameObject.Find person, or a transform.GetChild() person, i am a getchild person but i dont think that was a good move, now my code looks like Fettuccine. I am sorry to whoever is reading this source code, maybe you wanted to make sosig cooking aswell only to come across my code encrypted with .GetChild()
        bool isStarted = false;
        public GameObject grill;

        public float progress;

        public Transform knob;
        Transform sausagesParent;

        // lol sausage sequence
        void Start()
        {
            knob = transform.GetChild(0);
            sausagesParent = grill.transform.GetChild(7);
        }

        void Update()
        {
            if(isStarted)
            {
                if (progress > 359)
                {
                    Debug.Log("bruh you burnt it you idot");
                    isStarted = false;
                }
                else
                {
                    knob.localEulerAngles += new Vector3(0, 0, Time.deltaTime * 5);
                    progress += Time.deltaTime * 5;
                }
                if(progress > 120 && progress < 140)
                {
                    sausagesParent.GetChild(1).gameObject.SetActive(true);
                    if(sausagesParent.GetChild(0).gameObject.activeSelf)
                    {
                        sausagesParent.GetChild(0).gameObject.SetActive(false);
                    }
                }
                if (progress > 140 && progress < 259)
                {
                    sausagesParent.GetChild(1).GetComponent<Renderer>().material.color = new Color(1 - progress / 360, 0, 0);
                }
            }
        }
        public void StartSequence()
        {
            isStarted = true;
            grill.transform.GetChild(4).GetComponent<ParticleSystem>().Play();
            grill.transform.GetChild(4).GetComponent<AudioSource>().Play();
            sausagesParent.GetChild(0).gameObject.SetActive(true);
        }
        public void EndSequence()
        {
            if(grill.transform.GetChild(4).GetComponent<ParticleSystem>().isPlaying)
            {
                grill.transform.GetChild(4).GetComponent<ParticleSystem>().Stop();
                grill.transform.GetChild(4).GetComponent<AudioSource>().Stop();
            }
            if (sausagesParent.GetChild(0).gameObject != null && sausagesParent.GetChild(1).gameObject != null)
            {
                sausagesParent.GetChild(0).gameObject.SetActive(false);
                sausagesParent.GetChild(1).gameObject.SetActive(false);
            }
            isStarted = false;
        }
    }
}