using UnityEngine;

[CreateAssetMenu(fileName = "Acts", menuName = "Scriptable Objects/Acts")]
public class Acts : ScriptableObject
{
    public int ActId;

    public GameObject[] actors;

    public enum questType { Sword, Armor, Shield }

    public questType type;

    public string questSceneName;
    
    public GameObject[] endActors;
}
