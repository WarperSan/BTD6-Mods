This mod adds a paragon to the Monkey Sub

WARNING : For some reason, you can't upgrade the bottom path to a paragon. The paragon will glitch out if you do

Name of the Paragon : USS Seawolf-575

In game description: "After YEARS of making, I finally succeeded to create the perfect radioactive destroyer. It can destroy anything! Even... Oh no... I forgot about DDTs... - Dr. Monkey"

Stats for nerds (__let x be the Paragon Degree__):
```
range(_): 90 units
cost(_): $450,000
```

Base Darts Projectile (005 Sub's Darts):
```
1..49:
    speed(_) = 300
    damage(x) = ⌊x / 10⌋ + 16
   speed(x) = -0.00003(x - 1) + 0.03

50..100:
    speed(x) = 6.5x
    damage(x) = x / 10 + 16
   speed(x) = -0.00003(x - 1) + 0.03
```

Fraction Projectile (005 Sub's Darts' Projectile)
```
1..24:
     speed(_) = 900
    damage(_) = 8
     count(_) = 7

25..74:
     speed(_) = 900
    damage(x) = x / 8 + 8
     count(_) = 9

75..100:
     speed(_) = 900
    damage(x) = x / 8 + 8
     count(_) = 13
```

Big Missile (missing areas)
```
1..49:
    damage(_) = 20,000
     speed(x) = (5/7)x + 10
     count(_) = ???

50..74:
    damage(_) = ???
     speed(_) = ???
     count(_) = ???

75..99:
    damage(_) = ???
     speed(_) = 60
     count(_) = ???

100:
    damage(_) = ???
     speed(_) = 20
     count(_) = ???
```

Normal Missile
```
1..100:
    damage(x) = 2000(⌊x / 10⌋ + 1)
     speed(_) = 0.5
```

Radioactive Zone
```
1..49:
      damage(_) = 500
    interval(_) = 0.1
    lifespan(_) = 20

50..100:
      damage(_) = 500
    interval(_) = 0.05
    lifespan(x) = 0.4x + 20
```

Pre Emptive Strike
```
1..100:
    damage(_) = 1500
```
