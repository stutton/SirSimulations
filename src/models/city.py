import numpy as np
import random
from engine.box import Box

class City:
    def __init__(self, x, y, w, h):
        # Visuals
        self.visual = Box(x, y, w, h)

        # Properties
        self.population = 100
        self.position = np.array((x, y, 0))
        self.size = np.array((w, h, 0))

        # People
        self.people = []
    
    def add_person(self, person):
        self.people.append(person)
        person.city = self
        person.position = np.array((
            self.size[0] * random.random() + self.position[0],
            self.size[1] * random.random() + self.position[1],
            0
        ))
        person.set_bounds(self.position, self.position + self.size)

    def render(self):
        self.visual.render()