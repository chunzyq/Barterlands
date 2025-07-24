using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Button button;

    private StalkerData stalkerData;
    public event Action<StalkerData> OnUnitClicked;

    public void Initialize(StalkerData data)
    {
        stalkerData = data;
        nameLabel.text = data.stalkerName;
        button.onClick.AddListener(() => OnUnitClicked?.Invoke(data));
    }
}
