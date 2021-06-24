using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/**
 * A Dialogue is a collection of sentences that are to be displayed in order.
 *
 * Position of dialogue box:
 * Create an empty GameObject as a child of the character sprite and place it a
 * bit above its head.  Set this GameObject as `diaboxPosition`.  The bottom of
 * the dialogue box will touch the `diaboxPosition` object.
 */
[Serializable]
public class Dialogue
{
    // no conditions!  GetActiveDialogue handles this
    public List<Sentence> sentences;  // linear; branching = triggering new dialogues
    [Tooltip("Position of the interaction hint.")]
    public Transform hintPosition;
    [Tooltip("Leave empty to use the default background.")]
    public Sprite background;
}
