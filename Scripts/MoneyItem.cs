using System.Collections;
using UnityEngine;
using SummerInAustralia;

namespace SummerInAustralia.Scripts
{
    public class MoneyItem : MonoBehaviour
    {
        public Plugin plugin;
        void OnTriggerEnter(Collider other)
        {
            plugin.money += 1;
            GetComponent<AudioSource>().Play();
            GetComponent<Renderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            Invoke("DestroyMoney", 2);
        }
        void DestroyMoney()
        {
            Destroy(gameObject);
        }
    }
}