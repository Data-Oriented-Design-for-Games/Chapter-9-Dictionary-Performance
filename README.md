# Chapter 9 — Dictionary Performance

Sample project for **Chapter 9** of [*High Performance Unity Game Development (Using data-oriented design)*](https://www.manning.com/books/high-performance-unity-game-development) by Nitzan Wilnai (Manning).

This is a small benchmark, not a game. It does the same jobs with a `Dictionary` and with a plain array, and times both. The question it answers: when the key is just a number, how much does a dictionary cost compared to an array?

## What it shows

- **Access:** 1024 lookups by key in a `Dictionary<int, int>` against 1024 reads by index from an `int[]`, in the same shuffled order.
- **Copy:** copying values from one dictionary to another against copying from one array to another.
- **Iteration:** a `foreach` over a dictionary against a `for` loop over an array.
- **Search:** one dictionary lookup against a scan through an array for the same key.
- **Value type:** the same single lookup when the dictionary holds an `int`, a struct (`S`) or a class (`C`).

## How the test works

All the code is in `Assets/Scripts/DictionaryTest.cs`.

- `init()` runs in `Awake`. It fills the dictionaries and arrays with 1024 entries, where key `i` holds value `i`. It also fills `randomIndices` with the numbers 0 to 1023 and shuffles them.
- `RunDictAccessTest()` is the test. It repeats every part 10,000 times (`m_numIterations`) and adds up the time for each part with `Time.realtimeSinceStartupAsDouble`.
- `arrayIteration()`, `dictLookup()`, `dictSructLookup()` and `dictClassLookup()` each time one search or one lookup. They are called for keys 0 to 63.
- Every value that is read is added to `checksum`, which is printed with the results.
- `dictionaryLookupTest()` is a shorter version of the access test that logs to the Console. Nothing calls it. The call in `Start()` is commented out.

The access part is the heart of the sample:

```csharp
for (int j = 0; j < 1024; j++)
{
    checksum += dict2[randomIndices[j]];   // Dictionary<int, int>
}

for (int j = 0; j < 1024; j++)
{
    checksum += array2[randomIndices[j]];  // int[]
}
```

## Running it

1. Open the project in Unity **6000.3.22f1** (Unity 6.3 LTS) or newer.
2. Open `Assets/Scenes/SampleScene.unity` and press **Play**.
3. Click **Run Test**. The app freezes while the test runs. Then the results appear as text on screen. Nothing is written to the Console.

The scene is not in the project's build scene list. Add it there first if you want to make a build.

## Reading the results

The first three blocks look like this, once each for access, copy (`Dictionary to Dictionary` / `Array to Array`) and iteration:

```
Dictionary access <seconds>
Array access <seconds>
Array  <N>x faster
```

- `<seconds>` is the total time for all 10,000 repeats.
- `<N>x faster` is the dictionary time divided by the array time. A value below 1 means the array was slower.

The last block compares a search with a lookup. It prints up to three pairs of lines, one pair each for `Dictionary access`, `Dictionary struct access` and `Dictionary class access`:

```
Array iteration to key <K> <seconds>
Dictionary access to key <K> <seconds>
```

- The test checks keys from 63 down to 0. It prints the first key where scanning the array for that key took no longer than the dictionary lookup. If there is no such key, the pair is not printed.
- By this point `array1` holds the keys in shuffled order, because the copy part fills it that way. So key `K` is not at index `K`. How long the scan takes depends on where the shuffle put that key, and `<K>` can change from run to run.
- Each of these times covers a single lookup or a single scan, so the cost of reading the clock is part of the number. Compare them with each other, not with the totals above.

The numbers depend on your device and on whether you run in the Editor or in a build.

## More samples

All sample projects for the book: https://github.com/Data-Oriented-Design-for-Games
