using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIReactionVoiceLine
{
    public AIReaction reaction;

    public List<string> voiceLines;

    public AIReactionBehavior reactionBehavior;
}
