GeneralPrefab_Instantiation
===========================

The GeneralPrefab_Instantiation prefabs are the soul of the fields as they are the responsible for
triggering the procedural spawn of plants. Most of the fields are spawned from variations of the
original version, located at:


.. code-block:: bash

    Assets/Prefabs/GeneralPrefab_Instantiation.prefab

If you have this script attached to an object (like in our sample levels) and hover your mouse over a variable you can see tooltips on
how to use all the variables.

Sections of the code
--------------------

The first section of the code (like most scripts in the project) contains the private variables and
public variables (visible in Unity editor).

The spawning of objects in the code is mainly being ruled by the *procedural_Instantiate()*
function. Thus, we recommend that if you want to spawn your fields and take data from them, create a
inspection manager script to send requests of field spawning with the *procedural_Instantiate()*
function.

Creating your Prefab_Instantiations
-----------------------------------

If you want to save your procedural fields with preset conditions you can:

# Create a copy of the *GeneralPrefab_Instantiation*.
# Add your custom field variables.
# Drag the new prefab to your level. 


