using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float transSpeed;
    [SerializeField] private float rotSpeed;

    private void FixedUpdate()
    {
        handleTrans();
        handleRot();
    }

    private void handleTrans()
    {
        var targetPos = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPos, transSpeed * Time.deltaTime);
    }

    private void handleRot()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
    }
}
