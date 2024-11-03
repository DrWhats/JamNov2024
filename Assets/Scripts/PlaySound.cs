using UnityEngine;

public class PlaySound : MonoBehaviour
{

    [SerializeField] private GameObject SFXPrefab;

    public void Play()
    {
        Instantiate(SFXPrefab);
    }
}
