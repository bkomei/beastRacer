character you = "You"
character N = "Noda"
character V = "Velo"
character anon = "???"
character VR = "Varmint" 
character BL = "Bullet"


label start:
    you "Mm... "
	you ".....nhh."
    call intro_runin
	"   "
	call intro_cooloff
    "Okay, on to the next thing..."
    call dirty_clothes
    jump end

label intro_runin:
    anon "Hey, watch it!"
    you "Ah, shit, my bad. Didn't see you there."
	anon "Hmph, spectator seats are back out the doors and to the left."
    you "What's that?"
	anon "You're lost, right? Clearly. Spectators follow the arrows out there."
	you "Ooh, nah! I'm here for the tournament. Thanks, tho."
	anon "You're a racer...? Where's your ride, then?"
	you "Uh, right here? Ain't got a proji."
	anon "... "
	anon "Pfft. Right... Look, lemme be rank with you. You should really save yourself the entry fee and catch a good seat in the audience."
	anon "SAVG Tournament in the biggest tournament in the region and really not a place for beginners." 
	anon "This is only the qualifiers, and you'll be eaten alive." 
	you "... "
	you "What's your name?"
	anon "... "
	anon "They/her. Those are my pronouns. You'll hear my name in the top three if you stick around."
	you "Fair enough."
	N "My name's Noda, he/they." 
	anon "I didn't ask for your name."
	N "You didn't, but you'll remember it. I'll see you in the next round!"
	anon "...Ew."
	return
	
	
label intro_cooloff:
	N "That went well."
	
    menu:
        "I'm so smooth.":
            signal yes
            N "Bet."
        "It'll be embarrassing if I lose now.":
            signal no
            N "So, I better not lose."
	N "Now where was that sign up desk?"
	anon "/grrr/"
	N "Whaat, Velo? It's going to be fine."
	
    return

label dirty_clothes:
    "There's a pile of dirty clothes."
    "*sniff*"
    p "BLEAH, gross."
    p "...but I have nothing clean to wear..."
    "Should I wash them?"
    menu:
        "Yes":
            signal yes
            p "Well, at least if I'm late I'll be clean."
        "No":
            signal no
            p "Ugh, I guess I'll rewear the least smelly outfit I have."
    return

label end:
    "That's the end of this demo."
    return
