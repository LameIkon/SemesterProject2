INCLUDE globals.ink

VAR firstTimeAsking = true
-> main
=== main === 
{firstTimeAsking: Hello what do you want? | Anything else?}
~ firstTimeAsking = false

    * [Who are you?] 
        I am first lieutenant Laub.
        -> who_are_you
        
    * [What do you think of the mission?]
        I think it is an impossible one.
        -> mission
        
    * [Where are we?]
        In Greenland, you should know this!
        -> main
        
    * [See you]
        Goodbye
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

VAR agreeWithLaub = false
VAR firstTimeMissionAsk = true
=== mission ===
    * { agreeWithLaub } [I could try talking to Ejnar]
        Try to do that, he will need some evidence that this mission is impossible
        -> mission

    * [I agree]
        Finally someone with an ounce of sense, maybe you can talk some into Ejnar.
        ~ agreeWithLaub = true
        ~ firstTimeMissionAsk = false
        -> mission
     
    * [{firstTimeMissionAsk: What makes you think that? | Why is it impossible? }]
        
        We are here during the Winter, it gets brutally cold!
        ~ firstTimeMissionAsk = false
        -> mission
        
    * [ Could you explain why? ]
        Because almost all our provisions are used up, we had to slaughter the last dog to get food!
        ~ firstTimeMissionAsk = false
        -> mission
        
    * [Back to other questions]
        -> main 
























