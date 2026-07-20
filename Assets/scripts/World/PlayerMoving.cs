using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoving : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private Vector2 moveInput;
    public float moveSpeed = 5f;

    // Update is called once per frame
    void OnMove(InputValue value)
{
    moveInput = value.Get<Vector2>();
}

    private void Update()
    {
        Vector3 move = new Vector3(
            moveInput.x,
            moveInput.y,
            0f
        );

        transform.Translate(
            move * moveSpeed * Time.deltaTime,
            Space.World
        );
    }
}
