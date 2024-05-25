INCLUDE Globals.ink

{
    - theCardFound: -> card_main
    - ludvigDiaryFound: -> ludvig_main
    - nielsDiaryFound: -> niels_main
    - joergenDiaryFound: -> joergen_main
    - else: -> main

}



=== main ===

{
    - EjnarMainFirstTime: Remember to grab some provisions from the chest before you head out.
        ~ EjnarMainFirstTime = false
        
    - EjnarMainReturn: Is there anything else?
        ~EjnarMainReturn = false
        
} 

    *[Should I get anything specific?]
        -> supplies_to_grab

    *{EjnarMainHasAsked == false}[Where should I look?]
        ~EjnarMainHasAsked = true
        -> where_to_look
        
    +{EjnarMainHasAsked} [Who are we looking for?]
        -> the_men_you_look_for
    
    *[Where are we?]
        -> where_are_we
        
    
    * {EjnarMainFirstTime == false}[See you]
        Goodbye and good search.
            ~EjnarMainReturn = true
            -> END  
    
=== supplies_to_grab ===
Take some food, it will be necessary. You should also bring a lantern. The sun no longer rises this time of the year.
    *[Continue]
        Though, you shouldn't take too much. We are running low on provisions, and getting more here is not an easy task. Perhaps we should hunt for meat while you are out.
            * * [Continue]
                You should also place some firewood in the furnace, it's getting cold.
                -> main
            
            
=== where_to_look ===
Denmark's Harbour, just north of here. There is a small hut built over there. Hopefully, they have reached it. Could they be residing in it?

    *[Who are we looking for?]
        -> the_men_you_look_for
    
    *[Do you think they are alive?]
        -> still_alive



=== the_men_you_look_for ===
{
    - EjnarMainReturnToAsking && EjnarMainAskAboutJoergen && EjnarMainAskAboutNiels && EjnarMainAskAboutLudvig: You have heard about each one, I have nothing more to add. 
    
    - EjnarMainReturnToAsking: Would you like to hear about the others?
        ~EjnarMainReturnToAsking = false
    
    - EjnarMainReturnToAsking == false && EjnarMainAskAboutJoergen == false && EjnarMainAskAboutNiels == false && EjnarMainAskAboutLudvig == false:  We are looking for Joergen the Greenlander, Niels the cartographer and your brother Ludvig.
        ~EjnarMainReturnToAsking = false

}

    * {EjnarMainAskAboutJoergen == false}[Joergen]
        Joergen, born and raised in Greenland. If any of them will survive, he would be my first pick. I have travelled with him on a trip to Greenland in the past.
            ~EjnarMainAskAboutJoergen = true
        -> the_men_you_look_for
    
    * {EjnarMainAskAboutNiels == false}[Niels]
        Niels, the cartographer. He's responsible for all the drawings and measurements. The sketches he drew are all the reason we came here.
            ~EjnarMainAskAboutNiels = true
        -> the_men_you_look_for
    
    * {EjnarMainAskAboutLudvig == false}[Ludvig]
        Ludvig. He, I do not know much about, but you, as his brother, must know more.
            ~EjnarMainAskAboutLudvig = true
        -> the_men_you_look_for
        
    + {EjnarMainAskAboutJoergen == false || EjnarMainAskAboutNiels == false || EjnarMainAskAboutLudvig == false} [No thank you]
        What a shame, they are good people all worthy of mention.
        ~EjnarMainReturnToAsking = true
        -> main
    
    + {EjnarMainAskAboutJoergen && EjnarMainAskAboutNiels && EjnarMainAskAboutLudvig}[Okay]
        Great, I have nothing more to tell you about them.
        ~EjnarMainReturnToAsking = true
        -> main


=== still_alive === 
    If they have reached the hut in Denmark's Harbour and got the provisions from the stash, there is no doubt in my mind that they all very well are alive.
        *[Continue]
            But nothing is safe in Greenland, the weather is brutal. If their sled dogs have died, they can't go very far. Pushing a sled is no easy task.
                    -> main



=== where_are_we ===
    We are near Shannon Island. The ice is no good for the boat's hull. We cannot go any further north from here.
            -> main 







=== joergen_main ===
{
    - EjnarJoergenMainFirstTime: Welcome back, have you found anything? 
        ~ EjnarJoergenMainFirstTime = false
        
    - EjnarJoergenMainReturn: Is there anything else? 
        ~ EjnarJoergenMainReturn = false
}

    * {EjnarJoergenMainDiaryTalk == false} [Yes, this diary]
        ~ EjnarJoergenMainDiaryTalk = true
        -> found_diary
        
    * [I have not found the map]
        -> card_not_yet_found
        
    + [Where should I go next?]
    {
    
        - EjnarJoergenMainWhereToGoNext == 1: Since you have found the hut, try to go north or west. 
            -> joergen_main
        - EjnarJoergenMainWhereToGoNext == 2: Try to find the hut. 
            -> joergen_main 
        - else: The only place they could be is north. Have you found the hut at Denmark's Harbour yet?
    }
    
        * * [Yes]
            ~ EjnarJoergenMainWhereToGoNext = 1
            But they weren't there? Otherwise, they would have come with you, I'm sure of that. Try to go north or west from the hut. #hutFound
                -> joergen_main
        
        * * [No]
            ~ EjnarJoergenMainWhereToGoNext = 2
            Then try to go there next. That's what I would do. #hutNotFound
                -> joergen_main
    
    * [See you]
        -> ending_joergen



