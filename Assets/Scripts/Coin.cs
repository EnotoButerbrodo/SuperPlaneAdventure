using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Coin : MonoBehaviour
{   
    public AudioClip pickupAudio;
    private bool IsPickuped = false;
    [SerializeField] private float RotationSpeed = 180f;
    // Update is called once per frame

    private void Update()
    {
       MoveAndRotate();
    }

    private void MoveAndRotate(){
        //Вращение вокрус y оси
        transform.Rotate(0, 0,  RotationSpeed * Time.deltaTime);
        
        //Перемещение вверх вниз по оси y
        var _newPosition = transform.position;
        _newPosition.y += 0.004f * Mathf.Sin((Mathf.PI) * Time.time + transform.position.x);

        transform.position = _newPosition;
    }

    private void OnTriggerEnter(Collider collider) {
       
    }
}
