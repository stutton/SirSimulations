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

def quaternion_from_angle_axis(angle, axis):
    return [
        math.cos(angle / 2),
        *(math.sin(angle / 2) * normalize(axis))
    ]

def quaternion_mult(*quats):
    if len(quats) == 0:
        return [1, 0, 0, 0]
    result = quats[0]
    for next_quat in quats[1:]:
        w1, x1, y1, z1 = result
        w2, x2, y2, z2 = next_quat
        result = [
            w1 * w2 - x1 * x2 - y1 * y2 - z1 * z2,
            w1 * x2 + x1 * w2 + y1 * z2 - z1 * y2,
            w1 * y2 + y1 * w2 + z1 * x2 - x1 * z2,
            w1 * z2 + z1 * w2 + x1 * y2 - y1 * x2,
        ]
    return result

def quaternion_conjugate(quaternion):
    result = list(quaternion)
    for i in range(1, len(result)):
        result[i] *= -1
    return result

def rotate_vector(vector, angle, axis=OUT):
    if len(vector) == 2:
        z = complex(*vector) * np.exp(complex(0, angle))
        result = [z.real, z.imag]
    elif len(vector) == 3:
        quat = quaternion_from_angle_axis(angle, axis)
        quat_inv = quaternion_conjugate(quat)
        product = quaternion_mult(quat, [0, *vector], quat_inv)
        result = product[1:]
    else:
        raise Exception("vector must be 2 or 3 dimensions")

    if isinstance(vector, np.ndarray):
        return np.array(result)
    return result