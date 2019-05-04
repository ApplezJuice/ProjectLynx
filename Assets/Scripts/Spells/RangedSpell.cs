using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpell : MonoBehaviour
{
    [SerializeField]
    public Character player;
    [SerializeField]
    public GameObject playerGameObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator CastTimer()
    {
        yield return new WaitForSeconds(2);
        player.isCasting = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        playerGameObj = GameObject.FindGameObjectWithTag("Player");

        if (player.isCasting)
        {
            StartCoroutine(CastTimer());
            transform.position = playerGameObj.transform.position;
        }
        transform.Translate(new Vector3(1, 0, 0) * 15f * Time.deltaTime);
        //transform.Translate(new Vector3(1,0,0) * 10f * Time.deltaTime);
    }
}
