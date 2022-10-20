using System.Collections;
using UnityEngine;

namespace SummerInAustralia.Scripts
{
    public class FacePlayer : MonoBehaviour
    {
        GameObject face;
        // Use this for initialization
        void Start() => face = new GameObject();

        // Update is called once per frame
        void Update()
        {
            if (GorillaLocomotion.Player.Instance == null)
                return;

            face.transform.LookAt(GorillaLocomotion.Player.Instance.bodyCollider.transform);
            transform.eulerAngles = new Vector3(0,face.transform.eulerAngles.y + 180,0);
        }
    }
}