using UnityEngine;

// Reveals the level select scene: the spotlight fades in first, then the rest of the scene lights up.
public class LevelSelectIntro : MonoBehaviour
{
    [SerializeField] Spotlight spotLight;
    [SerializeField] Spotlight globalLight;

    void Start()
    {
        spotLight.TurnOn(() => globalLight.TurnOn());
    }
}
