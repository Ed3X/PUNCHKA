using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;

    private void Awake()
    {
        // Find or add a NavMeshSurface component to this GameObject
        navMeshSurface = GetComponent<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        }
    }

    private void Start()
    {
        // Bake the NavMesh when the game starts
        BakeNavMesh();
    }

    public void BakeNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface component is missing.");
        }
    }
}
