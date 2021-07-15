using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacoonCuteDialogue :  DialogueTrigger{
    public static RacoonCuteDialogue t = null;
    public Item snack;
    public Item coin;
    public Item crank;
    public Item _miniRacoonGamePlayed;
    public Item _miniRacoonGameWon;
    public Item _racoonMad;
    public Item _alreadyTalked;
    public Item _talkToRaccoon;
    public Portal portalToMiniGame;

    public void EnterMiniGame() {
        portalToMiniGame.TriggerTeleport();  
    }
    public override Dialogue GetActiveDialogue(){

        if(Inventory.Instance.HasItem(_miniRacoonGameWon)){
            return new MiniGameFinishedDia();
        } 
        if(Inventory.Instance.HasItem(_racoonMad) && !Inventory.Instance.HasItem(_talkToRaccoon)){
            return new DestructionNoise();
        }
        if(Inventory.Instance.HasItem(_racoonMad) && Inventory.Instance.HasItem(_talkToRaccoon)){
            return new TalkingToRaccoon();
        }
        if(Inventory.Instance.HasItem(_miniRacoonGamePlayed)){
            return new MiniGameLostDia();
        }
        if(Inventory.Instance.HasItem(_alreadyTalked)){
            return new NewChoiceDialogue();
        }
        
        
        return new RacoonCuteDefaultDialogue();
    }

    void Awake(){
        t = this;
        if(Inventory.Instance.HasItem(_racoonMad) || Inventory.Instance.HasItem(_miniRacoonGamePlayed)){
            RacoonCuteDialogue.t.transform.position = new Vector3(8.4f,-2.2f,1f);
        }
        
    }

    
}
public class RacoonCuteDefaultDialogue : Dialogue {
    public RacoonCuteDefaultDialogue(){
        Say("*sniff...sniff...*");
        Say("*puppy eyes* Oh hello mr. janitor.");
        Say("I have a big big big biiiiig problem, can you please help me out?");
        Say("I have also a little compensation for your kind service sir")
            .DoAfter(new TriggerDialogueAction<ChoiceDialogue>());
            

            
    }
}

public class MiniGameLostDia : Dialogue {
    public MiniGameLostDia(){
        Say("LOSER!..");
        Say("Try to stop me!")
            .Choice(
                new TextOption("Oh i will")
                .IfChosen(new DialogueAction(() => {
                        RacoonCuteDialogue.t.EnterMiniGame();
                    }))
            )
            .Choice(
                new TextOption("no")
            );
    }
}

public class DestructionNoise : Dialogue {
    public DestructionNoise(){
        Say("*destruction noise*");
    }
}

public class ChoiceDialogue : Dialogue {
    public ChoiceDialogue(){
        Say("I feel sick and I need candy as my medicine")
            .Choice(
                new TextOption("You don't look sick to me")
                .IfChosen(new TriggerDialogueAction<SickRacoonDia>()))
            .Choice(
                new ItemOption(RacoonCuteDialogue.t.snack)
                .IfChosen(new TriggerDialogueAction<SnackDialogue>()))
            .Choice(
                new TextOption("no!"));    
    }
}

public class NewChoiceDialogue : Dialogue {
    public NewChoiceDialogue(){
        Say("Please I need my medicine")
            .Choice(
                new ItemOption(RacoonCuteDialogue.t.snack)
                .IfChosen(new TriggerDialogueAction<SnackDialogue>()))
            .Choice(
                new TextOption("no!"));    
    }
}

public class TalkingToRaccoon : Dialogue {
    public TalkingToRaccoon() {
        Say("HAHA you fool! come and catch me!")
            .Choice(
                new TextOption("oh i will")
                .IfChosen(new DialogueAction(() => {
                        RacoonCuteDialogue.t.EnterMiniGame();
                    }))
            )
            .Choice(
                new TextOption("I'll pass")
                .IfChosen(new TriggerDialogueAction<DestructionNoise>())
            );
    }
}

public class MiniGameFinishedDia : Dialogue {
    public MiniGameFinishedDia(){
        Say("oof that was a workout...*yawn*");
        Say("Oh this is awkward...");
        Say("well I should go to bed!");
        Say("anyways, I am sorry..but thank you for giving me a treat :3")
            .DoAfter(RemoveItem(RacoonCuteDialogue.t._racoonMad))
            .DoAfter(RemoveItem(RacoonCuteDialogue.t._miniRacoonGamePlayed));
        
    }
}
public class SnackDialogue : Dialogue {
    public SnackDialogue(){
        Say("Thank you sooooo much my dear janitor")
            .DoAfter(RemoveItem(RacoonCuteDialogue.t.snack));
        Say("Since you helped me I will gift you this beautiful coin!")
            .DoAfter(GiveItem(RacoonCuteDialogue.t.coin))
            .DoAfter(GiveItem(RacoonCuteDialogue.t._racoonMad))
            .DoAfter(RemoveItem(RacoonCuteDialogue.t._alreadyTalked))
            .DoAfter(new DialogueAction(()=> {
                RacoonCuteDialogue.t.transform.position = new Vector3(8.4f,-2.2f,1f);

                // temporary, while we don't have a cutscene thing yet
                StoreOwnerDialogue candyPerson = GameObject.FindObjectOfType<StoreOwnerDialogue>();
                if (candyPerson != null) {
                    candyPerson.UpdateAnimator();
                }
                
            }));

    }
}
public class SickRacoonDia : Dialogue {
    public SickRacoonDia(){
        Say("i swear, i really need some of that sweet medicine...and by that I mean candy");
        Say("dont you want my little suprise that I have for you?")
            .DoAfter(new TriggerDialogueAction<NewChoiceDialogue>())
            .DoAfter(GiveItem(RacoonCuteDialogue.t._alreadyTalked));
    }
}