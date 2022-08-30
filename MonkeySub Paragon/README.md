This mod adds a paragon to the Monkey Sub

Name of the Paragon : USS Seawolf-575

In game description: "After YEARS of making, I finally succeeded to create the perfect radioactive destroyer. It can destroy anything! Even... Oh no... I forgot about DDTs... - Dr. Monkey"

Details about the Paragon:
```
Range: 90
```

Base Darts Projectile (005 Sub's Darts):
```
All degrees
Damage = degree / 10 + 16
Rate = -0.00003 * (degree - 1) + 0.03

From degree 1 to 49
Speed = 300

From degree 50
Speed = 6.5f * degree;
```

Fraction Projectile (005 Sub's Darts' Projectile)
```
All degrees
Speed = 900

From degree 1 to 24
Count = 7
Damage = 8

Upon degree 25
Damage = degree / 8 + 8

From degree 25 to 74
Count = 9

Upon degree 75
Count = 13
```

Big Missile
```
From degree 1 to 74
Rate = 5 / 7 * degree + 10;

From degree 1 to 49
Damage = 20000

From degree 75 to 99
Rate = 60

From degree 100
Rate = 20
```

Normal Missile
```
All degrees
Damage = 2000
Rate = 0.5
```

Radioactive Zone
```
All degrees
Damage = 500

From degree 1 to 49
Interval = 0.1
Lifespan = 20

Upon degree 50
Interval = 0.05
Lifespan = 0.4 * degree + 20
```

Pre Emptive Strike
```
Damage = 1500
```
