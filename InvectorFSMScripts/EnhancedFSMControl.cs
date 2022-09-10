using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Invector.vCharacterController.AI
{
    /// <summary>
    /// Extend the Interface
    /// </summary>
    public partial interface vIControlAI
    {
        // Control parameters
        public bool IsInConversation { get; set; }
    }

    /// <summary>
    /// Implementation of the new methods
    /// </summary>
    public partial class vControlAI
    {
        /// <summary>
        /// Allow configuration within the UI
        /// </summary>
        
        [vEditorToolbar("NPC Behaviour")]
        [vHelpBox("Various settings for the behaviour of the AI NPC", vHelpBoxAttribute.MessageType.Info)]
        private bool _isInConversation;

        /// <summary>
        /// IsInConversation getter and setter
        /// </summary>
        public bool IsInConversation
        {
            get => _isInConversation;
            set => _isInConversation = value;
        }
    }
}