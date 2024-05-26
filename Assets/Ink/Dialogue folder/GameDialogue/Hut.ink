-> start

=== start ===
You stand at the door. No light or sound is coming from inside. Maybe they didn't make it?
*[Knock on the door] -> knock
*[Leave]-> end_dialogue_without_knowing
    

=== knock ===
You decide to knock on the door
*[Continue]
    ...
    **[Continue]
        ...
        ***[Continue]
            There seems to be nobody inside.
            
            ****[Try knocking again]
                    -> knock_again
                
            ****[Leave]
                    -> conclusion

=== knock_again ===
You decide to knock on the door again
*[Continue]
     ...
    **[Continue]
        ...
        ***[Continue]
            Still nothing
            ****[Leave]
                    -> conclusion

=== end_dialogue_without_knowing ===
You decide to leave
    ->END

=== conclusion ===
You conclude there isn't any inside.
    -> END