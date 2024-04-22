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
        Where do you think? Greenland of course, we have spend the last 6 months getting here.
        -> main
        
    * [See you]
        Yes, I'll see you
            ~LaubMainReturn = true
        -> END


VAR firstWhoAreYouAsk = true
=== who_are_you ===
    * {firstWhoAreYouAsk} [Is that your full name?]
        No.
        
        * * [Do you want to tell me?]
            Not really!
            
            * * * [Are you sure?]
                Yes, now stop asking
                -> pestering_laub_for_his_name
                
            * * * [Okay]
                Thank you!
                -> who_are_you
            
        * * [Okay]
            Thank you
            -> who_are_you
            
            
    * [What do you do on the ship?]
        I am here to ensure the safety of this mission, nothing must go wrong. 
        ~ firstWhoAreYouAsk = false
        -> who_are_you
        
    * [Back to other questions]
        What do you want to know?
        -> main


VAR pesterings = 0
=== pestering_laub_for_his_name === 
    + [Please, I need your full name.]
        {pesterings > 4:
            -> pissed_off_laub
         - else:
            No, you do not.
            ~ pesterings++
            -> pestering_laub_for_his_name
        }
        
    + [Ejnar told me he needed it.]
        {pesterings > 4:
            -> pissed_off_laub
        - else:
            He knows it aleady, there is no need for me to tell him.
            ~ pesterings++
            -> pestering_laub_for_his_name
        }
        
    * [Okay]
        Thank you, now go away from me.
        -> END


=== pissed_off_laub ===
    BE QUIET AND STOP PESTERING ME!
    #pissedOffLaub
    -> END

=== mission ===
    * { LaubMainAgreeWithLaub } [I could try talking to Ejnar]
        Try to do that, he will need some evidence that this mission is impossible first.
        -> mission

    * [I agree]
        Finally someone with an ounce of sense, maybe you can talk some into Ejnar.
        ~ LaubMainAgreeWithLaub = true
        ~ LaubMainFirstTimeMissionAsk = false
        -> mission
     
    * [{LaubMainFirstTimeMissionAsk: What makes you think that? | Why is it impossible? }]
        
        We are here during the Winter, it gets brutally cold!
        ~ LaubMainFirstTimeMissionAsk  = false
        -> mission
        
    * [ Could you explain why? ]
        Because almost all our provisions are used up, we had to slaughter the last dog to get food!
        ~ LaubMainFirstTimeMissionAsk  = false
        -> mission
        
    * [Back to other questions]
        What do you want to know?
        -> main 







=== joergen_main ===
-> END

=== niels_main ===
-> END

=== ludvig_main ===
-> END


=== card_main === 
-> END