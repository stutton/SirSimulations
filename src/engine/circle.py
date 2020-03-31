from pyglet.gl import *
from math import pi, sin, cos
from .drawable import Drawable

class Circle(Drawable):
    def __init__(self):
        super().__init__()

        self.mode = GL_TRIANGLE_FAN

        self.x = 0.0
        self.y = 0.0
        self.radius = 10.0
    
    def on_render(self):
        iterations = int(2 * self.radius * pi)
        s = sin(2 * pi / iterations)
        c = cos(2 * pi / iterations)
        dx, dy = self.radius, 0
        glVertex2f(self.x, self.y)
        for i in range(iterations + 1):
            glVertex2f(self.x + dx, self.y + dy)
            dx, dy = (dx * c - dy * s), (dy * c + dx * s)
