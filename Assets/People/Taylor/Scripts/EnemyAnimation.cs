using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    public float moveSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
        {

            // Get the horizontal input value from the keyboard (A/D or Left/Right arrow).
            float horizontalInput = Input.GetAxisRaw("Horizontal");


            transform.position += new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);

            animator.SetFloat("HorizontalSpeed", horizontalInput);

            if (horizontalInput < 0)
            {
                // Facing left
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontalInput > 0)
            {
                // Facing right
                transform.localScale = new Vector3(1, 1, 1);
            }
       }
   }