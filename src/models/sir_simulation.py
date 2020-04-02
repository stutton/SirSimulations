import random
from engine.utils.vector_ops import *
from .city import City
from .person import Person

class SirSimulation:
    def __init__(self):
        self.p_infection = 0.5
        self.infection_time = 5
        self.n_cities = 1

        self.time = 0
        self.cities = []
        self.people = []

        self.add_cities()
        self.add_people()
    
    def add_cities(self):
        for i in range(self.n_cities):
            # TODO: Arrange multiple cities
            c = City(20, 20, 450, 450)
            c.population = 200
            self.cities.append(c)
    
    def add_people(self):
        for city in self.cities:
            for i in range(city.population):
                person = Person()
                city.add_person(person)
                self.people.append(person)
        
        # Choose patient zero
        random.choice(self.people).set_status("I")
    
    def update(self, dt):
        self.time += dt
        self.update_statuses(dt)
        for person in self.people:
            person.update(dt)

    def update_statuses(self, dt):
        for city in self.cities:
            s_people, i_people = [
                list(filter(
                    lambda m: m.status == status,
                    city.people
                ))
                for status in ["S", "I"]
            ]

            for i_person in i_people:
                for s_person in s_people:
                    dist = get_norm(i_person.position - s_person.position)
                    if dist < s_person.infection_radius and random.random() < self.p_infection * dt:
                        s_person.set_status("I")
                        i_person.num_infected += 1
                if (i_person.time - i_person.infection_start_time) > self.infection_time:
                    i_person.set_status("R")
            
        #TODO: Travel
        #TODO: Social distancing
    
    def render(self):
        for city in self.cities:
            city.render()
        for person in self.people:
            person.render()