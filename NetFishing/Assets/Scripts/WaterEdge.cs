using UnityEngine;

public class WaterTerrainIntersection : MonoBehaviour
{
    public Transform waterPlane;        // Reference to the water plane (set in inspector)
    public Terrain terrain;             // Reference to the terrain (set in inspector)
    public LineRenderer lineRenderer;   // Reference to the LineRenderer (set in inspector)

   void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        // Set up the LineRenderer
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        // Get the position of the water plane
        float waterHeight = waterPlane.position.y;

        // Define the terrain's width and height
        int terrainWidth = terrain.terrainData.heightmapWidth;
        int terrainHeight = terrain.terrainData.heightmapHeight;

        // Create a list to hold intersection points
        Vector3[] intersectionPoints = new Vector3[terrainWidth];
        int pointCount = 0;

        // Loop through the terrain in X and Z directions to check intersections with the water plane
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int z = 0; z < terrainHeight; z++)
            {
                // Get the world position for the point on the terrain
                float worldX = x * terrain.terrainData.size.x / terrainWidth + terrain.transform.position.x;
                float worldZ = z * terrain.terrainData.size.z / terrainHeight + terrain.transform.position.z;
                float terrainY = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrain.transform.position.y;

                // If the terrain height is near or below the water level, record this as an intersection point
                if (terrainY <= waterHeight)
                {
                    // Store the intersection point in world space
                    Vector3 intersection = new Vector3(worldX, waterHeight, worldZ);
                    intersectionPoints[pointCount++] = intersection;
                }
            }
        }

        // Set the positions for the line renderer
        lineRenderer.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            lineRenderer.SetPosition(i, intersectionPoints[i]);
        }
    }
}