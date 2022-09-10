Here is the informations about each sentry

# What is tier5Counts[x] ?
Like vanilla Paragons, the Village paragon increases in power the higher its degree is. But in order to fully get access to this power increase, you must sacrifice 5 T5 towers of each type. **Not every sacrifice buff every sentry**. Here is which tower type buffs which sentry with the ID:
```
(0) Primary -> Solver Sentry
(1) Military -> Big Boi Sentry
(2) Magic -> Group Sentry
(3) Support -> Farming Sentry
```
So tier5Counts[x] is equal to the number of sacrificed towers of the associated type.

# Farming Sentry ![Farming Sentry Picture](farming_sentry.png)
- Produces Money
- How much crates it can generate **at max**: 10 + 10 * degree / 100 * tier5Counts[3] / 5
- Is placed when the Crushing sentry should be placed
