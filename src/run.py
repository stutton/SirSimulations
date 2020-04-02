import pyglet
import numpy as np
from engine.circle import Circle
from engine.box import Box
from engine.engine import Engine
from models.person import Person
from models.sir_simulation import SirSimulation

# Settings
windowWidth = 540
windowHeight = 480
fps = 60
resizable = False

window = pyglet.window.Window(width=windowWidth, height=windowHeight, resizable=resizable)
eng = Engine(540, 480, 60, False)

# for i in range(100):
#     p = Person()
#     p.position = np.array((270., 240., 0.))

#     eng.drawables.append(p)
#     eng.updatables.append(p)

# b = Box(50, 50)
# eng.drawables.append(b)

sir = SirSimulation()
eng.drawables.append(sir)
eng.updatables.append(sir)

@window.event
def on_draw():
    window.clear()
    eng.render()

def update(dt):
    eng.update(dt)

if __name__ == "__main__":
    pyglet.clock.schedule_interval(update, 1/fps)
    pyglet.app.run()