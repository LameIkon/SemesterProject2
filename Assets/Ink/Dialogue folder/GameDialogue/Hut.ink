-> start

=== start ===
You stand at the door. No light or sound is coming from inside. Maybe they didn't make it?
*[Knock on the door] -> knock
*[leave]-> end_dialogue_without_knowing
    

=== knock ===
You decide to knock on the door
*[continue]
    ...
    **[continue]
        ...
        ***[continue]
            There seems to be nobody inside.
            
            ****[try knocking again]
                    -> knock_again
                
            ****[leave]
                    -> conclusion

=== knock_again ===
you decide to knock on the door again
*[continue]
     ...
    **[continue]
        ...
        ***[continue]
            Still nothing
            ****[leave]
                    -> conclusion

=== end_dialogue_without_knowing ===
You decide to leave
    ->END

=== conclusion ===
you conclude there isn't any inside.
    -> END