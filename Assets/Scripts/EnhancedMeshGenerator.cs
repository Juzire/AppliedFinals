using UnityEngine;
using System.Collections.Generic;

public class EnhancedMeshGenerator : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public PlayerCameraFollow camFollow;

    [Header("Platform Settings")]
    public int platformCount = 50;
    public float platformWidth = 4f;
    public float platformDepth = 4f;
    float minGap = 2f;
    float maxGap = 5f;
    public float maxHeightStep = 1f;

    [Header("Meshes & Materials")]
    public Mesh platformMesh;
    public Material platformMaterial;
    public Mesh collectibleMesh;
    public Material collectibleMaterial;

    public List<Matrix4x4> platformMatrices = new();
    public List<Matrix4x4> collectibleMatrices = new();

    public float platformTotalLength;
    public Vector3 playerPosition;

    void Start()
    {
        GenerateWorld();
        PositionPlayerAtStart();
    }

    void Update()
    {
        playerPosition = player.position;
        camFollow.SetPlayerPosition(playerPosition);
    }

    void GenerateWorld()
    {
        platformMatrices.Clear();
        collectibleMatrices.Clear();

        float currentX = 0f;
        float currentY = 0f;

        for (int i = 0; i < platformCount; i++)
        {
            float gap = Random.Range(minGap, maxGap);
            currentX += gap;

            float heightStep = Random.Range(-maxHeightStep, maxHeightStep);
            currentY += heightStep;
            currentY = Mathf.Clamp(currentY, 0f, 5f);

            Vector3 pos = new Vector3(currentX, currentY, 0);

            platformMatrices.Add(Matrix4x4.TRS(pos, Quaternion.identity, new Vector3(platformWidth, 1f, platformDepth)));

            if (Random.Range(0f, 1f) < 0.4f)
            {
                Vector3 collectPos = new Vector3(pos.x, pos.y + 1.2f, 0);
                collectibleMatrices.Add(Matrix4x4.TRS(collectPos, Quaternion.identity, Vector3.one * 0.6f));
            }
        }

        platformTotalLength = currentX;
    }

    void PositionPlayerAtStart()
    {
        if (platformMatrices.Count == 0) return;
        Vector3 pos = platformMatrices[0].GetPosition();
        pos.y += 1.5f;
        player.position = pos;
        camFollow.SetPlayerPosition(player.position);
    }

    void OnRenderObject()
    {
        if (platformMatrices.Count > 0)
            Graphics.DrawMeshInstanced(platformMesh, 0, platformMaterial, platformMatrices);
        if (collectibleMatrices.Count > 0)
            Graphics.DrawMeshInstanced(collectibleMesh, 0, collectibleMaterial, collectibleMatrices);
    }

    public bool CollectibleAtPosition(Vector3 playerPos, float radius = 0.5f)
    {
        for (int i = collectibleMatrices.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(playerPos, collectibleMatrices[i].GetPosition()) < radius)
            {
                collectibleMatrices.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}
