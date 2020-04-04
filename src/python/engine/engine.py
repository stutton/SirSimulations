
import pyglet
from pyglet.gl import *

class Engine:

    def __init__(self, windowWidth, windowHeight, fps, resizable):
        self.drawables = []
        self.updatables = []

    def update(self, dt):
        for item in self.updatables:
            item.update(dt)

    def render(self):
        for item in self.drawables:
            item.render()