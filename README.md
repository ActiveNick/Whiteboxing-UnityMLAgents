# Whiteboxing-UnityMLAgents
Experimental testbed where I test various Machine Learning & AI concepts using Unity ML Agents. Note that this is a work in progress that I am sharing "as is" as I build and expand new ML scenarios. I've also included the trainer_config.yaml file since the Python/Tenserflow parameters are essential to using these scenarios. Note that these scenarios, reward functions and config parameters are all experimental and very much works in progress. Feel free to file an issue here on GitHub if you have found better settings for these scenarios. This is an experimentation platform and you feedback is welcome.

**Unity version:** 2018.3.8f1
**Unity ML-Agents version:** 0.6.0a

## Environments
**Rat & Cheese (single):** The rat (gray ball) should run to the piece of cheese (yellow cube). The rat is given the relative position of the cheese in relationship to itself. Agent Reward Function: 
* +0.1 if getting closer.
* -0.05 time penalty.
* +1.0 when reaching target.

![Rat & Cheese (single) at runtime after training](Screenshots/RatCheese-Runtime02.gif)

**Rat & Cheese (multiple cheese):** The rat (gray cube) collects all the pieces of cheese (yellow cube) on the board. The rat does not know where the cheese pieces are, it uses raycasts to detect the cheese as it moves around. Agent Reward Function: 
* -0.2 penalty if the rat hits a wall.
* -0.005 time penalty.
* +1.0 when reaching a piece of cheese.

![Rat & Cheese (multiple cheese) at runtime after training](Screenshots/RatsAndCheese-Runtime03.gif)

**Civilization:** The goal is to re-create an ML-based AI for an RTS-style game like Age of Empires, such as gathering resources, building structures, and eventually fight other units. The current iteration shows a single "villager" unit, gathering wood from nearby trees, and eventually building a farm, which costs 100 units of wood. The models included here are far from perfect and this is still very much a work in progress. Stay tuned for more updates.

![Civilization/Villager at runtime after training](Screenshots/Civ-WoodGatherer-Runtime03a.gif)

## Project Setup
* The [TensorflowSharp](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Background-TensorFlow.md#tensorflowsharp) plugins folder was omitted from this project due to the massive file sizes. You will need to import this set of Unity plugins yourself. You can download the TensorFlowSharp plugin as a [Unity package here](https://s3.amazonaws.com/unity-ml-agents/0.3/TFSharpPlugin.unitypackage).
* There is currently a bug where if you use all the duplicate platforms for training, all the rats & cheese will spawn on the same platform since the initial spawn values are hard-coded and not relative to each board. regardless, it does not affect the training as they all still run independently from one another.

## Follow Me
* Twitter: [@ActiveNick](http://twitter.com/ActiveNick)
* SlideShare: [http://www.slideshare.net/ActiveNick](http://www.slideshare.net/ActiveNick)
