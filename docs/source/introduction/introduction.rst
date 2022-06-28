.. _first-steps:

Introduction
============

AgroUnity is developed for the generation of plant imagery for training of convolutional neural
networks (CNN). Additionally the levels can be built to generate excecutable files to work with our
swarm simulator `AgroSwarm <https://github.com/CSCarbone07/roma_quad_ai>`_.

Starting
--------

After following the `installation guide <https://github.com/CSCarbone07/roma_agroUnity/wiki/Installation>`_, open Unity Hub and open the project. The available levels are in the scenes folder, to start the simulation in each of them press the play button on the
center top.


If you do not have any experience with Unity or videogame engines, it is advised to take a look to
some basic tutorials for easier appreciation of this documentation. It is always good to start with
the tutorial courses available in the official pages of Unity:

* `Unity learning pathways <https://learn.unity.com/pathways>`_
* `Unity manual <https://docs.unity3d.com/Manual/index.html>`_

There are also plenty of tutorials in youtube and Udemy, we recommend using youtube for achieving
more specific goals, for example: "`how to make a semi transparent shader <https://www.youtube.com/results?search_query=unity+create+semi+transparent+shader>`_". General Unity tutorials can
still be found in youtube for free but it is difficult sometimes to find good ones. If you would
like more detailed courses, Udemy is a really good place for that but please consider that Udemy courses are paid courses. 


Where to find things
--------------------

From this point on, the documentation assumes that you have some basic knowledge of Unity.

AgroUnity is filled with a lot of scripts, levels and prefabs used for our experiments. In this
documentation we focus on walking you through the specific assets we included for general new users.
You are welcome to go over every asset included in the project but it is commonly filled with
specific approaches for our experiments which we could define less formally as "messy code".

The main levels used for demonstration are placed at the Scenes/release folder in the Project files.
Check the first level `**here** <level_demonstration_1>`.

* :ref:`Installing and getting started with Flightmare <quick-start>`

Many folders in this project act as imported utilities from other users to add required capabilities
to the project. Thus, here we list our made folders which are relevant for new users:

* **Agriculture:** Includes the prefabs that made specifically for vegetation simulation.
* **Materials:** Includes some materials for general use.
* **Prefabs:** Mostly includes our field experiment structures but it also includes the
  *GeneralPrefab_Instantiation* prefab which is the based used to procedurally spawn most of the
  fields. If additional procedural patterns are developed, they would be placed here.
* **Resources:** Includes assets which need to be called dynamically during simulation.
* **Scenes:** Includes the Unity scenes used for our experiments and demonstration scenes (in the
  release folder) to explain what can be done with this simulator.
* **Scripts:** Includes all the scripts used accross the project.
* **Textures:** Includes general purpose textures like noise masks.

You can continue by taking a look to our `**first demonstration level** <level_demonstration_1>`
