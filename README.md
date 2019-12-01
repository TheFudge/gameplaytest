# gameplaytest

Ich habe versucht den Character so erweiterbar wie möglich zu bauen, ohne zu viel Zeit in die Programmierung reinzu stecken.
Daher habe ich mich gegen Ende auch für einen weiteren Enum entschieden, damit man neue Bewegungsarten wie z.B. Schleichen einfach einbauen kann.

Es war zwar nicht in der Aufgabe, aber ein kleine Kamerascript hab auch geschrieben, damit man besser testen kann.
Die Testmap sollte alle möglichen Interaktionen testen können. 

Damit Gravity und andere Physics-Elemente funktionieren, habe ich mich dazu entschieden, kein eigenes Physics-Sstem zu bauen, sondern das eingebaute System von Unity zu verwenden. Ich glaube, dass das später einfacher zu erweitern ist als eine Custom Lösung.

Die Controller Skripte sind komplett vom Character losgelöst, damit man auch andere Controller leicht anbauen kann oder andere InputSysteme (z.B. das neue Input System.)