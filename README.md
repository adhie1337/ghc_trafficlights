# ghc_trafficlights
My solution for Google Hash Code 2021 Online Qualifications Round and Extended Round

I worked on this alone, so I didn't commit every change. I left the different algorithms in the Algorithms namespace. Most of these build on the same idea, but with some details varied. See class name prefixes for a hint. There's also the genetic algorithm that was working, but too slow too late :) I intend to improve it, even after the extended round closes.

## Highlights

### Online Qualifications Round

 - The key highlight of the solution is that it's so simple that the calculation for all the inputs took 5 secs tops. I never implemented a simulator or a time-sensitive algorithm because traffic is traffic, with a lot of cars (I didn't even count them); a probabilistic solution was just enough for me.
 - The other interesting thing is that after taking a quick look at the best solution files for ~99% of the cases, the green time range was 1 for the lights. I guess this Round-Robin based approach worked pretty well for me (however unintentional that was). I didn't try adding one for every time though, maybe it could have been a bit better. (Or couldn't, I guess we'll never know.)
 - I was planning to do some insights on the data, and at first, I created the simplest algorithms ever, and it resulted in big points. After that, I tried to look, but the numbers were too scary. I ended up adjusting the simple solution with small improvements.
 - In the last hour (of the 4 maybe), I only improved 1%. I guess I stuck in a local maximum. Sadly, I didn't have the time to implement anything smarter. The final score was 9,532,454 points.

### Extended Round

 - After a night long rest, I came up with ideas to improve the original simplified solution. Sadly, I only got ~20000 points plus. I still couldn't escape that local maximum. The highest score I got was 9,551,911 points.
 - Then I initiated a week-long break because when would be a better time to have a long break than in the middle of a competition?
 - After a couple of days, my good friend successfully implemented the referee algorithm[1]. That one was a huge help, so thanks from here as well ;) It turned out that it's always a huge help to have the referee implemented locally.
 - In the meantime, my friend found the blog post[2] from the team that won the 16th place. I was surprised to read that they started on the same path as I did. But they noticed something I totally forgot, that the order of the lights could bring a lot more points. When reading the blog post, I was amazed how this team was precisely aware of what was exactly happening during the competition. They did a lot more research and statistics than I did. I could learn an awful lot from that post!
 - I tried to implement the scorer based on the referee[1]. I only could come up with a solution that was close but not perfect. Some of the results weren't the same but in the ballpark as the original one. I started to do some premature optimizations at that point: storing only the first in a queue instead of the whole queue. That point was where the implementations diverged. Maybe it was too soon. From that point, I went into competition mode; where not perfect is good enough. The most important goal was delivery.
 - At the point I had a good-ish scoring algorithm, I started implementing the genetic algorithm. I had high hopes for a few rounds being enough to get into the 10 million club. Well, that was a failure. But no regrets here! This was the first time I could come up with a successful-ish use of the genetic algorithm. I'll definitely write a blog post about it in my own blog[3].


## How the solution idea was developing in my head

### Online Qualifications Round

 1. The first version was to assign 1 to every street at every intersection. That resulted in getting most of the points (~7.7m / 10.5m). (see Algorithms.Simplest[4])
 2. The same, but I filtered out the streets that no car used. (see Algorithms.SimplestWithoutUnnecessary[5])
 3. I created a weight map for all the streets by counting how many cars will *ever* get through that street and normalized the values by dividing by the greatest common divisor. (see Algorithms.WeightedScheduler[6])
 4. From here, I branched (I hope that from now on, you'll get the idea where the classes are):
     - The best (and first one) was taking the square root of the weights[7]
     - I tried log2 of the weights, results were horrible
     - I tried 3rd square root, didn't help much
     - I tried to modify the quare root with random operations to fine tune the results (/2, \*2, x => (x - 1) / 2 + 1, \*1.5, \*2.5, \*3). These resulted small increases in some of the files (each one improved one level tops).
     - At the last minute, I found out that I forgot my original idea; to normalize the weight between 0 and 1 and use some constants. I tried it with a couple of constants but couldn't improve.

### Extended Round

5. The first thing I tried was fine-tuning the weighted scheduler algorithm, but I couldn't get a significant increase.
6. After a day, I started working on a proof of concept implementation of the genetic algorithm that was guessing the given string ("Hello World!" at first, paragraphs of the Lorem Ipsum text later.) character by character. I managed to come up with a pretty fast algorithm, but on the easy problem where the scoring function was an array difference counting.
   After the break, I managed to get back into the game when my good friend implemented the referee[1]. The goal was to re-implement it in C# to spare the cost of interop because the scorer needed to be fast for the genetic algorithm. Sadly I couldn't come up with the perfect solution. What I had, was calculating a score near what the judge system did (+-5%, I think). But it was awfully slow. I started speeding it up instead of aiming for the perfect one. During the optimization, I still managed to change the scores, but the differences were minor.
   Once I managed to reduce the needed time of the scoring to 0.3s on D, I assembled the genetic algorithm with a population size of 10 running for 10 rounds. It wasn't enough, but I totally lost the speed in the last couple of days.
   At the last minute, I modified the algorithm to run with a time limit. Results were still poor: in 10 minutes, it only could do 16 rounds. Based on that, I can say that it wasn't enough. But as a first one, I'm happy with it! For the code, see [8].


## References

[1] - https://github.com/KristofHolecz/hash-code-2021-traffic-signaling-insights-reporter
[2] - https://curiouscoding.nl/hashcode-2021/
[3] - https://adhie.website
[4] - https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/Simplest.cs
[5] - https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/SimplestWithoutUnnecessary.cs
[6] - https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/WeightedScheduler.cs
[7] - https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/SqrtWeightedScheduler.cs
[8] - https://github.com/adhie1337/ghc_trafficlights/blob/main/src/TrafficLights.Console/Algorithms/Genetic.cs
