using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngancheController : MonoBehaviour
{
    public Sprite inactiveSprite;
    public Sprite activeSprite;

    public GameObject player;

    public bool isActive;
    SpriteRenderer thisRenderer;

    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);

        if (isActive)
        thisRenderer.sprite = activeSprite;
        else thisRenderer.sprite = inactiveSprite;
    }
}
