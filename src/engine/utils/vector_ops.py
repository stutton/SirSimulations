import numpy as np
import math
import engine.utils
from engine.utils.constants import *

def get_norm(vector):
    return sum([x**2 for x in vector])**0.5

def normalize(vector, fall_back=None):
    norm = get_norm(vector)
    if norm > 0:
        return np.array(vector) / norm
    elif fall_back is not None:
        return fall_back
    else:
        return np.zeros(len(vector))

def unit_vector(vector):
    """Returns the unit vector of the vector."""
    return vector / np.linalg.norm(vector)

def angle_between(v1, v2):
    """Finds angle between two vectors."""
    v1_u = unit_vector(v1)
    v2_u = unit_vector(v2)
    return np.arccos(np.clip(np.dot(v1_u, v2_u), -1., 1.))

def z_rotation(vector, theta):
    """Rotates a 3-D vector around z-axis"""
    R = np.array([[np.cos(theta), -np.sin(theta), 0], [np.sin(theta), np.cos(theta), 0], [0, 0, 1]])
    return np.dot(R, vector)