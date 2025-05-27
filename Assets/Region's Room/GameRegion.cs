using UnityEngine;

public class GameRegion : MonoBehaviour
{
    [SerializeField] private GameObject blackCoverageObject;
    [SerializeField] private Material blackCoverageMat;

    public void ApplyMaterial()
    {
        Renderer renderer = blackCoverageObject.GetComponent<Renderer>();
        renderer.material = blackCoverageMat;
    }
}
