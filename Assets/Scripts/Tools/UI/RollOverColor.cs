using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class RollOverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color baseColor;
    public Color rollOver;
    public float fade;

    private bool hover;

    private float value = 0f;
    Image image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover = false;
    }

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        value = hover ? value + (Time.deltaTime*fade) : value - (Time.deltaTime*fade);

        image.color = Color.Lerp(baseColor, rollOver, value);
    }
}
