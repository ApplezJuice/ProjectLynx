using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpell : MonoBehaviour
{
    [SerializeField]
    public Character player;
    [SerializeField]
    public GameObject playerGameObj;
    [SerializeField]
    public PlayerMovement playerMovement;

    public float castTime = 2f;

    //private bool hasCasted = false;
    public bool needPlayerDir = true;
    private bool playerFacingRight = true;

    public BoxCollider2D collider;
    public LayerMask collisionMask;
    public int horizontalRayCount = 4;

    public int spellID;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    IEnumerator CastTimer()
    {
        yield return new WaitForSeconds(castTime);
        player.isCasting = false;
    }

    void GetPlayerDir() {
        playerFacingRight = playerMovement.isFacingRight;
        needPlayerDir = false;
    }


    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        playerGameObj = GameObject.FindGameObjectWithTag("Player");
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (player.isCasting)
        {
            StartCoroutine(CastTimer());
            transform.position = playerGameObj.transform.position;
        }
        if (needPlayerDir)
        {
            GetPlayerDir();
        }

        if (!playerFacingRight)
        {
            if (transform.localRotation != Quaternion.Euler(0, 180, 0))
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        if (!player.isCasting)
        {
            ShootRay();
        }
        transform.Translate(new Vector3(1, 0, 0) * 15f * Time.deltaTime);
        Destroy(gameObject, 3f);
        //transform.Translate(new Vector3(1,0,0) * 10f * Time.deltaTime);
    }

    void ShootRay()
    {
        Bounds bounds = collider.bounds;
        float rayLength = 1f;
        float directionX = playerFacingRight ? 1 : -1;

        for (int i = 0; i < horizontalRayCount; i++)
        {

            Vector2 rayOrigin = (playerFacingRight) ? new Vector2(bounds.min.x, bounds.min.y + .2f) : new Vector2(bounds.max.x, bounds.min.y + .2f);
            rayOrigin += Vector2.up * (bounds.size.y / (horizontalRayCount - 1)-.15f * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                if (hit.collider.GetComponent<EnemyTest>())
                {
                    hit.collider.GetComponent<EnemyTest>().TakeDamage(player.playerSpellBook.playerOwnedSpells[spellID].getSpellBaseDamage());
                }
                Destroy(gameObject);
            }
        }

    }

}
