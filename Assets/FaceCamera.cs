using UnityEngine;


// ✅ This will make the nickname always face the player.
public class FaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
         // Always face the main camera
        if (Camera.main != null)
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
