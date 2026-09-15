# VoxelTerra

A voxel world generation system.

# Project Specifics

This project uses Dotnet version of Godot 4.7.2 with C# as it's primary programming language.

# What I've Learned

Much of what I have learned in this project is about game performance. For example, in this project I decided to use C# over GDScript; for most projects GDScript is generally an excellent choice for programming game logic, however when generating procedural meshes and collisions I've found that C# is much more performant and thus reduces the annoying frame stutters that come with updating large sections of the game worlds procedural geometry. Also I've discovered the benefits of multithreading when processing large amounts of data. With the game world potentially containing millions of blocks that are constantly updating, offloading world geometry updates to a separate thread has greatly reduced the potential for frame stutters.
