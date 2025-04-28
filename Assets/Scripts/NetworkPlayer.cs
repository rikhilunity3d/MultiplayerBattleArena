using Photon.Pun;
using UnityEngine;
using TMPro;


public class NetworkPlayer : MonoBehaviourPun
{
    public TextMeshProUGUI nicknameText;
    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        if (photonView.IsMine)
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        }
        nicknameText.text = photonView.Owner.NickName;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return; // Only control my own player!

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * 5f * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 10f);
        }

    }



    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;
        Debug.Log(photonView.Owner.NickName + " took " + damage + " damage. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
