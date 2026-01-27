using UnityEngine;

public class player : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 movement = transform.right * x + transform.forward * z;

        controller.Move(movement * (speed * Time.deltaTime));
        
        
    }
}
