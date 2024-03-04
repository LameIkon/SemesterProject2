This is test 2
->main

===main===
* [<i>"lets rest for now"] 
    Alright, let's stop up for now #testTag4
    ->END
* [<i>"Let's continue for a while more"]
    we shall continue then #testTag3
    ->continue
* [Ignore]
   <i>"Hello, can you hear me?? why aren't you answering me?"
   ->next
   
===next===
* [turn around and start to eat your crewmate]
    <i>"Oh, you finally noticed i was talking to you... why are you ominously moving towards me?" #testTag1
    ->third

=third
* [take a bite]
    You take a huge bite 
    <i>"The captain has gone crazy, SHOOT HIM!"
    <color=red> die from gunshot</color> #testTag2
    ->END

===continue===
*[let's stop now]
    We have walked for a while now, I think we should rest now before it becomes too dangerous 
    ->main
       
