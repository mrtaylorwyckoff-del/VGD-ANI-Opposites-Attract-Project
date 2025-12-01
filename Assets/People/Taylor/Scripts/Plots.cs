using Unity.VisualScripting;
using UnityEngine;

public class Plots : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hoverColor;

    private GameObject turret;
    private Color startcolor;

    private void Start()
    {
        startcolor = spriteRenderer.color;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = startcolor;
    }

    private void OnMouseDown()
    {
        if (turret != null)
        {
            Debug.Log("Can't build there! Plot already occupied.");
            return;
        }
        GameObject turretToBuild = BuildManager.main.GetSelectedTower();
        turret = Instantiate(turretToBuild, transform.position, Quaternion.identity);
        Debug.Log("Turret built!");
    }
}
