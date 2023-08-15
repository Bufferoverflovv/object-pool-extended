# Object Pooling System for Unity
## Introduction

This object pooling system is designed to use the native unity object pool system and use a manager class as a nice way to manage all your pools and pooled objects. 

### Setup
1. Import the Namespace:

At the top of your script, add the line:
```cs
using Buffer.ObjectPooling;
```
2. Add the PoolManager to a GameObject:
Drag the PoolManager script onto a GameObject in your scene.

3. Configure Pools in the Inspector:
    * Click on the GameObject that has the PoolManager script attached.
    * In the Inspector, you'll see a field to add object pools.
    * For each object type you wish to pool:
        *  Set the name.
        * Drag the prefab into the Component field.
        * Define the Initial Size and Max Size for the pool.
        * Optionally, set a Container for pooled objects.
     
### Usage
### Retrieve an Object from the Pool
`var bullet = PoolManager.Get(Bullet);`

### Retrieve an Object with Initialization
If your pooled object implements the *IPoolObject* interface, you can pass initialization arguments:
`var bullet = PoolManager.Get(Bullet, arg1, arg2);`

### Return an Object to the Pool
`PoolManager.Release(Bullet);`
