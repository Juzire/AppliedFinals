using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public EnhancedMeshGenerator meshGen;

    private List<Matrix4x4> activeCollectibles = new List<Matrix4x4>();

    void Start()
    {
        activeCollectibles = new List<Matrix4x4>(meshGen.collectibleMatrices);
    }

    void Update()
    {
        if (activeCollectibles.Count > 0)
            Graphics.DrawMeshInstanced(meshGen.collectibleMesh, 0, meshGen.collectibleMaterial, activeCollectibles);
    }

    public bool CheckCollected(Vector3 playerPos)
    {
        bool collected = false;

        for (int i = activeCollectibles.Count - 1; i >= 0; i--)
        {
            Vector3 collectPos = activeCollectibles[i].GetPosition();
            if (Vector3.Distance(playerPos, collectPos) < 0.6f)
            {
                activeCollectibles.RemoveAt(i);
                collected = true;
            }
        }

        return collected;
    }
}
