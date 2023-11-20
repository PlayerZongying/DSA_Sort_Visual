using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : MonoBehaviour
{
    // Start is called before the first frame update
    private DisplaySorting _displaySorting;

    public bool usingPauseInterval = false;
    public float pauseInterval = 0.01f;

    void Start()
    {
        _displaySorting = DisplaySorting.instance;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void Sort()
    {
        // QuickSort(_displaySorting.arr);
        QuickSortCoroutine(_displaySorting.arr);
    }

    #region QuickSort
    void QuickSort(float[] array)
    {
        QuickSortHelper(array, 0, array.Length - 1);
    }
    void QuickSortHelper(float[] array, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(array, low, high);

            QuickSortHelper(array, low, partitionIndex - 1);
            QuickSortHelper(array, partitionIndex + 1, high);
        }
    }
    int Partition(float[] array, int low, int high)
    {
        int i = low;
        int j = high;
        float pivot = array[low];
        while (i < j)
        {
            while (i < j && array[j] >= pivot) j--;
            while (i < j && array[i] <= pivot) i++;
            (array[i], array[j]) = (array[j], array[i]);
        }
        (array[low], array[i]) = (array[i], array[low]);
        return i;
    }

    void QuickSortCoroutine(float[] array)
    {
        StartCoroutine(QuickSortHelperCoroutine(array, 0, array.Length - 1));
    }
    
    IEnumerator QuickSortHelperCoroutine(float[] array, int low, int high)
    {
        if (low < high)
        {
            // int partitionIndex;
            StartCoroutine(Partition(array, low, high, i =>
            {
                // partitionIndex = i;
                StartCoroutine(QuickSortHelperCoroutine(array, low, i - 1));
                StartCoroutine(QuickSortHelperCoroutine(array, i + 1, high));
            }));
        }
        yield return usingPauseInterval? new WaitForSeconds(pauseInterval): new WaitForEndOfFrame();
    }
    
    IEnumerator Partition(float[] array, int low, int high, System.Action<int> callback)
    {
        int i = low;
        int j = high;
        float pivot = array[low];
        while (i < j)
        {
            while (i < j && array[j] >= pivot)
            {
                j--;
                // yield return usingPauseInterval? new WaitForSeconds(pauseInterval): new WaitForEndOfFrame();
            }

            while (i < j && array[i] <= pivot)
            {
                i++;
                // yield return usingPauseInterval? new WaitForSeconds(pauseInterval): new WaitForEndOfFrame();
            }
            (array[i], array[j]) = (array[j], array[i]);
            yield return usingPauseInterval? new WaitForSeconds(pauseInterval): new WaitForEndOfFrame();
        }
        (array[low], array[i]) = (array[i], array[low]);
        // yield return usingPauseInterval? new WaitForSeconds(pauseInterval): new WaitForEndOfFrame();
        callback(i);
    }
    #endregion
}