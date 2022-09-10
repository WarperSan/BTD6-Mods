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
- How much crates it can generate **at max**: 10 + 10 * degree / 100 * tier5Counts[3] / 5
- Value of each crates: 625 + 625 * degree / 100 * tier5Counts[3] / 5
- Replaced the Crushing sentry

# Big Boi Sentry ![Big Boi Sentry Picture](big_boi_sentry.png)
- How much crates it can generate **at max**: 10 + 10 * degree / 100 * tier5Counts[3] / 5
- Value = 625 + 625 * degree / 100 * tier5Counts[3] / 5
- Replaced the Energy sentry

# Group Sentry ![Group Picture](group_sentry.png)
- Ability cooldown = 15 - 10 * degree / 100 * tier5Counts[2] / 5
- Damage = 4 + 12 * degree / 100 * tier5Counts[2] / 5
- Pierce = 18 + 54 * degree / 100 * tier5Counts[2] / 5
- Replaced the Cold sentry

# Solver Sentry ![Solver Picture](solver_sentry.png)
- Damage = 5 + 15 * degree / 100 * tier5Counts[0] / 5
- Rate = 0.15f - 0.1f * degree / 100 * tier5Counts[0] / 5
- Replaced the Boom sentry
