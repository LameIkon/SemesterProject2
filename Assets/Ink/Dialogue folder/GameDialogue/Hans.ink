INCLUDE Globals.ink
{
    - theCardFound: -> card_main
    - ludvigDiaryFound: -> ludvig_main
    - nielsDiaryFound: -> ludvig_main
    - joergenDiaryFound: -> joergen_main
    - else: -> main

}




=== main ===

{
    - HansTemperaturFeeling == "" && HansMainReturn: Ahoy, may I help you?
    
    - HansMainReturn && HansWeatherTheory: Last time you reported you felt {HansTemperaturFeeling}, has this changed?
    
    - HansMainReturn && HansNewTheory: Do you want something? 
}
        ~ HansMainReturn = false
 
    + {HansTemperaturFeeling} [Yes, it has changed]
        -> weather_feelings
    
    * {HansWeatherTheory == false} [What are you working on?]
        -> working_on
        
    * {HansNewTheory} [Any new theories?]
        Negative, it will take some time before new ones come to me.
            -> main
        
    
    * [What are you doing on the ship?]
        -> what_do_you_do
    
    * [See you]
        See you too.
        ~ HansMainReturn = true
        -> END
        
        
=== working_on ===
I am now on the brink of brilliance. I came with the intent to learn why some people don't feel the cold as much as others.
    
    *[What did you learn?]
        -> first_theory
    
    *[Okay]
        Do you want to hear about it?
            
            * * [Yes]
                -> first_theory
            
            * * [No]
                How come? I promise it is very exciting.
                
                    * * * [Okay, go ahead]
                        -> first_theory
                    * * * [I'm just not interested]
                        -> new_theory
        
        

=== first_theory ===
    ~HansWeatherTheory = true
    I have deduced that I feel the cold, and Ejnar does not. Though he has been here before. If I go out in the snow, I'll maybe feel less cold when I return to the ship.
    
    *[Anything else]
        No, but maybe you could report to me how you feel?
            
            * * [Yes, I can do that]
                -> weather_feelings
        
            * * [No thank you]
                Oohh, are you sure?
                    
                    * * * [Yes]
                        -> new_theory
                        
                    * * * [No, let me report]
                        -> weather_feelings
      
=== weather_feelings ===
~HansTemperaturAmountOfChanges++
{
    - HansTemperaturAmountOfChanges < 5 :Oohh, how exciting. Please tell me.
    - HansTemperaturAmountOfChanges < 10 : You have now reported back {HansTemperaturAmountOfChanges} times, the data is getting better the more you report. 
    - HansTemperaturAmountOfChanges < 25 : With {HansTemperaturAmountOfChanges} changes in how you feel, I think you do not have to report more. You are of course welcome to.
    - HansTemperaturAmountOfChanges < 50 : {HansTemperaturAmountOfChanges} times! I think you are just toying with me at this point.
    - HansTemperaturAmountOfChanges < 100 : {HansTemperaturAmountOfChanges} times! Stop! Please Stop!
    - HansTemperaturAmountOfChanges < 10000 : I am just going to ignore the rest reports.
}

    + [I'm feel cold]
        -> noted("cold")
    + [I'm freezing]
        -> noted("freezing")
    + [I'm warm]
        -> noted("warm")
    + [I feel fine]
        -> noted("fine")
      
=== noted(var) === 
Noted. Please report back if this ever changes.
~HansTemperaturFeeling = var
{
    - ludvigDiaryFound: -> ludvig_main
    - nielsDiaryFound: -> ludvig_main
    - joergenDiaryFound: -> joergen_main
    - else: -> main

}


VAR HansMainAskWhatHeDoes = false
=== what_do_you_do ===
{HansMainAskWhatHeDoes: | I'm Hans, a scientist from the University of Copenhagen. I'm here to conduct experiments. I am responsible for constructing the final map from the sketches. }

    * [What did you study?]
        ~ HansMainAskWhatHeDoes = true
        I've studied quite a lot. My main interests are humans and weather. How does one live in a place like Greenland voluntarily? My thermometer cannot measure temperatures this low.
        -> what_do_you_do
    
    * [Back to the other questions]
        Sure, anything else I can help with?
        -> main



=== new_theory ===
Hmpf, you just gave me a new theory I'll have to ponder. It will be even better than the last.
    ~ HansNewTheory = true
-> main











=== joergen_main ===
{
    - HansJoergenFirstTimeMain: Oh, you are back already. {HansWeatherTheory: Do you have anything to report? Maybe you've got something new about the cold.} {HansNewTheory: I have a new theory}
        ~HansJoergenFirstTimeMain = false
    - HansJoergenReturn: Would you like anything else? 
        ~HansJoergenReturn = false
}

    + {HansWeatherTheory && HansJoergenTheoryHeard}[I'd like to report]
        -> weather_feelings
    
    * {HansWeatherTheory && HansJoergenTheoryHeard == false}[Yes]
        ~HansJoergenTheoryHeard = true
        -> weather_feelings
    * {HansWeatherTheory && HansJoergenTheoryHeard == false}[No]
        ~HansJoergenTheoryHeard = true
        -> nothing_to_report
        
        
    * {HansNewTheory && HansJoergenTheoryHeard == false}[You do?]
        ~HansJoergenTheoryHeard = true
        -> new_theory_joergen
        
    * [Who were you again?]
        Me? I'm Hans the scientist {HansBecomeInsane: not that it means anything to someone like you.} {HansWeatherTheory: currently occupied by your reports from outside.} {HansNameTheory: Hans, name: Hans occupation: scientist. You remember that now.} {HansNewTheory: currently, I have a new theory for you to hear. } {HansEatingTheory: have you eaten something today?}
                -> joergen_main
    
    * [See you]
    ~ HansJoergenReturn = true
    {
        - HansWeatherTheory && HansJoergenTheoryHeard: See you, and please report back.   
        - HansNewTheory && HansJoergenTheoryHeard: See you.
        - else: See you.
    }
            -> END




=== nothing_to_report ===
No no no no... that can't be. You must have something, anything, just anything you can report. 
    * [I have something]
        You are a funny man Iver. Of course you got something to report.
            -> weather_feelings

    * [No, nothing to report]
        ~HansWeatherTheory = false
        That makes sense. You are forgetting your struggle from being outside. Wait, that must be it! It's so cold you can't remember. Do you know the name of the captain?
        
        * * [Laub]
            -> does_not_remember_captain("Laub")
        * * [Hans]
            -> does_not_remember_captain("Hans")
        * * [Ejnar]
            No, my hypothesis must be wrong... or is it? No, he must have guessed and gotten it right. Yes, that's it. The hypothesis cannot be wrong. I can never be wrong.
                -> joergen_main
        * * [Who?]
            -> does_not_remember_captain("Who")



=== does_not_remember_captain(name) ===
~ HansNameTheory = true
{name}, interesting, very interesting. {name == "Who": Subject does not remember our captain. | Subject recalls a name but not the correct one.} Yes, my theory must be true. 
    -> joergen_main
    


=== new_theory_joergen ===
~HansNewTheory = false
Oohh, yes I do. Would you like to hear it?
    * [Yes]
        I cannot wait to tell you.
            -> eating_theory
    
    * [No, not really]
        Oohh, why not not!?
        
            * * [I am just joking]
                Very funny Iver. You sure are humorous.
                    -> eating_theory
            
            * * [I just don't want to]
            ~HansBecomeInsane = true
                No, of course not. Who would even want to listen to someone like me? No one.
                -> joergen_main


=== eating_theory ===
~HansEatingTheory = true

Something strange happens here in Greenland. It seems that one can eat and eat and not become full. Have you tried this?
    
    * [Yes]
        Yes, strange isn't it? I still have no good explanation for this phenomenon.
        -> joergen_main
    
    * [No]
        Strange. How long does one have to be here to get to that level? 
        -> joergen_main


=== ludvig_main ===
{
    - HansBecomeInsane: -> hans_becomes_insane
    - HansNameTheory: -> name_theory_end
    - HansEatingTheory: -> eating_theory_end
    - HansWeatherTheory: -> weather_theory_end
    - else: -> main
}
    


-> END

=== eating_theory_end ===
{
    - HansNielsFirstEatingTheory: Welcome back Iver! Have you tried out my new theory?
        ~ HansNielsFirstEatingTheory = false
    - HansNielsEatingTheoryReturn: Iver, you have returned. Have you tried my theory?
        ~ HansNielsEatingTheoryReturn = false
}
    * {HansNielsEatingTheoryPicked == false}[Yes]
        ~ HansNielsEatingTheoryPicked = true
        -> eating_theory_tried(true)
    
    * {HansNielsEatingTheoryPicked == false} [No]
        ~ HansNielsEatingTheoryPicked = true
        -> eating_theory_tried(false)
    
    * {HansNielsEatingTheoryPicked == false} [What theory?]
        My theory about you being able eat without stopping. Like you are never full.
        -> eating_theory_end
    
    * [See you]
        ~ HansNielsEatingTheoryReturn = true
        Yes yes, see you.
        -> END
    



=== eating_theory_tried(response) ===
{response: Oohh yes, can you explain your findings?| What a shame, it could have provided invaluable data.}


    * {response} [My stomach hurts]
        -> eating_theory_yes
    
    * {response} [Nothing to say]
        -> eating_theory_yes
    
    * {response == false} [So what?!]
        -> eating_theory_no
    
    * {response == false} [Does it matter?]
        -> eating_theory_no
        

=== eating_theory_yes ===
Yes, a most logical explanation. You don't feel full, but you are. Interesting, very interesting indeed. I will note it down.
    -> eating_theory_end
    
    
=== eating_theory_no ===
Iver, you are no man of science. You don't seem to comprehend the complexity of the world around you. What a shame. But it doesn't matter.
    -> eating_theory_end

=== weather_theory_end ===
{
    - HansNielsFirstWeatherTheory: Ahoy Iver! Have you come to report how you are feeling?
        ~ HansNielsFirstWeatherTheory = false
    - HansNielsWeatherTheoryReturn: You came back.
        ~ HansNielsWeatherTheoryReturn = false

}

    * {HansNielsWeatherTheoryPicked == false} [Yes]
        ~ HansNielsWeatherTheoryPicked = true
        -> weather_theory(true)
        
    * {HansNielsWeatherTheoryPicked == false} [No]
        ~ HansNielsWeatherTheoryPicked = true
        -> weather_theory(false)
        
    + {HansNielsWeatherTheoryPicked} [It has changed]
        -> weather_feelings
    
    * [See you]
        ~ HansNielsWeatherTheoryReturn = true
        Yes, yes see you Iver.
            -> END


=== weather_theory(response) ===
{ 
    - response == false: Oohh, that's alright. I have gathered enough about Ejnar alone to answer my questions.
        -> END
    - HansTemperaturAmountOfChanges < 5: -> weather_feelings
    - HansTemperaturAmountOfChanges < 10 : With the amount of times you have reported, I do not think you are needed for my report anymore. Thank you Iver.
        -> END
    - HansTemperaturAmountOfChanges < 100 : I do not need any more reports, please.
        -> END
    - HansTemperaturAmountOfChanges < 10000 : By my accounts, you are way too interested in the weather to be unbiased. Please leave me alone Iver.
        -> END
    
   
}


=== name_theory_end ===
{
    - HansNielsFirstNameTheory: Ahoy Christian, how are you doing?
        ~ HansNielsFirstNameTheory = false
    - HansNielsNameTheoryReturn && HansNielsNameTheoryPicked == false: Ahoy Marcus, what do you want?
    - HansNielsNameTheoryReturn: He returns, but I have nothing to say to him. I will just smile and wait until he leaves.
}
    * {HansNielsNameTheoryPicked == false} [I'm Iver]
        -> name_theory("Iver")
    
    * {HansNielsNameTheoryPicked == false}[Just fine]
        -> name_theory("Fine")
        
    * {HansNielsNameTheoryPicked == false}[Not good]
        -> name_theory("Not good")
    
    * [See you]
        ~ HansNielsNameTheoryReturn = true
        See you.
        -> END


=== name_theory(response) ===
~ HansNielsNameTheoryPicked = true
~ HansNielsEatingTheoryReturn = true
{
    - response == "Iver": Subject recalls his name. There must be something in the subconscious that tells him something. The cold is getting to him.
        
    - else: {response}, how curious. Subject does not react to me giving the wrong name. He must be more affected than I thought. Interesting, very interesting.
}
        -> END   

=== hans_becomes_insane ===
Ahoy Iver. What could a humble servant like Hans do for you?

    * [Got any new theories?]
        Theories, yes. But are they of any interest to you now, are they? You didn't want to hear about them before, so you will not hear them now!
            -> END
    
    * [Nothing]
        No, of course not. What would I, Hans, have to theorise that the great Iver would ever like to hear? Nothing. Of course, nothing.
            -> END



=== card_main === 
{
    - HansBecomeInsane: -> hans_becomes_insane
    - HansGotTheMap: I have everything I need. Go talk to Ejnar. -> END
    - HansDidNotGetTheMap: Please just leave. I do not need the map anyways -> END
    - else: Do you have the map? Please give it, then I can compare it to the old ones.
}

    * [Here is the map]
        ~ HansGotTheMap = true
        Thank you Iver. I will get to work immediately.
        -> END
        
    * [No]
        What! You must give it to me, it is what I am here for Iver!
        
        * * [Just joking]
            ~ HansGotTheMap = true
            I did not find that funny. Why did I not? Hhhmmm, I can feel a new theory brewing.
                -> END
                
        * * [I'm not giving it]
            ~ HansDidNotGetTheMap = true
            Then leave me be. And do not come back!
                -> END


