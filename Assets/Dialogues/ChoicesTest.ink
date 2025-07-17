INCLUDE globals.ink

{ how == "": -> main | -> already_chose }

-> main

=== main ===
How are you to day bitch?
    + [Ok]
        -> chosen("nha just live one by day")
    + [Good]
        -> chosen("it was my best day")
    + [Bad]
        -> chosen("I'm so f up mannn")
    


=== chosen(How) ===
~ how = How
You chose {How}!
-> END

=== already_chose ===
You already chose {how}!
-> END