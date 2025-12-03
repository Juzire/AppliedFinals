using UnityEngine;
using System.Collections.Generic;

public class PlayerShooter : MonoBehaviour
{
    public EnhancedMeshGenerator meshGen;
    public PowerUpManager powerUpManager;
    public EnemyManager enemyManager;
    public Material bulletMaterial;

    private bool canShoot = false;
    private List<Matrix4x4> bullets = new List<Matrix4x4>();
    private Mesh bulletMesh;

    void Start()
    {
        bulletMesh = new Mesh();
        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(-0.2f,0,-0.2f), new Vector3(0.2f,0,-0.2f),
            new Vector3(0.2f,0,0.2f), new Vector3(-0.2f,0,0.2f),
            new Vector3(-0.2f,0.5f,-0.2f), new Vector3(0.2f,0.5f,-0.2f),
            new Vector3(0.2f,0.5f,0.2f), new Vector3(-0.2f,0.5f,0.2f)
        };
        int[] triangles = new int[36]
        {
            0,4,1,1,4,5,
            2,6,3,3,6,7,
            0,3,4,4,3,7,
            1,5,2,2,5,6,
            0,1,3,3,1,2,
            4,7,5,5,7,6
        };
        bulletMesh.vertices = vertices;
        bulletMesh.triangles = triangles;
        bulletMesh.RecalculateNormals();
    }

    void Update()
    {
        Vector3 playerPos = meshGen.player.position;

        for (int i = meshGen.collectibleMatrices.Count - 1; i >= 0; i--)
        {
            Vector3 collectPos = meshGen.collectibleMatrices[i].GetPosition();
            if (Vector3.Distance(playerPos, collectPos) < 1f)
            {
                meshGen.collectibleMatrices.RemoveAt(i);
                canShoot = true;
                Debug.Log("Power-up collected! Shooting enabled!");
            }
        }

        if (canShoot && Input.GetKeyDown(KeyCode.F))
        {
            Vector3 firePoint = meshGen.player.GetComponent<PlayerController>().GetFirePoint();
            bullets.Add(Matrix4x4.TRS(firePoint, Quaternion.identity, Vector3.one));
        }

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            Matrix4x4 m = bullets[i];
            Vector3 pos = m.GetPosition();
            pos.x += 20f * Time.deltaTime; 
            m.SetColumn(3, new Vector4(pos.x, pos.y, pos.z, 1));
            bullets[i] = m;

            if (enemyManager.CheckHit(pos))
            {
                bullets.RemoveAt(i);
                continue;
            }

            if (pos.x > meshGen.platformTotalLength + 5f)
            {
                bullets.RemoveAt(i);
            }
        }

        if (bullets.Count > 0)
            Graphics.DrawMeshInstanced(bulletMesh, 0, bulletMaterial, bullets.ToArray());
    }
}
