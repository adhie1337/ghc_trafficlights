# ghc_trafficlights
My solution for Google Hash Code 2021 Online Qualifications round

Sorry, I worked on this alone so I didn't commit every change. I left the different algorithms in the Algorithms namespace. Most of them differs in the main idea, but some of them have alternatives (with different prefixes).

## Highlights

 - The key highlight of the solution is that it's so simple that the calculation for all the inputs took 5 secs tops. I never implemented a simulator or a time sensitive algorithm because traffic is traffic, with a lot of cars (didn't even count them), a probabilistic solution was just enough for me.
 - The other thing that was interesting is that (after taking a quick look at the best solution files) for ~99% of the cases, the green time range was 1 for the lights. I guess this round-robin based approach worked really good for me (however unintentional that was). I didn't try adding one for every time though, maybe it could have been a bit better, or couldn't, I guess I'll never know.
 - I was planning to do some insights on the data, but first I created the simplest algorithms ever, and it resulted a lot of points, after that I tried to look but the amounts were so scary, I ended up adjusting the simple solution with small improvements.
 - In the last hour (from the 4 of them maybe) I only managed to improve 1%, I guess I was stuck in a local maximum but I didn't have the time to implement anything smarter. The final score was 9,532,454 points.

## How the solution idea was developing in my head

 1. The first version was to assign 1 to every street at every intersection. This resulted most of the points (~7.7m). ([see Algorithms.Simplest](https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/Simplest.cs))
 2. The same, but I filtered out the streets that no car used. ([see Algorithms.SimplestWithoutUnnecessary](https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/SimplestWithoutUnnecessary.cs))
 3. I created a weight map for all the streets by counting how many cars will *ever* get through that street, and normalized the values by dividing by the greatest common divisor. ([see Algorithms.SimplestWithoutUnnecessary](https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/SimplestWithoutUnnecessary.cs))
 4. From here, I branched (I hope that from now on, you'll get the idea where the classes are):
     - The best (and first one) was taking the square root of the weights
     - I tried log2 of the weights, results were horrible
     - I tried 3rd square root, didn't help much
     - I tried to modify the quare root with random operations to fine tune the results (/2, \*2, x => (x - 1) / 2 + 1, \*1.5, \*2.5, \*3). These resulted small increases in some of the files (each one improved one level tops).
     - In the last minutes I found out that I forgot my original idea; to normalize the weight between 0 and 1 and use some constants, I tried it with a couple of constants but couldn't improve.
