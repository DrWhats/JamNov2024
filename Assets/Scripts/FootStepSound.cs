using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FootstepSound : MonoBehaviour
{
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepInterval = 3f;
    [SerializeField] private float minSpeedForFootsteps = 0.1f;
    [SerializeField] private float maxDistance = 10f; // ������������ ����������, �� ������� ���� ����� ������
    [SerializeField] private float minDistance = 1f; // ����������� ����������, �� ������� ���� ����� ������

    private NavMeshAgent agent;
    private AudioSource audioSource;
    private float lastFootstepTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = footstepClip;
        audioSource.loop = false;
        audioSource.volume = 0.2f;
        audioSource.spatialBlend = 1f; // ������������� 3D ����
        audioSource.maxDistance = maxDistance;
        audioSource.minDistance = minDistance;
        audioSource.rolloffMode = AudioRolloffMode.Linear; // �������� ��������� �����
    }

    private void Update()
    {
        if (agent.velocity.magnitude > minSpeedForFootsteps && agent.remainingDistance > agent.radius)
        {
            if (Time.time - lastFootstepTime > footstepInterval)
            {
                PlayFootstepSound();
                lastFootstepTime = Time.time;
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (audioSource != null && footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip);
        }
    }
}