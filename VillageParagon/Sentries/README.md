Here is the informations about each sentry.

# What is tier5Counts[x] ?
Like vanilla Paragons, the Village paragon increases in power the higher its degree is. But in order to fully get access to this power increase, you must sacrifice 5 T5 towers of each type. **Not every sacrifice buff every sentry**. Here is which tower type buffs which sentry with the ID:
```
(0) Primary -> Solver Sentry
(1) Military -> Big Boi Sentry
(2) Magic -> Group Sentry
(3) Support -> Farming Sentry
```
So tier5Counts[x] is equal to the number of sacrificed towers of the associated type (x = 3 for the Farming Sentry).

# Farming Sentry ![Farming Sentry Picture](farming_sentry.png)
Description: 
```
Spawn a money geyser for 5 seconds. 
The sentry is spawned every 60 seconds.

Value of the crates = 625 + 625 * degree / 100 * tier5Counts[3] / 5
Rate = 0.05
```

# Big Boi Sentry ![Big Boi Sentry Picture](big_boi_sentry.PNG)
```
Deals huge single target damage for 120 seconds.
The sentry is spawned every 30 seconds.

Rate = 0.15f - degree / 1000f
Damage = 240 + Math.FloorToInt(960f * degree / 1000f) * 10 * tier5Counts[1] / 5
```

# Group Sentry ![Group Picture](group_sentry.PNG)
```
Deals huge multi target damage for 60 seconds.
The sentry is spawned every 60 seconds.

Ability cooldown = 15 - 10 * degree / 100 * tier5Counts[2] / 5
Damage = 4 + 12 * degree / 100 * tier5Counts[2] / 5
Pierce = 18 + 54 * degree / 100 * tier5Counts[2] / 5
```

# Solver Sentry ![Solver Picture](solver_sentry.PNG)
```
Deals slow bloons for 30 seconds.
The sentry is spawned every 30 seconds.

Rate = 0.15f - 0.1f * degree / 100 * tier5Counts[0] / 5
Slow Moab Multiplier = 4  + 2 * degree / 100 * tier5Counts[3] / 5
Slow BFB Multiplier = 0.2f + 0.4f * degree / 100 * tier5Counts[3] / 5
Slow ZOMG Multiplier = 0.4f + 0.8f * degree / 100 * tier5Counts[3] / 5
Slow BAD Multiplier = 3f + 3f * degree / 100 * tier5Counts[3] / 5
```
