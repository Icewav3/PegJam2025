using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeySceneGod : MonoBehaviour
{
    private void Awake()
    {
        SceneGod.SInstance.player = gameObject;
    }
}