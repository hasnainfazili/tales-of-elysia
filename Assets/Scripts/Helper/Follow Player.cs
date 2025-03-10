using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    void LateUpdate()
    {
        transform.position = GameManager.instance.playerTransform.position + offset;
    }
}
