using UnityEngine;
using UnityEngine.InputSystem;
public class CrosshairController : MonoBehaviour
{
    public Transform player;
    public float orbitDistance = 2f;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - player.position).normalized;
        transform.position = player.position + direction * orbitDistance;
    }
}
