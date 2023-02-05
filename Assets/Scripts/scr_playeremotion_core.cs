using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_playeremotion_core : MonoBehaviour
{
    Image emotionImage;
    PlayerController playerController;

    public Sprite neutral;
    public Sprite Falling;
    public Sprite SuperFalling;
    public Sprite Hardened;
    public Sprite Impact;
    public Sprite LooseThrow;
    public Sprite Release;

    // Start is called before the first frame update
    void Start()
    {
        emotionImage = GetComponent<Image>();
        playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.playerState == PlayerController.PlayerState.Neutral)
        {
            emotionImage.sprite = neutral;
        }
        if(playerController.playerState == PlayerController.PlayerState.Falling)
        {
            emotionImage.sprite = Falling;
        }
        if (playerController.playerState == PlayerController.PlayerState.SuperFalling)
        {
            emotionImage.sprite = SuperFalling;
        }
        if (playerController.playerState == PlayerController.PlayerState.Hardened)
        {
            emotionImage.sprite = Hardened;
        }
        if (playerController.playerState == PlayerController.PlayerState.Impact)
        {
            emotionImage.sprite = Impact;
        }
        if (playerController.playerState == PlayerController.PlayerState.LooseThrow)
        {
            emotionImage.sprite = LooseThrow;
        }
        if (playerController.playerState == PlayerController.PlayerState.Release)
        {
            emotionImage.sprite = Release;
        }

    }
}
