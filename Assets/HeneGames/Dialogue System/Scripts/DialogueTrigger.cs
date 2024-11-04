using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HeneGames.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Controllers")]
        private CharacterController characterController;
        private MonoBehaviour firstPersonController; // Предполагается, что это скрипт, управляющий первым лицом

        [Header("Events")]
        public UnityEvent startDialogueEvent;
        public UnityEvent nextSentenceDialogueEvent;
        public UnityEvent endDialogueEvent;

        private void Start()
        {
            // Автоматический поиск контроллеров
            FindControllers();

            // Подписка на события
            startDialogueEvent.AddListener(DisableControllers);
            endDialogueEvent.AddListener(EnableControllers);
        }

        private void FindControllers()
        {
            // Поиск CharacterController
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogWarning("CharacterController not found on the GameObject.");
            }

            // Поиск FirstPersonController (предполагается, что это MonoBehaviour)
            firstPersonController = GetComponent<MonoBehaviour>();
            if (firstPersonController == null)
            {
                Debug.LogWarning("FirstPersonController not found on the GameObject.");
            }
        }

        private void DisableControllers()
        {
            if (characterController != null)
            {
                characterController.enabled = false;
            }

            if (firstPersonController != null)
            {
                firstPersonController.enabled = false;
            }
        }

        private void EnableControllers()
        {
            if (characterController != null)
            {
                characterController.enabled = true;
            }

            if (firstPersonController != null)
            {
                firstPersonController.enabled = true;
            }
        }
    }
}