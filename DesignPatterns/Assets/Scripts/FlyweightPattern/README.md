# Flyweight Pattern
This is a fairly loose example of a Flyweight implementation.  

Basically there are a series of kits with an inventory of rations, water and batteries.  

Each Kit has a specific number of each.  

Let's assume, even though it's not clear, that all kits can be the same as if they're just a schematic or template.  

The wrong way to instantiate a bunch of objects of the same kit type is to call new KitType() because that will instantiate a new object for each schematic.  

Creating an individual schematic then assigning that to every individual schematic object will save a good amount of memory, especially when scaled significantly.