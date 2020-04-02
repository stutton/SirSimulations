from pyglet.gl import *
from math import pi, sin, cos
from .drawable import Drawable
import numpy as np

class Circle(Drawable):
    def __init__(self, x = 0.0, y = 0.0, r = 10.0):
        super().__init__()

        self.mode = GL_TRIANGLE_FAN

        self.position = np.array([x, y, 0.])
        self.radius = r
    
    def on_render(self):
        iterations = int(2 * self.radius * pi)
        s = sin(2 * pi / iterations)
        c = cos(2 * pi / iterations)
        dx, dy = self.radius, 0
        glVertex2f(self.position[0], self.position[1])
        for i in range(iterations + 1):
            glVertex2f(self.position[0] + dx, self.position[1] + dy)
            dx, dy = (dx * c - dy * s), (dy * c + dx * s)
