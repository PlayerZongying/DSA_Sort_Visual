using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeRecorder", menuName = "TimeRecorder", order = 1)]
public class TimeRecorder : ScriptableObject
{
    public int testTime = 10;
    public Stats insertionStats;
    public Stats bubbleStats;
    public Stats selectionStats;
    public Stats mergeStats;
    public Stats quickStats;

    public TimeRecorder()
    {
        insertionStats = new Stats(testTime);
        bubbleStats = new Stats(testTime);
        selectionStats = new Stats(testTime);
        mergeStats = new Stats(testTime);
        quickStats = new Stats(testTime);
    }

    public Stats GetStats(Sorter.Algorithm algorithm)
    {
        Stats statsToReturn;
        switch (algorithm)
        {
            case Sorter.Algorithm.InsertionSort:
                statsToReturn = insertionStats;
                break;
            case Sorter.Algorithm.BubbleSort:
                statsToReturn = bubbleStats;
                break;
            case Sorter.Algorithm.SelectionSort:
                statsToReturn = selectionStats;
                break;
            case Sorter.Algorithm.MergeSort:
                statsToReturn = mergeStats;
                break;
            case Sorter.Algorithm.QuickSort:
                statsToReturn = quickStats;
                break;
            default:
                statsToReturn = insertionStats;
                break;
        }
        return statsToReturn;
    }
}

public struct Stats
{
    public float[,] timeRecords;
    public float[,] min;
    public float[,] max;
    public float[,] med;
    public float[,] avg;

    private int _testTime;

    public Stats(int testTime)
    {
        timeRecords = new float[6, testTime];
        min = new float[6, 1];
        max = new float[6, 1];
        med = new float[6, 1];
        avg = new float[6, 1];
        _testTime = testTime;
    }

    public void Print()
    {
        for (int i = 0; i < 6; i++)
        {
            int size = (i % 2 == 0 ? 1 : 5) * (int)Mathf.Pow(10, (int)(i / 2 + 1));
            Debug.Log($"------------Time for size {size}: -------------");
            for (int j = 0; j < _testTime; j++)
            {
                Debug.Log(timeRecords[i, j]);
            }
            Debug.Log($"Min for size {size}: {min[i,0]}");
            Debug.Log($"Max for size {size}: {max[i,0]}");
            Debug.Log($"Med for size {size}: {med[i,0]}");
            Debug.Log($"Avg for size {size}: {avg[i,0]}");
        }
    }

    public void CalculateStats()
    {
        for (int i = 0; i < 6; i++)
        {
            float[] arr = new float[_testTime];
            float sum = 0;
            for (int j = 0; j < _testTime; j++)
            {
                arr[j] = timeRecords[i, j];
                sum += arr[j];
            }

            Array.Sort(arr);
            min[i, 0] = arr[0];
            max[i, 0] = arr[_testTime - 1];
            med[i, 0] = _testTime % 2 == 0 ? (arr[_testTime / 2] + arr[_testTime / 2 - 1]) / 2 : arr[_testTime / 2];
            avg[i, 0] = sum / _testTime;
        }
    }
}