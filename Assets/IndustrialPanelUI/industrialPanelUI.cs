using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class industrialPanelUI : MonoBehaviour
{
    public Image workerSlot1;
    public Image workerSlot2;
    public Color activeColor = Color.white;
    public Color inactiveColor = Color.gray;

    private void Start() {

        // workerSlot1.color = inactiveColor;
        // workerSlot2.color = inactiveColor;

        AddClickListener(workerSlot1, ToggleWorkSlot1);
        AddClickListener(workerSlot2, ToggleWorkSlot2);
    }

    private void ToggleWorkSlot1()
    {
        SetTransparencyForWorkSlot1(1f);
    }
    private void ToggleWorkSlot2()
    {
        SetTransparencyForWorkSlot2(1f);
    }

    public void AddClickListener(Image image, Action onClick)
    {
        Button button = image.GetComponent<Button>();
        button.onClick.AddListener(() => onClick());
    }
    private void SetTransparencyForWorkSlot1(float alpha)
    {
        Color color = workerSlot1.color;

        color.a = Mathf.Clamp01(alpha);

        workerSlot1.color = color;
    }
    private void SetTransparencyForWorkSlot2(float alpha)
    {
        Color color = workerSlot2.color;

        color.a = Mathf.Clamp01(alpha);

        workerSlot2.color = color;
    }
}
