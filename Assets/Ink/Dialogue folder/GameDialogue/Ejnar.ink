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
    - EjnarMainFirstTime: Remember to grab some supplies from the chest, before you head out.
        ~ EjnarMainFirstTime = false
        
    - EjnarMainReturn: Is there anything else?
        ~EjnarMainReturn = false
        
} 

    *[Anything specific I should grab?]
        -> supplies_to_grab

    *{EjnarMainHasAsked == false}[Where should I look?]
        ~EjnarMainHasAsked = true
        -> where_to_look
        
    +{EjnarMainHasAsked} [Who are we looking for?]
        -> the_men_you_look_for
    
    *[Where are we?]
        -> where_are_we
        
    
    * {EjnarMainFirstTime == false}[See you]
        Goodbye and good search
            ~EjnarMainReturn = true
            -> END  
    
=== supplies_to_grab ===
Yes, some food. You could also take a lantern. This time of the year the sun no longer rises.
    *[Continue]
        Just don't take to much, the provisions are low and geting more food here is not an easy task. Maby we can go hunting for meat while you are searching.
            * * [Continue]
                You can grab some wood to put on the stove
                -> main
            
            
=== where_to_look ===
Denmark's Harbour is north of here hopefully they reached there. They have build a small hut there, could be they are residing in that.

    *[Who are we looking for?]
        -> the_men_you_look_for
    
    *[Do you think they are alive?]
        -> still_alive



=== the_men_you_look_for ===
{
    - EjnarMainReturnToAsking && EjnarMainAskAboutJoergen && EjnarMainAskAboutNiels && EjnarMainAskAboutLudvig: You have heard about all of them, I have no more to tell you.  
    
    - EjnarMainReturnToAsking: Would you like to hear about the others.
        ~EjnarMainReturnToAsking = false
    
    - EjnarMainReturnToAsking == false && EjnarMainAskAboutJoergen == false && EjnarMainAskAboutNiels == false && EjnarMainAskAboutLudvig == false:  We are looking for Jørgen the Greenlander, Niels the Cartographer and your brother Ludvig.
        ~EjnarMainReturnToAsking = false

}

    * {EjnarMainAskAboutJoergen == false}[Jørgen]
        Jørgen, born and raised in Greenland if anyone of them shall have survived. He would be my first pick, I travelled together with him on an earlier trip to Greenland.
            ~EjnarMainAskAboutJoergen = true
        -> the_men_you_look_for
    
    * {EjnarMainAskAboutNiels == false}[Niels]
        Niels, the cartographer, responsible for all the drawings and measuring. Those sketches he must have made are all the reason we came here.
            ~EjnarMainAskAboutNiels = true
        -> the_men_you_look_for
    
    * {EjnarMainAskAboutLudvig == false}[Ludvig]
        Ludvig, him I do not know so much about. But you must, seeing as he is your brother.
            ~EjnarMainAskAboutLudvig = true
        -> the_men_you_look_for
        
    + {EjnarMainAskAboutJoergen == false || EjnarMainAskAboutNiels == false || EjnarMainAskAboutLudvig == false} [No thank you]
        A shame, they are good people all worthy an mention.
        ~EjnarMainReturnToAsking = true
        -> main
    
    + {EjnarMainAskAboutJoergen && EjnarMainAskAboutNiels && EjnarMainAskAboutLudvig}[Okay]
        Great I have nothing more to tell you about them.
        ~EjnarMainReturnToAsking = true
        -> main


=== still_alive === 
    If they have made it to the hut in Denmark’s Harbour and gotten the provisions from the stash. There is no doubt in my mind they all very well are alive.
        *[Continue]
            But here in Greenland nothing is safe, the weather is brutal. And if their sled dogs haven’t died, they cannot go very far. Pushing a sled is no easy work.
                    -> main



=== where_are_we ===
    We are close to the Shannon Island, the ice is not good for the boat’s hull. We cannot go any further north from here.
            -> main 







=== joergen_main ===
{
    - EjnarJoergenMainFirstTime: Welcome back, have you found anything? 
    
    - EjnarJoergenMainReturn: Is there anything else? 
        ~ EjnarJoergenMainReturn = false
}
~ EjnarJoergenMainFirstTime = false

    * {EjnarJoergenMainDiaryTalk == false} [Yes, this diary]
        ~ EjnarJoergenMainDiaryTalk = true
        -> found_diary
        
    * [I have not found the card]
        -> card_not_yet_found
        
    + [Where should I go next?]
    {
    
        - EjnarJoergenMainWhereToGoNext == 1: Since you have found the hut, try go, north or west. 
            -> joergen_main
        - EjnarJoergenMainWhereToGoNext == 2: Since you have not found the hut, try to find that. 
            -> joergen_main 
        - else: The only place they could be is north. Have you found the hut at Denmark's Harbour?
    }
    
        * * [Yes]
            ~ EjnarJoergenMainWhereToGoNext = 1
            But they where not there. Else they would have gone with you I am sure. Try to go, north or west from the hut. #hutFound
                -> joergen_main
        
        * * [No]
            ~ EjnarJoergenMainWhereToGoNext = 2
            Then try to go there next. That is what I would do. #hutNotFound
                -> joergen_main
    
    * [See you]
        -> ending_joergen



=== found_diary === 
~EjnarJoergenMainDiaryFound = true
Let me see it! ... Yes, or shall I say no. Jørgen always wrote in Greenlandic, so the fact he used time to make it readable for us tells me, he knew his time was comming. Where did you find this?
    
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

    * [I found him to in cave] 
        If we have the time we shall build a grave for him. A grave that shall stand the test of time.
            -> joergen_main
    
    * [It cannot have been good]
        No, please return if you ever find him.
            -> joergen_main
            

=== card_not_yet_found ===
{EjnarJoergenMainDiaryFound: With Jørgen's diary found,| Good question } I think you should continue searching north for the card.

    -> joergen_main

    
=== ending_joergen ===
Yes goodbye
~EjnarJoergenMainReturn = true
    --> END

=== niels_main ===
-> END

=== ludvig_main ===
-> END


=== card_main ===
You return what have you found?

    * {EjnarCardHasFoundIt == false} [I have found the card]
        ~EjnarCardHasFoundIt = true
        How splendid, we shall start our return trip home to Copenhagen. Are you ready to return?
        
            * * [Yes, let us return]
                -> end_the_game
                
            * * [No, not yet]
                Be quick, we do not have long before the ice will lock us in here
                    -> END
    
    * [Nothing to report]
        That is fine, let me hear when you have found something
            -> END

    * {EjnarCardHasFoundIt} [Let us return]
        ->end_the_game

=== end_the_game ===
So we shall. #EndGame
-> END




