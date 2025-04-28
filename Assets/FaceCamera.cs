using UnityEngine;


// ✅ This will make the nickname always face the player.
public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
