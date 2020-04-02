from .city import City
from .person import Person

class SirSimulation:
    def __init__(self):
        self.p_infection = 0.2
        self.n_cities = 1

        self.time = 0
        self.cities = []
        self.people = []

        self.add_cities()
        self.add_people()
    
    def add_cities(self):
        for i in range(self.n_cities):
            # TODO: Arrange multiple cities
            c = City(20, 20, 500, 440)
            self.cities.append(c)
    
    def add_people(self):
        for city in self.cities:
            for i in range(city.population):
                person = Person()
                city.add_person(person)
                self.people.append(person)
    
    def update(self, dt):
        for person in self.people:
            person.update(dt)
    
    def render(self):
        for city in self.cities:
            city.render()
        for person in self.people:
            person.render()