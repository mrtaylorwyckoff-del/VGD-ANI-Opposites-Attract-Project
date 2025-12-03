using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Search;

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
