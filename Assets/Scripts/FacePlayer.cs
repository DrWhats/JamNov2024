using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform playerTransform;

    private void Start()
    {
        // ���� MainCamera � �����
        GameObject player = GameObject.Find("Look");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("MainCamera not found in the scene. Make sure it has the 'MainCamera' tag.");
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // ������������ NPC ����� � ������
            transform.LookAt(playerTransform);
        }
    }
}