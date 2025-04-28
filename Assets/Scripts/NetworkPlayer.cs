using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviourPun
{
    private void Update()
    {
        if (!photonView.IsMine)
            return; // Only control my own player!

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * 5f * Time.deltaTime);
    }
}
