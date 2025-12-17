using UnityEngine;

public class WeaponAnim : MonoBehaviour
{
    private Transform target;

    //plays an animation when target is in range
  public void PlayWeaponAnimation(Transform target)
    {
        this.target = target;
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isFiring", true);
        }
    }
}
