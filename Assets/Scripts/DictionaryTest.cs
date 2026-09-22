using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class C
{
    public int I;

    public C(int i)
    {
        I = i;
    }
}

public struct S
{
    public int I;

    public S(int i)
    {
        I = i;
    }
}

public class DictionaryTest : MonoBehaviour
{
    public TextMeshProUGUI ResultText;

    Dictionary<int, int> dict1 = new Dictionary<int, int>(1024);
    Dictionary<int, int> dict2 = new Dictionary<int, int>(1024);
    int[] array1 = new int[1024];
    int[] array2 = new int[1024];
    int[] randomIndices = new int[1024];

    Dictionary<int, C> classDict = new Dictionary<int, C>(1024);
    Dictionary<int, S> structDict = new Dictionary<int, S>(1024);

    int m_numIterations = 10000;

    private void Awake()
    {
        init();
    }

    // Start is called before the first frame update
    void Start()
    {
        // dictionaryLookupTest();
    }

    private void init()
    {
        for (int i = 0; i < 1024; i++)
        {
            array1[i] = i;
            array2[i] = i;
            dict1[i] = i;
            dict2[i] = i;
            randomIndices[i] = i;

            classDict[i] = new C(i);
            structDict[i] = new S(i);
        }

        // shuffle
        for (int i = 0; i < 1024; i++)
        {
            int index = (Mathf.FloorToInt(Random.value * 1024.0f));
            if (index == 1024)
                index = 1023;

            int a = randomIndices[i];
            randomIndices[i] = randomIndices[index];
            randomIndices[index] = a;
        }

    }

    static long dictionaryLookupTest()
    {
        Dictionary<int, int> dict = new Dictionary<int, int>(1024);
        int[] array = new int[1024];
        int[] randomIndices = new int[1024];

        // init
        for (int i = 0; i < 1024; i++)
        {
            array[i] = i;
            dict[i] = i;
            randomIndices[i] = i;
        }

        // shuffle
        for (int i = 0; i < 1024; i++)
        {
            int index = Random.Range(0, 1024);

            int a = randomIndices[i];
            randomIndices[i] = randomIndices[index];
            randomIndices[index] = a;
        }

        // run test
        double time;
        double timer1 = 0.0d;
        double timer2 = 0.0d;

        long checksum = 0;
        int numIterations = 100000;
        for (int i = 0; i < numIterations; i++)
        {
            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                checksum += dict[randomIndices[j]];
            }
            timer1 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                checksum += array[randomIndices[j]];
            }
            timer2 += (Time.realtimeSinceStartupAsDouble - time);

        }
        Debug.Log("Dictionary " + timer1.ToString("N4"));
        Debug.Log("Array " + timer2.ToString("N4"));
        Debug.Log("Checksum " + checksum);
        return checksum;
    }

    public void RunDictAccessTest()
    {
        ResultText.text = "Dictionary Access Test\n\n";

        double timer1 = 0.0d;
        double timer2 = 0.0d;
        double timer3 = 0.0d;
        double timer4 = 0.0d;
        double timer5 = 0.0d;
        double timer6 = 0.0d;
        double time = 0.0d;

        int SIZE = 64;

        double[] dictTimer = new double[SIZE];
        double[] arrayTimer = new double[SIZE];

        double[] classTimer = new double[SIZE];
        double[] structTimer = new double[SIZE];

        long checksum = 0;

        for (int i = 0; i < m_numIterations; i++)
        {
            // access
            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                checksum += dict2[randomIndices[j]];
            }
            timer1 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                checksum += array2[randomIndices[j]];
            }
            timer2 += (Time.realtimeSinceStartupAsDouble - time);

            // copy
            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                dict1[j] = dict2[randomIndices[j]];
            }
            timer3 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
            {
                array1[j] = array2[randomIndices[j]];
            }
            timer4 += (Time.realtimeSinceStartupAsDouble - time);

            // iteration
            time = Time.realtimeSinceStartupAsDouble;
            foreach (var item in dict1)
            {
                checksum += item.Value;
            }
            timer5 += (Time.realtimeSinceStartupAsDouble - time);

            time = Time.realtimeSinceStartupAsDouble;
            for (int j = 0; j < 1024; j++)
                checksum += array1[j];
            timer6 += (Time.realtimeSinceStartupAsDouble - time);

            for (int k = 0; k < SIZE; k++)
            {
                arrayIteration(ref arrayTimer[k], k, ref checksum);
                dictLookup(ref dictTimer[k], k, ref checksum);
                dictSructLookup(ref structTimer[k], k, ref checksum);
                dictClassLookup(ref classTimer[k], k, ref checksum);
            }
        }

        ResultText.text += "Dictionary access " + timer1.ToString("N4") + "\n";
        ResultText.text += "Array access " + timer2.ToString("N4") + "\n";
        ResultText.text += "Array  " + (timer1/timer2).ToString("N4") + "x faster\n";
        ResultText.text += "\n";
        ResultText.text += "Dictionary to Dictionary " + timer3.ToString("N4") + "\n";
        ResultText.text += "Array to Array " + timer4.ToString("N4") + "\n";
        ResultText.text += "Array  " + (timer3/timer4).ToString("N4") + "x faster\n";
        ResultText.text += "\n";
        ResultText.text += "Dictionary iteration " + timer5.ToString("N4") + "\n";
        ResultText.text += "Array iteration " + timer6.ToString("N4") + "\n";
        ResultText.text += "Array  " + (timer5/timer6).ToString("N4") + "x faster\n";
        ResultText.text += "\n";
        ResultText.text += "Checksum " + checksum + "\n";
        ResultText.text += "\n";

        for (int k = SIZE - 1; k >= 0; k--)
        {
            if (arrayTimer[k] <= dictTimer[k])
            {
                ResultText.text += "Array iteration to key " + k + " " + arrayTimer[k].ToString("N4") + "\n";
                ResultText.text += "Dictionary access to key " + k + " " + dictTimer[k].ToString("N4") + "\n";
                break;
            }
        }

        for (int k = SIZE - 1; k >= 0; k--)
        {
            if (arrayTimer[k] <= structTimer[k])
            {
                ResultText.text += "Array iteration to key " + k + " " + arrayTimer[k].ToString("N4") + "\n";
                ResultText.text += "Dictionary struct access to key " + k + " " + structTimer[k].ToString("N4") + "\n";
                break;
            }
        }

        for (int k = SIZE - 1; k >= 0; k--)
        {
            if (arrayTimer[k] <= classTimer[k])
            {
                ResultText.text += "Array iteration to key " + k + " " + arrayTimer[k].ToString("N4") + "\n";
                ResultText.text += "Dictionary class access to key " + k + " " + classTimer[k].ToString("N4") + "\n";
                break;
            }
        }
    }

    private void arrayIteration(ref double timer, int element, ref long checksum)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        for (int j = 0; j < 1024; j++)
            if (array1[j] == element)
            {
                checksum += array2[j];
                break;
            }
        timer += (Time.realtimeSinceStartupAsDouble - time);
    }

    private void dictLookup(ref double timer, int element, ref long checksum)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        checksum += dict2[element];
        timer += (Time.realtimeSinceStartupAsDouble - time);

    }

    private void dictSructLookup(ref double timer, int element, ref long checksum)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        checksum += structDict[element].I;
        timer += (Time.realtimeSinceStartupAsDouble - time);

    }

    private void dictClassLookup(ref double timer, int element, ref long checksum)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        checksum += classDict[element].I;
        timer += (Time.realtimeSinceStartupAsDouble - time);
    }
}
