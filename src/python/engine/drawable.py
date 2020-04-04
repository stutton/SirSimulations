from pyglet.gl import *

class Drawable:
    def __init__(self):
        self.mode = GL_POINTS
        self.color = [1.0, 1.0, 1.0]

    def on_render(self):
        pass

    def render(self):
        glBegin(self.mode)
        glColor3f(self.color[0], self.color[1], self.color[2])

        self.on_render()

        glEnd()
