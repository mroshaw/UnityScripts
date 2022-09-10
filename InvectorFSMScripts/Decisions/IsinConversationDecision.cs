using DaftAppleGames.Characters;
using Invector.vCharacterController.AI.FSMBehaviour;

namespace DaftAppleGames.AI
{
#if UNITY_EDITOR
    [vFSMHelpbox("This is an IsinConversation decision", UnityEditor.MessageType.Info)]
#endif
    public class IsinConversationDecision : vStateDecision
    {
		public override string categoryName => "Dialog and Quests/";

        public override string defaultName => "Is in Conversation";

        public override bool Decide(vIFSMBehaviourController fsmBehaviour)
        {
            if (fsmBehaviour.aiController != null)
            {
                return fsmBehaviour.aiController.IsInConversation;
            }
            return false;
        }
    }
}
