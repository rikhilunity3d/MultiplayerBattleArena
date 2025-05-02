using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class NetworkPlayer : MonoBehaviourPun
{
    public TextMeshPro nicknameText; // 3D TextMeshPro label to display player name
    public float maxHealth = 100f;
    private float currentHealth;

    public GameObject bulletPrefab;

    private Renderer playerRenderer;
    public Transform shootPoint;

    public float respawnDelay = 3f;
    private Vector3[] spawnPositions;
    
    public GameObject healthBarPrefab; // Health bar prefab reference
    private Slider healthSlider;
    private GameObject healthBarInstance;

    void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>(); // Assuming your model has a MeshRenderer
    }

    private void Start()
    {
        // Set initial health
        currentHealth = maxHealth;

        // Only run the following code for the local player (photonView.IsMine)
        if (photonView.IsMine)
        {
            // Assign a random nickname to the player
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);

            // Assign a random color to this player, synced over the network
            Color randomColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f); // Vibrant color
            photonView.RPC("SetColor", RpcTarget.AllBuffered, randomColor.r, randomColor.g, randomColor.b);

            // Retrieve spawn points from a parent GameObject called "SpawnPoints"
            Transform[] spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
            spawnPositions = new Vector3[spawnPoints.Length - 1];
            for (int i = 1; i < spawnPoints.Length; i++)
                spawnPositions[i - 1] = spawnPoints[i].position;

            // Instantiate health bar prefab above the player
            GameObject canvas = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            healthBarInstance = canvas;
            canvas.transform.SetParent(transform); // So it moves with the player

            // Get slider component and set initial health values
            healthSlider = canvas.GetComponentInChildren<Slider>();
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
        }

        // Show the nickname for all players (this will run on all clients)
        if (nicknameText != null)
        {
            nicknameText.text = photonView.Owner.NickName;
        }
    }

    private void Update()
    {
        // Only allow movement control for the local player
        if (!photonView.IsMine) return;

        // Player movement control
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        transform.Translate(move * 5f * Time.deltaTime);

        // If the player presses space, take damage for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("TakeDamage", RpcTarget.AllBuffered, 10f);
        }

        // If the player presses left mouse button, instantiate the bullet
        if (photonView.IsMine && Input.GetMouseButtonDown(0))
        {
            PhotonNetwork.Instantiate("Bullet", shootPoint.position, shootPoint.rotation);
        }
    }

    // Take damage method, called by RPC
    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (!photonView.IsMine) return;

        // Apply damage
        currentHealth -= damage;
        Debug.Log(photonView.Owner.NickName + " took " + damage + " damage. Current HP: " + currentHealth);

        // Update the health bar slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // If health reaches 0, trigger respawn
        if (currentHealth <= 0)
        {
            photonView.RPC("DieAndRespawn", RpcTarget.AllBuffered);
        }
    }

    // Set the color for this player's character (syncs over the network)
    [PunRPC]
    void SetColor(float r, float g, float b)
    {
        if (playerRenderer != null)
        {
            Color newColor = new Color(r, g, b);
            playerRenderer.material.color = newColor;
        }
    }

    // Handle player death and respawn
    [PunRPC]
    void DieAndRespawn()
    {
        if (photonView.IsMine)
            StartCoroutine(RespawnRoutine());
    }

    // Coroutine to handle respawn after a delay
    IEnumerator RespawnRoutine()
    {
        // Disable player visuals and collider
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<Renderer>().enabled = false;
        this.enabled = false;

        // Hide health bar during respawn
        if (healthBarInstance != null)
            healthBarInstance.SetActive(false);

        // Wait for respawn delay
        yield return new WaitForSeconds(respawnDelay);

        // Move the player to a random spawn position
        Vector3 respawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
        transform.position = respawnPos;

        // Reset health
        currentHealth = maxHealth;

        // Reactivate player visuals and logic
        GetComponent<Collider>().enabled = true;
        GetComponentInChildren<Renderer>().enabled = true;
        this.enabled = true;

        // Reactivate health bar and reset the slider value
        if (healthBarInstance != null)
        {
            healthBarInstance.SetActive(true);

            if (healthSlider != null)
            {
                healthSlider.value = maxHealth;
            }
        }
    }
}
