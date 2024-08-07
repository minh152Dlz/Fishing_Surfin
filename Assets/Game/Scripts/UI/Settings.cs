using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public AudioSource bgsoundGame;
    public void ActiveSound(bool isActive)
    {
        bgsoundGame.enabled = isActive;
    }
}
