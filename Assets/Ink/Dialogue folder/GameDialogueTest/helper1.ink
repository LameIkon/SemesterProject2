INCLUDE globals.ink


{variable1: -> main | -> notMain}


=== main ===
You are in main because variable1 is true

    * [set variable2 to true. ]
    ~ variable2 = true
        -> END

    * [set variable2 to false. ]
    ~ variable2 = false
        -> END



=== notMain ===
You are in notMain because variable1 is false. Go to the other NPC to change it
    -> END