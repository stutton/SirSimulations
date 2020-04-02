import numpy as np
import random
from engine.circle import Circle
from engine.drawable import Drawable
from engine.utils.vector_ops import *
from engine.utils.constants import *

class Person(Circle):

    def __init__(self, r = 10.0):
        super().__init__(r)

        self.time = 0
        self.last_step_change = -1

        self.dl_bound = np.array((0, 0))
        self.ur_bound = np.array((540, 480))

        self.max_speed = 65
        self.velocity = np.zeros(3)

        self.gravity_well = None
        self.gravity_strength = 2000000
        self.wander_step_size = 150
        self.wander_step_duration = 2

        self.wall_buffer = 50
        self.wall_strength = 10000

    
    def update(self, dt):
        total_force = np.zeros(3)
        self.time += dt
        # Wander
        if self.wander_step_size != 0:
            if (self.time - self.last_step_change) > self.wander_step_duration:
                vect = rotate_vector(RIGHT, TAU * random.random())
                self.gravity_well = (self.position + self.wander_step_size) * vect
                self.last_step_change = self.time
        
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


    def shift(self, vector):
        self.position += vector
        return self
