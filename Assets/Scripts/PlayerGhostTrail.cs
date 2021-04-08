using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostTrail : MonoBehaviour
{
    private PlayerMovement player;
    private SpriteRenderer sprite;

    public float alphaDecay = 0.01f;
    public Vector4 ghostColour = new Vector4(169, 38, 113, 0.2f);

    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        player = FindObjectOfType<PlayerMovement>();

        if (player == null)
        {
            return;
        }

        transform.position = player.transform.position;
        transform.localScale = player.transform.localScale;

        sprite.sprite = player.GetComponent<SpriteRenderer>().sprite;

        // Set sprite's shader to a GUI text shader to get silhouette
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default"); // Or whatever sprite shader is being used

        sprite.material.shader = shaderGUItext;
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = ghostColour;
        ghostColour -= new Vector4(0, 0, 0, alphaDecay * Time.deltaTime);

        if (sprite.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
