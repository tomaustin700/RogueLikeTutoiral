using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite DamageSprite;
    public int HP = 4;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        _spriteRenderer.sprite = DamageSprite;
        HP -= loss;

        if (HP <= 0)
            gameObject.SetActive(false);
    }
  
}
