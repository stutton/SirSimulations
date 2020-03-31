import sys
from math import pi, sin, cos
import pyglet
from engine.circle import Circle

window = pyglet.window.Window(width=540, height=480, resizable=False)

c = Circle()
c.x = 0
c.y = 240
v = 2

drawables = []
drawables.append(c)

@window.event
def on_draw():
    window.clear()
    for d in drawables:
        d.render()

def update(dt):
    global c
    global v
    if c.x >= window.width:
        c.x = window.width
        v = -v
    if c.x <= 0:
        c.x = 0
        v = -v
    
    c.x = c.x + v

if __name__ == "__main__":
    pyglet.clock.schedule_interval(update, 1/60)
    pyglet.app.run()
