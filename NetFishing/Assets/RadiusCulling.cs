using UnityEngine;

public class RadiusCulling : MonoBehaviour
{
    public float renderRadius = 50f; // Define the radius
    private Transform player;

    void Start()
    {
        player = transform.parent; // Assuming this script is on the sphere
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsCullable(other.gameObject))
        {
            other.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsCullable(other.gameObject))
        {
            other.gameObject.SetActive(false);
        }
    }

    bool IsCullable(GameObject obj)
    {
        // Optionally tag or layer objects to manage rendering
        return obj.CompareTag("Cullable");
    }
}
