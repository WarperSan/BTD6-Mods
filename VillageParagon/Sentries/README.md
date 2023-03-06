Here is the informations about each sentry.

# What is S, B, G, and F?
Like vanilla Paragons, the Village paragon increases in power the higher its degree is. But in order to fully get access to this power increase, you must sacrifice 5 T5 towers of each type. **Not every sacrifice buff every sentry**. Here is which tower type buffs which sentry with the variable name:
```
Primary Sacrifice -> Solver Sentry (s)
Military Sacrifice -> Big Boi Sentry (b)
Magic Sacrifice -> Group Sentry (g)
Support Sacrifice -> Farming Sentry (f)
```

# Solver Sentry ![Solver Picture](solver_sentry.PNG)
```
Deals slow bloons for 60 seconds.
The sentry is spawned every 60 seconds.

     speed(x) = 0.15 - 0.001(sx)
moab_speed(x) = 4    + sx / 150
 bfb_speed(x) = 0.2  + 0.0008(sx)
zomg_speed(x) = 0.4  + 0.0016(sx)
 bad_speed(x) = 3    + 0.006(sx)
```

# Big Boi Sentry ![Big Boi Sentry Picture](big_boi_sentry.PNG)
```
Deals huge single target damage for 120 seconds.
The sentry is spawned every 30 seconds.

 speed(x) = 0.15 - x / 1,000
damage(x) = 240  + 2⌊(960/1,000)x⌋b
```

# Group Sentry ![Group Picture](group_sentry.PNG)
```
Deals huge multi target damage for 60 seconds.
The sentry is spawned every 60 seconds.

cooldown(x) = 15 -  0.02gx
  damage(x) =  4 + 0.024gx
  pierce(x) = 18 + 0.108gx
```

# Farming Sentry ![Farming Sentry Picture](farming_sentry.png)
Description:
```
Spawn a money geyser for 5 seconds. 
The sentry is spawned every 60 seconds.

crate_value(x) = 625 + 1.25x
      speed(_) = 0.05
```
