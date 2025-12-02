using Unity.VisualScripting;
using UnityEngine;

public class Plots : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
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
        if (tower != null)
        {
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Broke");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}
