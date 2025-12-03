using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    public float jumpForce = 10f;

    private float verticalVelocity = 0f;
    private bool canJump = true;

    [Header("Stats")]
    public int maxHP = 100;
    public int currentHP;
    public int lives = 3;

    [Header("References")]
    public EnhancedMeshGenerator meshGen;
    public EnemyManager enemyManager;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float fallMultiplier = 2.5f;

    [Header("Firepoint")]
    public float fireHeightOffset = 0.5f;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(h * speed * Time.deltaTime, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            verticalVelocity = jumpForce;
            canJump = false;
        }

        if (verticalVelocity > 0f)
            verticalVelocity += gravity * Time.deltaTime; 
        else
            verticalVelocity += gravity * fallMultiplier * Time.deltaTime;

        transform.position += new Vector3(0, verticalVelocity * Time.deltaTime, 0);
        float lowestPlatformY = 0f;
        foreach (var mat in meshGen.platformMatrices)
        {
            Vector3 platPos = mat.GetPosition();
            float halfWidth = meshGen.platformWidth / 2f;
            if (transform.position.x >= platPos.x - halfWidth && transform.position.x <= platPos.x + halfWidth)
            {
                float top = platPos.y + 0.5f;
                if (transform.position.y <= top)
                    lowestPlatformY = Mathf.Max(lowestPlatformY, top);
            }
        }

        if (transform.position.y <= lowestPlatformY)
        {
            transform.position = new Vector3(transform.position.x, lowestPlatformY, transform.position.z);
            verticalVelocity = 0f;
            canJump = true;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        CheckEnemyCollision();
    }

    void CheckEnemyCollision()
    {
        if (enemyManager != null)
        {
            if (enemyManager.CheckPlayerCollision(transform.position))
            {
                TakeDamage(10);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            lives--;
            if (lives > 0)
            {
                Vector3 startPos = meshGen.platformMatrices[0].GetPosition();
                transform.position = new Vector3(startPos.x, startPos.y + 0.55f, 0);
                verticalVelocity = 0f;
                canJump = true;
                currentHP = maxHP;
            }
            else
            {
                Debug.Log("Game Over!");
            }
        }
    }

    public Vector3 GetFirePoint()
    {
        return transform.position + Vector3.up * fireHeightOffset;
    }
}
