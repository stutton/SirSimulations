import numpy as np
from engine.box import Box

class City:
    def __init__(self, x, y, w, h):
        # Visuals
        self.visual = Box(x, y, w, h)

        # Properties
        self.population = 100
        self.position = np.array((20, 20, 0))
        self.size = np.array((500, 440, 0))

        # People
        self.people = []
    
    def add_person(self, person):
        self.people.append(person)
        person.city = self
        person.position = self.size / 2 + self.position
        person.set_bounds(self.position, self.position + self.size)

    def render(self):
        self.visual.render()