using Completed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject soundManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance == null)

            Instantiate(gameManager);

        if (SoundManager.instance == null)
            Instantiate(soundManager);
    }

}
