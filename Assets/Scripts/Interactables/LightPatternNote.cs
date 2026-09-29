using UnityEngine;

public class LightPatternNote : MonoBehaviour
{
    [SerializeField] private bool[] correctPattern =
    {
        true,
        false,
        true,
        false,
        false
    };

    public bool[] GetPattern()
    {
        return correctPattern;
    }
}