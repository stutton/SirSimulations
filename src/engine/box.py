
from pyglet.gl import *
from math import pi, sin, cos
from .drawable import Drawable
import numpy as np

class Box(Drawable):
    def __init__(self, x=0., y=0., w=50., h=50.):
        super().__init__()

        self.mode = GL_LINE_LOOP
        self.position = np.array((x, y, 0))
        self.size = np.array((w, h))

    def on_render(self):
        glVertex2f(self.position[0], self.position[1])
        glVertex2f(self.position[0] + self.size[0], self.position[1])
        glVertex2f(self.position[0] + self.size[0], self.position[1] + self.size[1])
        glVertex2f(self.position[0], self.position[1] + self.size[1])