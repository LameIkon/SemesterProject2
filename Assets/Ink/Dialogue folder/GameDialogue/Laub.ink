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
    - LaubMainFirstTime: Hello what do you want?
        ~LaubMainFirstTime = false
    - LaubMainReturn: Anything else?
        ~LaubMainReturn = false
}


    * [Who are you?] 
        I am first lieutenant Laub.
        -> who_are_you
        
    * [What do you think of the mission?]
        I think it is an impossible one.
        -> mission
        
    * [Where are we?]
        Where do you think? Greenland of course. We have spent the last 6 months getting here.
        -> main
        
    * [See you]
        Yes, I'll see you.
            ~LaubMainReturn = true
        -> END


=== who_are_you ===
    * {LaubMainFirstTimeNameAsk} [Is that your full name?]
        No.
            -> asking_laub_for_his_name
    
    *{LaubMainFirstTimeNameAsk == false} [What is your full name?]
        ...
            -> asking_laub_for_his_name
            
    * [What do you do on the ship?]
        I am here to ensure the safety of this mission. Nothing must go wrong. 
        ~ LaubMainFirstTimeNameAsk = false
        -> who_are_you
        
    * [Back to other questions]
        What do you want to know?
        -> main


=== asking_laub_for_his_name ===
    * [Do you want to tell me?]
        {LaubMainFirstTimeNameAsk: Not really!| My name is not of importance to you.}
        
        * * [Are you sure?]
            Yes, now stop asking.
            -> pestering_laub_for_his_name
            
        * * [Okay]
            Thank you.
            -> who_are_you
        
    * [Okay]
        Thank you.
        -> who_are_you


=== pestering_laub_for_his_name === 
    + [Please, I need your full name]
        {LaubMainPersteringLaub > 4:
            -> pissed_off_laub
         - else:
            No, you do not.
            ~ LaubMainPersteringLaub++
            -> pestering_laub_for_his_name
        }
        
    + [Ejnar told me he needed it]
        {LaubMainPersteringLaub > 4:
            -> pissed_off_laub
        - else:
            He knows it already, there is no need for me to tell you.
            ~ LaubMainPersteringLaub++
            -> pestering_laub_for_his_name
        }
        
    * [Okay]
        Thank you. Now, go away.
        -> END


=== pissed_off_laub ===
    BE QUIET AND STOP PESTERING ME!
    -> END

=== mission ===
    * { LaubMainAgreeWithLaub } [I could try talking to Ejnar]
        Try to do that, he will need some evidence that this mission is impossible first.
        -> mission

    * [I agree]
        Finally, someone with an ounce of sense. Maybe you could talk some into Ejnar.
        ~ LaubMainAgreeWithLaub = true
        ~ LaubMainFirstTimeMissionAsk = false
        -> mission
     
    * [{LaubMainFirstTimeMissionAsk: What makes you think that? | Why is it impossible? }]
        
        We are here during the winter, it gets brutally cold!
        ~ LaubMainFirstTimeMissionAsk  = false
        -> mission
        
    * [Could you explain why?]
        Because almost all our provisions are used up. We had to slaughter the last dog to get food!
        ~ LaubMainFirstTimeMissionAsk  = false
        -> mission
        
    * [Back to the other questions]
        What do you want to know?
        -> main 





=== joergen_main ===
What do you want?

    * [I found Joergen]
        Do I look like the captain of the expedition? Go talk to Ejnar about it, not me!
            -> END

    * [Nothing]
        Great, then leave me alone! I am busy.
            -> END

    * [See you]
        Goodbye
            -> END


=== niels_main ===
Can I help you with anything?
0
    * [I found Niels]
        Do I look like the captain of the expedition? Go talk to Ejnar about it, not me!
            -> END
            
    * [Yes]
        Go and talk to Ejnar then!
            -> END

    * [No]
        Great, then I have nothing more to say to you.
            -> END

    * [See you]
        Goodbye.
            -> END

=== ludvig_main ===
What now?

    * [I found Ludvig]
        Dead persumably? Such is the rules of this land.
            -> END

    * [Nothing]
        Great, go talk with Hans then.
            -> END

    * [See you]
        Goodbye.
            -> END


=== card_main === 
What do you want now?
    * [I found the map]
        Finally. At last we can get away from this horribly cold place. You made my day Iver. Talk with Ejnar about it.
            -> END

    * [Nothing]
        Then bother someone else!
            -> END

    * [See you]
        Goodbye.
            -> END