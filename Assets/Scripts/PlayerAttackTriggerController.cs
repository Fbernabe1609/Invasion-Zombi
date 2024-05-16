using UnityEngine;

public class PlayerAttackTriggerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController playerMovementController;

    internal void FinishAttack()
    {
        playerMovementController.isAttacking = false;

        for (int i = 0; i < playerMovementController.attackTriggers.Length; i++)
        {
            playerMovementController.attackTriggers[i].SetActive(false);
        }
    }

    internal void PlayAttackSound()
    {
        playerMovementController.PlayAttackSound();
    }
}
