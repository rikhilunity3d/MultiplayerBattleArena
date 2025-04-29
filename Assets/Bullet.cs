using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        NetworkPlayer hitPlayer = other.GetComponent<NetworkPlayer>();
        if (hitPlayer != null && hitPlayer.photonView != photonView)
        {
            hitPlayer.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
            PhotonNetwork.Destroy(gameObject);
        }

    }
}
