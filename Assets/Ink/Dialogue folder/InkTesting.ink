Sir, it has become too cold for us to continue without setting our <color=red>lives at risk</color>. Perhaps we should settle down for the night and then continue tomorrow?
->main

===main===
* [<i>"lets rest for now"]
    Alright, let's stop up for now
    ->END
* [<i>"Let's continue for a while more"]
    we shall continue then
    ->continue
* [Ignore]
   <i>"Hello, can you hear me?? why aren't you answering me?"
   ->next
   
===next===
* [turn around and start to eat your crewmate]
    <i>"Oh, you finally noticed i was talking to you... why are you ominously moving towards me?"
    ->third

=third
* [take a bite]
    You take a huge bite
    <i>"The captain has gone crazy, SHOOT HIM!"
    You die from gunshot
    ->END

===continue===
*[let's stop now]
    test
    ->main
       
