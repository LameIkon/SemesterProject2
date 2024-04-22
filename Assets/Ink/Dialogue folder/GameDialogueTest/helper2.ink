INCLUDE globals.ink


{variable2: -> main | -> notMain}


=== main ===
You are in main because variable2 is true

    * [set variable1 to true. ]
    ~ variable1 = true
        -> END

    * [set variable1 to false. ]
    ~ variable1 = false
        -> END



=== notMain ===
You are in notMain because variable1 is false. Go to the other NPC to change it.
    -> END