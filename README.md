# ghc_trafficlights
My solution for Google Hash Code 2021 Online Qualifications round

The key highlight of the solution is that it's so simple that the calculation for all the inputs took 5 secs tops. I never implemented a simulator or a time sensitive algorithm because traffic is traffic, with a lot of cars (didn't even count them), a probabilistic solution was just enough for me.

Sorry, I worked on this alone so I didn't commit every change. I left the different algorithms in the Algorithms namespace. Most of them differs in the main idea, but some of them have alternatives (with different prefixes).

## How the solution idea was developing in my head

 1. The first version was to assign 1 to every street at every intersection. This resulted most of the points (~7.7m). ([see Algorithms.Simplest](https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/Simplest.cs))
 2. The same, but I filtered out the streets that no car used. ([see Algorithms.SimplestWithoutUnnecessary](https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/SimplestWithoutUnnecessary.cs))
 3. I created a weight map for all the streets by counting how many cars will *ever* get through that street, and normalized the values by dividing by the greatest common divisor. ([see Algorithms.SimplestWithoutUnnecessary](https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/SimplestWithoutUnnecessary.cs))
 4. From here, I branched:
     - The best (and first one) was taking the square root of the weights
     - I tried log2 of the weights, results were horrible
     - I tried 3rd square root, didn't help much
     - I tried to modify the quare root with random operations to fine tune the results (/2, \*2, x => (x - 1) / 2 + 1, \*1.5, \*2.5, \*3). These resulted small increases in some of the files (each one improved one level tops).
     - In the last minutes I found out that I forgot my original idea; to normalize the weight between 0 and 1 and use some constants, I tried it with a couple of constants but couldn't improve.
