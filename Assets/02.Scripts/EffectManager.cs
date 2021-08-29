using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    GameObject hitEffect;


    public void SpawnHitEffect( Transform playerPosition ,Vector3 HitOffest)
    {
        if (hitEffect != null)
        {
            float direction = playerPosition.localScale.x > 0 ? -1f : 1f;
            // Skull1 -> back x : 0.5224, y : -1.2569 | front x : -0.49, y : -0.76
            // Skull2 -> front x :-0.465, y : -0.885
            Vector3 effectPoint = playerPosition.position + new Vector3(HitOffest.x * direction, HitOffest.y, 0f);
            GameObject newhitEffect = Instantiate(hitEffect, effectPoint, Quaternion.identity) as GameObject;
            newhitEffect.transform.localScale = new Vector3(0.48f * direction * -1 , 0.48f, 1f);
        }
    }
}
