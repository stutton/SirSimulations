import numpy as np
import random
from engine.circle import Circle
from engine.drawable import Drawable
from engine.utils.vector_ops import *
from engine.utils.constants import *

COLOR_MAP = {
    "S": BLUE,
    "I": RED,
    "R": GREY
}

class Person:

    def __init__(self, x=0, y=0, r = 5.0):
        # Visuals
        self.color = WHITE
        self.visual = Circle(0, 0, r)
        self.visual.color = self.color
        self.gravity_well_visual = Circle(0, 0, 3)

        # Time
        self.time = 0
        self.last_step_change = -1

        # Bounds
        self.dl_bound = np.array((0, 0))
        self.ur_bound = np.array((540, 480))

        # Movement
        self.position = np.array((x, y, 0))
        self.velocity = np.zeros(3)
        self.max_speed = 30
        self.gravity_well = None
        self.gravity_strength = 500000
        self.wander_step_size = 100
        self.wander_step_duration = 2
        self.wall_buffer = 20
        self.wall_strength = 10000

        # Infection
        self.status = "S" # SRI status
        self.infection_radius = 15
        self.infection_start_time = np.inf
        self.infection_end_time = np.inf
        self.num_infected = 0
        self.city = None
    
    def update(self, dt):
        total_force = np.zeros(3)
        self.time += dt
        # Wander
        if self.wander_step_size != 0:
            if (self.time - self.last_step_change) > self.wander_step_duration:
                vect = z_rotation(RIGHT, TAU * random.random())
                self.gravity_well = self.position + (self.wander_step_size * vect)
                self.last_step_change = self.time
                # DEBUG: Gravity well visual
                self.gravity_well_visual.position = self.gravity_well

        if self.gravity_well is not None:
            to_well = (self.gravity_well - self.position)
            dist = get_norm(to_well)
            if dist != 0:
                total_force += self.gravity_strength * to_well / (dist**3)
        
        # TODO: Social distance

        # TODO: Avoid walls
        wall_force = np.zeros(3)
        for i in range(2):
            to_lower = self.position[i] - self.dl_bound[i]
            to_upper = self.ur_bound[i] - self.position[i]

            # Bounce
            if to_lower < 0:
                self.velocity[i] = np.abs(self.velocity[i])
                self.position[i] = self.dl_bound[i]
            if to_upper < 0:
                self.velocity[i] = -np.abs(self.velocity[i])
                self.position[i] = self.ur_bound[i]
            
            wall_force[i] += max((-1 / self.wall_buffer + 1 / to_lower), 0)
            wall_force[i] -= max((-1 / self.wall_buffer + 1 / to_upper), 0)
        total_force += wall_force * self.wall_strength

        # Apply force
        self.velocity += total_force * dt

        # Limit speed
        speed = get_norm(self.velocity)
        if speed > self.max_speed:
            self.velocity *= self.max_speed / speed
        
        self.shift(self.velocity * dt)

    def set_bounds(self, dl, ur):
        self.dl_bound = dl
        self.ur_bound = ur

    def shift(self, vector):
        self.position += vector
        self.visual.position = self.position
        return self
    
    def set_color(self, color):
        self.visual.color = color
    
    def render(self):
        self.visual.render()
        # DEBUG: Draw gravity well
        #self.gravity_well_visual.render()