=== found_diary === 
~EjnarJoergenMainDiaryFound = true
Let me see that! Joergen always writes in Greenlandic. The fact he wrote this in Danish for us tells me he knew his time was coming. Where did you find this?
    
    * [In a cave, together with him]
        -> joergens_fate(true)
    
    * [In a cave]
        -> joergens_fate(false)
    
    * [In the snow]
        -> joergens_fate(false)

VAR joergenFound = false
=== joergens_fate(var) === 
~ joergenFound = var
{joergenFound: The fact he did not come with you tells me enough. If we have the time we shall build a grave for him. A grave that shall stand the test of time. | Without him? Good God what could have happened to him? }
    
    {joergenFound: -> joergen_main}

    * [I found him in a cave]
        If we have the time we shall build a grave for him. A grave that shall stand the test of time.
            -> joergen_main
    
    * [It cannot have been good]
        No, please return if you ever find him.
            -> joergen_main
            

=== card_not_yet_found ===
{EjnarJoergenMainDiaryFound: With Joergen's diary found,| Good question } I think you should continue searching north for the map.

    -> joergen_main

    
=== ending_joergen ===
Goodbye.
~EjnarJoergenMainReturn = true
    --> END

=== niels_main ===
{
    - EjnarNielsMainFirstTime: You have returned, did you find anything new?
        ~ EjnarNielsMainFirstTime = false
    - EjnarNielsMainReturn: Was there anything else?
        ~ EjnarNielsMainReturn = false
}


    * [{EjnarNielsMainToldAboutDiary == false: Yes, another diary | Questions about diary}]
        -> niels_diary
        
        
    * {EjnarNielsSledFirstAsk}[I have found a sled]
        Did it have anything on it?
            -> niels_sled
    
    + [See you]
        ~ EjnarNielsMainReturn = true
        {EjnarNielsMainToldAboutDiary: Yes, now go find that map! | See you too} 
            -> END



=== niels_diary ===
    ~EjnarNielsMainToldAboutDiary = true
    {
        - EjnarNielsDiaryFirstAsk: Let me see it. Yes they did it, the Peary Channel does not exist! But they left the map in a stone cairn it seems.
        - EjnarNielsDiaryReturn: Do you have another question?
    
    }
    ~ EjnarNielsDiaryFirstAsk = false
    ~ EjnarNielsDiaryReturn = false
        * [Peary Channel?]
            American Robert Peary drew a map of Greenland being connected to North America. The map Niels drew disproves it. Greenland is rightfully a Danish colony.
                -> niels_diary
            
        
        * [Stone cairn?]
            Greenland has few landmarks, so we stack stones to find our way back. Sometimes, they also contain provisions.
                -> niels_diary
        
        + [What now?]
        ~ EjnarNielsDiaryReturn = true
            Find the cairn and get the map. It must be north of where you found this diary.
                -> niels_main
                
    

=== niels_sled ===
    * [{EjnarNielsSledFirstAsk: Yes, a diary | Also a diary}]
        ~ EjnarNielsSledFirstAsk = false
            -> niels_diary
    
    * [The sled was broken]
        ~ EjnarNielsSledFirstAsk = false
        That's bad. Nothing good comes from a broken sled. Is there anything else?
            -> niels_sled

=== ludvig_main ===
{

    - EjnarLudvigMainFirstTime: Welcome back, do you have anything to report?
        ~ EjnarLudvigMainFirstTime = false

    - EjnarLudvigMainReturn: The last thing we now is the map.
        ~ EjnarLudvigMainReturn = false
}

    * [I found Ludvig]
        The fact that he is not here must mean he is with God now. The risk is huge when you come to Greenland.
            -> ludvig_main
        
    + [See you]
        See you. Now go find that map!
            -> END
        
        

=== card_main ===
You have returned, did you find something?

    * {HansBecomeInsane && HansGotTheMap == false}[I have the map]
        Usually, I would have Hans look at it, but he seems to have lost his mind. The cold does strange things to men. Are you ready to return?
            -> to_return
            
    * {HansDidNotGetTheMap} [I have the map]
        And you didn't want to give it to Hans? I cannot fathom why, but more importantly, we have it. Are you ready to return?
            -> to_return

    * {HansGotTheMap == false && HansBecomeInsane == false} [I have the map]
        Splendid! Give it to Hans.
            -> END
        
    * {HansGotTheMap} [Hans has the map]
        Then we shall begin our return trip home to Copenhagen. Are you ready to return?
            -> to_return
    
    * [Nothing to report]
        That's fine, let me hear when you have found something.
            -> END

    * {EjnarCardHasFoundIt} [Let us return]
        ->end_the_game
        
        
=== to_return ===
    ~EjnarCardHasFoundIt = true
    * [Yes, let's return]
        -> end_the_game
        
    * [No, not yet]
        Be quick, we do not have long before the ice will lock us in here.
            -> END

=== end_the_game ===
So we shall. #EndGame
-> END




