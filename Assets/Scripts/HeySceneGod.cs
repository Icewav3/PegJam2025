using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeySceneGod : MonoBehaviour
{
    private void Start()
    {
        SceneGod.SInstance.player = gameObject;
    }
}