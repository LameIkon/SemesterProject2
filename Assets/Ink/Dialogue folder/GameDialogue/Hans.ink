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
    
    - HansMainReturn && HansNewTheory: You want something? 
}
        ~ HansMainReturn = false
 
    + {HansTemperaturFeeling} [Yes, it has changed]
        -> weather_feelings
    
    * {HansWeatherTheory == false} [What are you working on?]
        -> working_on
        
    * {HansNewTheory} [Any new theories?]
        Negative, It will take sometime before new ones come to me.
            -> main
        
    
    * [What do on the ship?]
        -> what_do_you_do
    
    * [See you]
        See you too.
        ~ HansMainReturn = true
        -> END
        
        
=== working_on ===
Right now I am on the brink of brilliance. I came with the intent to learn about how some people don’t feel the cold, as much as others.
    
    *[What have you found out?]
        -> first_theory
    
    *[Okay]
        Do you want to hear about it?
            
            * * [Yes]
                -> first_theory
            
            * * [No]
                How come I promise it is really exciting
                
                    * * * [Okay, go ahead]
                        -> first_theory
                    * * * [I'm just not interested]
                        -> new_theory
        
        

=== first_theory ===
    ~HansWeatherTheory = true
    For now, I have deduced that I feel cold, while Ejnar is not. But he has also been here before. Maybe if I go outside and stay in the snow I’ll feel less cold when I come back in the ship.
    
    *[Anything else]
        No, but maybe you could report back to me how you feel?
            
            * * [Yes, I could do that]
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
    - HansTemperaturAmountOfChanges < 5 :Oohh, how exciting, please tell me.
    - HansTemperaturAmountOfChanges < 10 : You have now reported back {HansTemperaturAmountOfChanges} times, the data is getting better the more you report back. 
    - HansTemperaturAmountOfChanges < 25 : With {HansTemperaturAmountOfChanges} changes in how you feel, I think you do not have to report more, you are of course welcome to.
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
Noted, please report back if this ever changes.
~HansTemperaturFeeling = var
{
    - ludvigDiaryFound: -> ludvig_main
    - nielsDiaryFound: -> ludvig_main
    - joergenDiaryFound: -> joergen_main
    - else: -> main

}


VAR HansMainAskWhatHeDoes = false
=== what_do_you_do ===
{HansMainAskWhatHeDoes: | I'm Hans, scientist from the University of Copenhagen, I'm here to conduct experiments and it will be me who has the responsiblity to consturct the final card from the sketches. }

    * [What have you studied]
        ~ HansMainAskWhatHeDoes = true
        I have studied quiet a lot, but my main intrest is humans and weather. How does one live in such a place like Greenland volentary? My themometer cannot mesure temperatures so low.
        -> what_do_you_do
    
    * [Back to other questions]
        Sure, anything else I can help with?
        -> main



=== new_theory ===
Hmpf, you just gave me a new theory I'll have to think out. It will be even better than the last.
    ~ HansNewTheory = true
-> main











=== joergen_main ===
{
    - HansJoergenFirstTimeMain: Oh, you are back already. {HansWeatherTheory: Do you have anything to report? Maybe you have something new about the cold.} {HansNewTheory: I have a new theory}
        ~HansJoergenFirstTimeMain = false
    - HansJoergenReturn: Anything else you would like? 
        ~HansJoergenReturn = false
}

    + {HansWeatherTheory && HansJoergenTheoryHeard}[I would like to report]
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
        
    * [Who where you again?]
        Me? I Hans scientist {HansBecomeInsane: not that it means anything to someone like you.} {HansWeatherTheory: currently occupide by your reports from the outside.} {HansNameTheory: Hans, name: Hans occupation: scientist. You remember that now.} {HansNewTheory: currently I have a new theory for you to hear. } {HansEatingTheory: have you tried to eat something today?}
                -> joergen_main
    
    * [See you]
    ~ HansJoergenReturn = true
    {
        - HansWeatherTheory && HansJoergenTheoryHeard: See you, and please report back   
        - HansNewTheory && HansJoergenTheoryHeard: See you 
        - else: See you
    }
            -> END




=== nothing_to_report ===
That cannot be, no no no no no. You must have something, anything, things you must have. 
    * [Yes I have something]
        You are a funny man Iver, yes of course you have something to report.
            -> weather_feelings

    * [No, nothing to report]
        ~HansWeatherTheory = false
        Yes it makes sense, you are forgetting your struggle from the outside. Yes that must be it, it is so cold that you cannot remember. Do you remember the name of the captain?
        
        * * [Laub]
            -> does_not_remember_captain("Laub")
        * * [Hans]
            -> does_not_remember_captain("Hans")
        * * [Ejnar]
            No, my hypotesis must be wrong. Can it be wrong, no he must have guessed and gotten it right. Yes that's it, it can't be wrong, I can't be wrong.
                -> joergen_main
        * * [Who?]
            -> does_not_remember_captain("Who")



=== does_not_remember_captain(name) ===
~ HansNameTheory = true
{name}, interesting, very interesting. {name == "Who": Subject does not remember that we have a captain. | Subject remembers a name but not the correct one.} Yes my theory must be true. 
    -> joergen_main
    


=== new_theory_joergen ===
~HansNewTheory = false
Ooohh yes I do. Would you like to hear it?
    * [Yes]
        I cannot wait to tell you
            -> eating_theory
    
    * [No, not really]
        Ooh, why not not!?
        
            * * [I am just joking]
                So funny Iver, yes funny, that you are.
                    -> eating_theory
            
            * * [I just don't]
            ~HansBecomeInsane = true
                No, of course not, who would even want to listen to someone like me. No one that is who.
                -> joergen_main


=== eating_theory ===
~HansEatingTheory = true

Something strange happens here on Greenland. It seems that one can eat and eat and not become full. Have you tried this?
    
    * [Yes]
        Yes, stange is it not. I still have no good explantion for this phenomena.
        -> joergen_main
    
    * [No]
        Stange, how long does one have to be here to get to this level? 
        -> joergen_main


=== ludvig_main ===
{HansBecomeInsane: -> hans_becomes_insane}
    


-> END


=== hans_becomes_insane ===
Ahoy Iver, what could a humble servant like Hans do for you?

    * [Got any new theories?]
        Theories, yes. But they are of no intrest for you, now are they? You did not want them before, so you shall not have them now!
            -> END
    
    * [Nothing]
        No, of course not. What would I Hans have to theorise that the great Iver would ever like to hear, nothing. Of course nothing.
            -> END

-> END


=== card_main === 
-> END
