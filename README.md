# Whiteboxing-UnityMLAgents
Experimental testbed where I test various Machine Learning & AI concepts using Unity ML Agents. Note that this is a work in progress that I am sharing "as is" as I build and expand new ML scenarios.

**Unity version:** 2018.1.0b13 (beta)
**Unity ML-Agents version:** 0.3.1a

## Project Setup
* The [TensorflowSharp](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Background-TensorFlow.md#tensorflowsharp) plugins folder was omitted from this project due to the massive file sizes. You will need to import this set of Unity plugins yourself. You can download the TensorFlowSharp plugin as a [Unity package here](https://s3.amazonaws.com/unity-ml-agents/0.3/TFSharpPlugin.unitypackage).
* There is currently a bug where if you use all the duplicate platforms for training, all the rats & cheese will spawn on the same platform since the initial spawn values are hard-coded and not relative to each board. regardless, it does not affect the training as they all still run independently from one another.

## Follow Me
* Twitter: [@ActiveNick](http://twitter.com/ActiveNick)
* Blog: [AgeofMobility.com](http://AgeofMobility.com)
* SlideShare: [http://www.slideshare.net/ActiveNick](http://www.slideshare.net/ActiveNick)
