using UnityEngine;

public class Tooltips : MonoBehaviour
{
    [SerializeField] private GameObject tooltipObject;

    public void PointerEnter()
    {
        tooltipObject.SetActive(true);
    }

    public void PointerExit()
    {
        tooltipObject.SetActive(false);

    }
}
