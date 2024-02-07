using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChildType
{
    Basic,
    Sword,
    Hammer,
    
}
public class Basic_Child : MonoBehaviour
{
    public ChildType CType;
    public GameObject ChildDeathPrefab;

    private bool isSantaPresent = false;

    public int ChildHealth = 15;
    public int AttackDamage;
    public float MovementSpeed = 0.4f;
    public float OriginalSpeed = 0.4f;

    public float AttackInterval = 1f;
    public float AttackTimer = 0;

    public bool AttackAutoMode = false;

    // ���� ������ ������ ����
    private Color originalColor;

    // SpriteRenderer ������Ʈ�� ������ ����
    private SpriteRenderer spriteRenderer;

    // ChildHealth�� ���� ���� ������ ����
    private float previousChildHealth;

    public AudioSource AttackSound;

    public void Init()
    {
        ChildHealth = 15;
        AttackTimer = AttackInterval;
    }

    void Start()
    {
        // SpriteRenderer ������Ʈ ��������
        spriteRenderer = GetComponent<SpriteRenderer>();
        // ���� ���� ����
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        Vector2 dir = Vector2.left;
        transform.position += (Vector3)(dir * MovementSpeed) * Time.deltaTime;
        dir = dir.normalized;

        if (ChildHealth <= 0)
        {
            Death();
        }

        if (!isSantaPresent)
        {
            MovementSpeed = OriginalSpeed;
        }
        
        if (CType == ChildType.Basic || CType == ChildType.Hammer)
        {
            AttackDamage = 1;
        }
        else if (CType == ChildType.Sword)
        {
            AttackDamage = 2;
        }
        AttackInterval = Mathf.Max(0.5f, AttackInterval);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Arrow")
        {
            Attack arrow = collision.GetComponent<Attack>();
            if(arrow.AType == AttackType.Arrow)
            {
                ChildHealth -= 1;
            }
            else if(arrow.AType == AttackType.Bullet)
            {
                ChildHealth -= 2;
            }
            arrow.gameObject.SetActive(false);

           
            // �ǰ� �� ���� ����
            StartCoroutine(FlashRed());
            AttackSound.Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        AttackTimer -= Time.deltaTime;
        if (collision.tag == "Santa")
        {
            MovementSpeed -= 0.4f;
            Santa santa = collision.GetComponent<Santa>();
            isSantaPresent = true;
            if (AttackTimer <= 0f)
            {
                AttackTimer = AttackInterval;
                santa.SantaHealth -= AttackDamage;
                Debug.Log($"��Ÿü��:{santa.SantaHealth}");
            }
            if (santa.SType == SantaType.Sword && ChildHealth < previousChildHealth)
            {
                StartCoroutine(FlashRed());
                AttackSound.Play();
            }
        }
        // ChildHealth ���� ���� ������ ����
        previousChildHealth = ChildHealth;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Santa")
        {
            isSantaPresent = false;
        }
    }

    private void Death()
    {
        spriteRenderer.color = originalColor;
        Instantiate(ChildDeathPrefab, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
    // ���������� �����̴� Coroutine

    IEnumerator FlashRed()
    {
        // ���������� ����
        spriteRenderer.color = Color.red;
        // 0.2�� ���
        yield return new WaitForSeconds(0.2f);
        // ���� �������� ����
        spriteRenderer.color = originalColor;
    }

}