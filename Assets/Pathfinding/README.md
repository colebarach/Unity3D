# Pathfinding
A collection of pathfinding algorithms implemented in C#. Each algorithm is implemented through a main library file, which contains frontend functions for generic usage.
## A* Algorithm
The A* pathfinding algorithm, referred to as AStar in source, can be read about here: https://optimization.cbe.cornell.edu/index.php?title=A-star_algorithm
The implementation of this algorithm uses a path, a start position, and an end position. The obstacle field is a 2D texture, which represents the open tiles with white pixels and the obstructed tiles with black pixels. The starting and ending positions are given as 3D vectors, which are projected onto the field using the matrix of the field object. The path is returned as an array of 3D vectors, with the least significant value being the start and the most significant value being the end
