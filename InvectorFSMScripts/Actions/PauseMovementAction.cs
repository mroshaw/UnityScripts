using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.AI
{
#if UNITY_EDITOR
    [vFSMHelpbox("Pause or unpause a NavMeshAgent AI movement", UnityEditor.MessageType.Info)]
#endif
    public class PauseMovementAction : vStateAction
    {
        public bool pausedState;
        
        /// <summary>
        /// Category to include this action in, in the Action menu
        /// </summary>
        public override string categoryName => "Movement/";

        /// <summary>
        /// Name to display in Action menu
        /// </summary>
        public override string defaultName => "Pause Movement";

        /// <summary>
        /// Implement the Action
        /// </summary>
        /// <param name="fsmBehaviour"></param>
        /// <param name="executionType"></param>
        public override void DoAction(vIFSMBehaviourController fsmBehaviour, vFSMComponentExecutionType executionType = vFSMComponentExecutionType.OnStateUpdate)
        {
            if (pausedState)
            {
                fsmBehaviour.aiController.StopMovement();
            }
        }
    }
}