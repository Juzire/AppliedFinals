using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public EnhancedMeshGenerator meshGen;
    public Mesh enemyMesh;
    public Material enemyMaterial;

    private class EnemyData
    {
        public Vector3 position;
        public float speed;
        public float direction = 1f; 
        public float leftBound;
        public float rightBound;
    }

    private List<EnemyData> enemies = new List<EnemyData>();

    void Start()
    {
        GenerateEnemies();
    }

    void GenerateEnemies()
    {
        enemies.Clear();

        foreach (var mat in meshGen.platformMatrices)
        {
            Vector3 pos = mat.GetPosition();
            if (Random.Range(0f, 1f) < 0.3f)
            {
                EnemyData e = new EnemyData();
                e.position = new Vector3(pos.x, pos.y + 1f, pos.z); 
                e.speed = Random.Range(2f, 4f);
                float halfWidth = meshGen.platformWidth / 2f;
                e.leftBound = pos.x - halfWidth + 0.2f; 
                e.rightBound = pos.x + halfWidth - 0.2f;
                enemies.Add(e);
            }
        }
    }

    public bool CheckHit(Vector3 bulletPos)
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(bulletPos, enemies[i].position) < 0.5f)
            {
                enemies.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        foreach (var e in enemies)
        {
            e.position.x += e.speed * e.direction * Time.deltaTime;
            if (e.position.x > e.rightBound)
            {
                e.position.x = e.rightBound;
                e.direction = -1f;
            }
            else if (e.position.x < e.leftBound)
            {
                e.position.x = e.leftBound;
                e.direction = 1f;
            }
        }
    }

    void OnRenderObject()
    {
        if (enemies.Count == 0) return;

        List<Matrix4x4> mats = new List<Matrix4x4>();
        foreach (var e in enemies)
            mats.Add(Matrix4x4.TRS(e.position, Quaternion.identity, Vector3.one));

        Graphics.DrawMeshInstanced(enemyMesh, 0, enemyMaterial, mats);
    }

    public bool CheckPlayerCollision(Vector3 playerPos, float radius = 0.5f)
    {
        foreach (var e in enemies)
        {
            if (Vector3.Distance(playerPos, e.position) < radius)
                return true;
        }
        return false;
    }
}
