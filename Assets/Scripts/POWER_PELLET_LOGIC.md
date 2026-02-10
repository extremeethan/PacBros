# Power Pellet – Full Logic (Who Calls What)

## PART 1: Player eats the power pellet

| Step | Who | What happens |
|------|-----|--------------|
| 1 | **Unity** | Player’s collider enters the power pellet’s trigger. |
| 2 | **Pellet** (base) | `OnTriggerEnter2D(other)` runs. Checks: `other.layer == "Player"`. |
| 3 | **Pellet** | Calls `Eat()`. (PowerPellet overrides this.) |
| 4 | **PowerPellet** | `Eat()` runs → calls `GameManager.PowerPelletEaten(this)`. |

So: **Pellet (trigger) → PowerPellet.Eat() → GameManager.PowerPelletEaten(powerPellet)**.

---

## PART 2: GameManager.PowerPelletEaten(powerPellet)

| Step | Who | What happens |
|------|-----|--------------|
| 1 | **GameManager** | Calls `PelletEaten(powerPellet)`. |
| 2 | **GameManager.PelletEaten** | Pellet is deactivated, score updated, “all pellets gone” checked (can trigger LoadNextLevel). |
| 3 | **GameManager** | `Invoke(nameof(ResetGhostMultiplier), powerPellet.duration)` → in 8s (or `duration`) calls `ResetGhostMultiplier()`. |
| 4 | **GameManager** | Sets `PowerPelletEatenTime = Time.time` (used to block “ghost eaten” for 2 seconds). |
| 5 | **GameManager** | For each ghost in `ghosts[]`: |

---

## PART 3: For each ghost (inside the loop)

| Step | Who | What happens |
|------|-----|--------------|
| 1 | **GameManager** | `g.chase.Disable()`. |
| 2 | **GhostChase** | `OnDisable()` runs → calls `ghost.scatter.Enable()`. So **chase off, scatter on**. |
| 3 | **GameManager** | `g.scatter.Disable()`. |
| 4 | **GhostScatter** | `OnDisable()` runs → calls `ghost.chase.Enable()`. So **scatter off, chase on**. |
| 5 | **GameManager** | `g.frightened.Enable(powerPellet.duration)`. |
| 6 | **GhostFrightened** | `Enable(duration)` runs: `base.Enable(duration)` → behavior enabled + `Invoke(Disable, duration)`. Sets frightened sprite, `_ignoreEatUntil = Time.time + 1`. |

So after the loop, each ghost has **Chase** and **Frightened** both enabled (scatter and chase keep re‑enabling each other when disabled). Frightened will turn itself off after `powerPellet.duration` seconds.

---

## PART 4: GhostBehavior.Enable(duration) (base)

| Step | Who | What happens |
|------|-----|--------------|
| 1 | **GhostBehavior** | `this.enabled = true` (e.g. GhostFrightened is now enabled). |
| 2 | **GhostBehavior** | `Invoke(nameof(Disable), duration)` → in `duration` seconds, `GhostFrightened.Disable()` will run. |

---

## PART 5: When frightened duration ends (later, via Invoke)

| Step | Who | What happens |
|------|-----|--------------|
| 1 | **GhostFrightened** | `Disable()` runs: base disable, normal sprite, `ghost.scatter.Enable()`. |
| 2 | **GameManager** (same time) | `ResetGhostMultiplier()` runs (from the Invoke in PowerPelletEaten). |

---

## PART 6: When the player touches a ghost (two cases)

### Case A: Ghost is NOT frightened (normal / chase / scatter)

| Step | Who | What happens |
|------|-----|--------------|
| 1 | **Ghost** | `OnCollisionEnter2D` or `OnTriggerEnter2D` with the player. |
| 2 | **Ghost** | `IsPlayer(...)` true, `frightened.enabled` false → calls `GameManager.PacmanEaten()`. |
| 3 | **GameManager** | Pacman hidden, lives decreased, then either `Invoke(ResetState, 3f)` or `GameOver()`. |

So: **Ghost (collision/trigger) → GameManager.PacmanEaten()** (player loses).

---

### Case B: Ghost IS frightened (player eats ghost)

| Step | Who | What happens |
|------|-----|--------------|
| 1 | **GhostFrightened** | `OnTriggerEnter2D(other)` or `OnCollisionEnter2D(collision)` with the player. |
| 2 | **GhostFrightened** | If it’s a **Node**: pick direction farthest from target (run away). If it’s the **player** and `Time.time >= _ignoreEatUntil`: calls `GameManager.GhostEaten(ghost)`. |
| 3 | **GameManager.GhostEaten** | If `Time.time - PowerPelletEatenTime < 2f` → **return (do nothing)**. Else: add score, increase multiplier, call `ghost.EatenByPlayer()`. |
| 4 | **Ghost.EatenByPlayer()** | Frightened/chase/scatter disabled, dead sprite, movement off, teleport to `home.mazeStartPosition`, start `RespawnAfterEaten()` coroutine. |
| 5 | **Ghost** (3 seconds later) | Coroutine: normal sprite, movement on, `scatter.Enable()`. |

So: **GhostFrightened (collision/trigger with player) → GameManager.GhostEaten(ghost) → ghost.EatenByPlayer()** (ghost “dies” and respawns at home).

---

## Summary chain: “Player eats power pellet”

1. **Pellet** (trigger) → **PowerPellet.Eat()** → **GameManager.PowerPelletEaten(powerPellet)**  
2. **PowerPelletEaten** → **PelletEaten** (pellet off, score, maybe load next level)  
3. **PowerPelletEaten** → **Invoke(ResetGhostMultiplier, duration)**  
4. **PowerPelletEaten** → **PowerPelletEatenTime = Time.time**  
5. **PowerPelletEaten** → for each ghost: **chase.Disable()** → **scatter.Enable()** → **scatter.Disable()** → **chase.Enable()** → **frightened.Enable(duration)**  
6. **GhostFrightened.Enable** → sprite = frightened, **Invoke(Disable, duration)**, _ignoreEatUntil = now + 1s  

No one calls **GhostEaten** or **EatenByPlayer** in this path. Those are only called when the **player** and a **frightened ghost** touch (and only after the 2‑second block and the 1‑second ignore window).

---

## Important detail: scatter/chase ping‑pong

When you do:

- `g.chase.Disable()` → Chase.OnDisable → **scatter.Enable()**
- `g.scatter.Disable()` → Scatter.OnDisable → **chase.Enable()**

you end up with **Chase** enabled again, so each ghost has **Chase + Frightened** on at the same time. Only **Frightened** should run during power pellet; the “who calls what” for the pellet itself is above; fixing the scatter/chase ping‑pong would be a separate change so that only Frightened is active.
