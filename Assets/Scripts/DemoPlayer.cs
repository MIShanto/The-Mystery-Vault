using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Rigidbody2D playerRigidBody;
    private Vector2 movePos = new Vector2();

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movePos.x = Input.GetAxisRaw(AllString.HORIZONTAL);
        movePos.y = Input.GetAxisRaw(AllString.VERTICAL);
    }

    private void FixedUpdate()
    {
        playerRigidBody.velocity = movePos * movementSpeed;
    }
}
