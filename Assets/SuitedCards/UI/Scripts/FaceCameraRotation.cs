using UnityEngine;

public class FaceCameraRotation : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
    
}
