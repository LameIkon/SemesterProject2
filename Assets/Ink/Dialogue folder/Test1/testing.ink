This is a dialogue test. First we got the size of the text:
->main

===main===
* [short answer]
    pellentesque nec placerat eget, consectetur rutrum elit. 
    ->main
* [mid answer]
    Praesent dictum nulla sit amet mi porta, ac dapibus ipsum consequat. Integer lobortis lectus sit amet lorem auctor
    ->main
* [long answer]
   Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus ante velit, viverra nec iaculis et, consectetur et magna. Vestibulum condimentum ac tellus vitae gravida. Proin sapien magna
   ->main
* [Next Test]
  Do you prefer if you answer is within the answerbox or its own textbox
  ->test1
  
===test1===
* [start test]
    Hello, what can i do for you?    
    ->answer
*[End test]
    ending test
->END

===answer===
* [(within)Where am i?]
    You are in nothern most part of the world where partly any human has ever been.
    ->test1
* [(own)What is this place?]
    What is this place?
    ->answer2

===answer2===
* [next]
You are in nothern most part of the world where partly any human has ever been.
->test1

       
