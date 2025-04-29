using Photon.Pun;
using UnityEngine;
using TMPro;


public class NetworkPlayer : MonoBehaviourPun
{
    public TextMeshProUGUI nicknameText;
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject bulletPrefab;

    private Renderer playerRenderer;
    public Transform shootPoint;

    void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>(); // Assuming your model has a MeshRenderer
    }


    private void Start()
    {
        currentHealth = maxHealth;
        if (photonView.IsMine)
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);

            Color randomColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f); // Vibrant random color
            photonView.RPC("SetColor", RpcTarget.AllBuffered, randomColor.r, randomColor.g, randomColor.b);

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

        if (photonView.IsMine && Input.GetMouseButtonDown(0))
        {
            PhotonNetwork.Instantiate("Bullet", shootPoint.position, shootPoint.rotation);
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

    [PunRPC]
void SetColor(float r, float g, float b)
{
    if (playerRenderer != null)
    {
        Color newColor = new Color(r, g, b);
        playerRenderer.material.color = newColor;
    }
}


    void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
