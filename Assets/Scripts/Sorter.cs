using System.Collections;
using UnityEngine;

public class Sorter : MonoBehaviour
{
    public static Sorter instance;

    // Start is called before the first frame update
    private DisplaySorting _displaySorting;

    public bool displayMode = false;
    public bool usingPauseInterval = false;
    public bool testOnRandomizedArray = true;
    public float pauseInterval = 0.01f;

    public enum Algorithm
    {
        SelectionSort,
        BubbleSort,
        InsertionSort,
        QuickSort,
        MergeSort,
    }

    public Algorithm algorithm = Algorithm.SelectionSort;

    public TimeRecorder timeRecorder;
    public Gradient gradientForData;

    private delegate void SortDelegate(float[] arr);

    private SortDelegate _sort;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        _displaySorting = DisplaySorting.instance;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SelecteSortAlgorithm()
    {
        switch (algorithm)
        {
            case Algorithm.SelectionSort:
                if (!displayMode) _sort = SelectionSort;
                else _sort = SelectionSortDisplay;
                break;
            case Algorithm.BubbleSort:
                if (!displayMode) _sort = BubbleSort;
                else _sort = BubbleSortDisPlay;
                break;
            case Algorithm.InsertionSort:
                if (!displayMode) _sort = InsertionSort;
                else _sort = InertionSortDisplay;
                break;
            case Algorithm.MergeSort:
                if (!displayMode) _sort = MergeSort;
                else _sort = MergeSortDisplay;
                break;
            case Algorithm.QuickSort:
                if (!displayMode) _sort = QuickSort;
                else _sort = QuickSortDisplay;
                break;

            default:
                if (!displayMode) _sort = QuickSort;
                else _sort = QuickSortDisplay;
                break;
        }
    }


    public void Sort()
    {
        SelecteSortAlgorithm();
        _sort(_displaySorting.arr);
    }

    public void SortForRecording()
    {
        SelecteSortAlgorithm();
        Stats statsToWriteIn = timeRecorder.GetStats(algorithm);

        // // warm up the sorting function;
        // _displaySorting.Randomize();
        // // _displaySorting.RegenerateArray();
        // _sort(_displaySorting.arr);

        for (int i = 0; i < 6; i++)
        {
            _displaySorting.OnSizeSelected(i);

            for (int j = 0; j < 10; j++)
            {
                //prepare the array
                if (testOnRandomizedArray)
                {
                    _displaySorting.Randomize();
                }
                // _displaySorting.RegenerateArray();

                // sort the array and count time;
                float startTime = Time.realtimeSinceStartup;
                _sort(_displaySorting.arr);
                float endTime = Time.realtimeSinceStartup;
                float timeInterval = endTime - startTime;
                float timeIntervalInMicroSecond = timeInterval * 1000000f;
                statsToWriteIn.timeRecords[i, j] = timeIntervalInMicroSecond;
            }
        }

        statsToWriteIn.CalculateStats();

        // statsToWriteIn.Print();
    }

    public void ShowDataOnSortTable()
    {
        SortTable sortTable = GetSortTable();
        Stats statsToRead = timeRecorder.GetStats(algorithm);

        // for 6 types of size: 10, 50,100,500,1000,5000 
        for (int i = 0; i < 6; i++)
        {
            float min = statsToRead.min[i, 0];
            sortTable.dataLines[i].min.text = min.ToString("F2");
            sortTable.dataLines[i].min.color = DeriveColorFromData(min);

            float max = statsToRead.max[i, 0];
            sortTable.dataLines[i].max.text = max.ToString("F2");
            sortTable.dataLines[i].max.color = DeriveColorFromData(max);

            float med = statsToRead.med[i, 0];
            sortTable.dataLines[i].med.text = med.ToString("F2");
            sortTable.dataLines[i].med.color = DeriveColorFromData(med);

            float avg = statsToRead.avg[i, 0];
            sortTable.dataLines[i].avg.text = avg.ToString("F2");
            sortTable.dataLines[i].avg.color = DeriveColorFromData(avg);
        }
    }

    Color DeriveColorFromData(float data)
    {
        float t = Mathf.Log10(data + 1) / 5;
        Color color = gradientForData.Evaluate(t);
        return color;
    }


    SortTable GetSortTable()
    {
        SortTable sortTable;
        if (testOnRandomizedArray)
        {
            switch (algorithm)
            {
                case Algorithm.InsertionSort:
                    sortTable = UIManager.instance.insertionSortTable;
                    break;
                case Algorithm.BubbleSort:
                    sortTable = UIManager.instance.bubbleSortTable;
                    break;
                case Algorithm.SelectionSort:
                    sortTable = UIManager.instance.selectionSortTable;
                    break;
                case Algorithm.MergeSort:
                    sortTable = UIManager.instance.mergeSortTable;
                    break;
                case Algorithm.QuickSort:
                    sortTable = UIManager.instance.quickSortTable;
                    break;
                default:
                    sortTable = UIManager.instance.selectionSortTable;
                    break;
            }
        }
        else
        {
            switch (algorithm)
            {
                case Algorithm.InsertionSort:
                    sortTable = UIManager.instance.insertionSortTableSorted;
                    break;
                case Algorithm.BubbleSort:
                    sortTable = UIManager.instance.bubbleSortTableSorted;
                    break;
                case Algorithm.SelectionSort:
                    sortTable = UIManager.instance.selectionSortTableSorted;
                    break;
                case Algorithm.MergeSort:
                    sortTable = UIManager.instance.mergeSortTableSorted;
                    break;
                case Algorithm.QuickSort:
                    sortTable = UIManager.instance.quickSortTableSorted;
                    break;
                default:
                    sortTable = UIManager.instance.selectionSortTableSorted;
                    break;
            }
        }

        return sortTable;
    }


    #region SelectionSort

    void SelectionSort(float[] array)
    {
        int len = array.Length;
        for (int i = 0; i < len - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < len; j++)
            {
                if (array[j] < array[minIndex]) minIndex = j;
            }

            (array[i], array[minIndex]) = (array[minIndex], array[i]);
        }
    }

    void SelectionSortDisplay(float[] array)
    {
        StartCoroutine(SelectionSortCoroutineHelper(array));
    }

    IEnumerator SelectionSortCoroutineHelper(float[] array)
    {
        int len = array.Length;
        int minIndex;
        for (int i = 0; i < len - 1; i++)
        {
            minIndex = i;
            for (int j = i + 1; j < len; j++)
            {
                if (array[j] < array[minIndex]) minIndex = j;
                yield return new WaitForEndOfFrame();
            }

            (array[i], array[minIndex]) = (array[minIndex], array[i]);
            // yield return new WaitForEndOfFrame();
        }
    }

    #endregion

    #region BubbleSort

    void BubbleSort(float[] array)
    {
        int len = array.Length;
        if (len < 2) return;
        for (int i = 0; i < len - 1; i++)
        {
            bool didSwap = false;
            for (int j = 0; j < len - 1 - i; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    didSwap = true;
                }
            }

            if (!didSwap) return;
        }
    }

    void BubbleSortDisPlay(float[] array)
    {
        StartCoroutine(BubbleSortCorutine(array));
    }

    IEnumerator BubbleSortCorutine(float[] array)
    {
        int len = array.Length;
        if (len < 2) yield break;
        for (int i = 0; i < len - 1; i++)
        {
            bool didSwap = false;
            for (int j = 0; j < len - 1 - i; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    didSwap = true;
                }

                yield return new WaitForEndOfFrame();
            }

            if (!didSwap) yield break;
        }
    }

    #endregion

    #region InsertionSort

    void InsertionSort(float[] array)
    {
        int len = array.Length;
        int preIndex;
        float currentValue;
        for (int i = 1; i < len; i++)
        {
            preIndex = i - 1;
            currentValue = array[i];
            while (preIndex >= 0 && array[preIndex] > currentValue)
            {
                array[preIndex + 1] = array[preIndex];
                preIndex--;
            }

            array[preIndex + 1] = currentValue;
        }
    }

    void InertionSortDisplay(float[] array)
    {
        StartCoroutine(InsertionSortCoroutine(array));
    }

    IEnumerator InsertionSortCoroutine(float[] array)
    {
        int len = array.Length;
        int preIndex;
        float currentValue;
        for (int i = 1; i < len; i++)
        {
            preIndex = i - 1;
            currentValue = array[i];
            while (preIndex >= 0 && array[preIndex] > currentValue)
            {
                array[preIndex + 1] = array[preIndex];
                preIndex--;
                yield return new WaitForEndOfFrame();
            }

            array[preIndex + 1] = currentValue;
        }
    }

    #endregion

    #region MergeSort

    void MergeSort(float[] array)
    {
        MergeSortHelper(array, 0, array.Length - 1);
    }

    void MergeSortHelper(float[] array, int left, int right)
    {
        if (left < right)
        {
            int middle = (left + right) / 2;

            // Recursively sort the two halves
            MergeSortHelper(array, left, middle);
            MergeSortHelper(array, middle + 1, right);

            // Merge the sorted halves
            Merge(array, left, middle, right);
        }
    }

    void Merge(float[] array, int left, int middle, int right)
    {
        int len1 = middle - left + 1;
        int len2 = right - middle;

        float[] leftArray = new float[len1];
        float[] rightArray = new float[len2];

        int i = 0;
        int j = 0;

        for (i = 0; i < len1; ++i) leftArray[i] = array[left + i];
        for (j = 0; j < len2; ++j) rightArray[j] = array[middle + 1 + j];

        i = 0;
        j = 0;

        int k = left;

        while (i < len1 && j < len2)
        {
            if (leftArray[i] <= rightArray[j])
            {
                array[k] = leftArray[i];
                i++;
                k++;
            }
            else
            {
                array[k] = rightArray[j];
                j++;
                k++;
            }
        }

        while (i < len1)
        {
            array[k] = leftArray[i];
            i++;
            k++;
        }

        while (j < len2)
        {
            array[k] = rightArray[j];
            j++;
            k++;
        }
    }

    void MergeSortDisplay(float[] array)
    {
        StartCoroutine(MergeSortCorutineHelper(array, 0, array.Length - 1, i => { }));
        // MergeSortCorutineHelper(array, 0, array.Length - 1);
    }

    IEnumerator MergeSortCorutineHelper(float[] array, int left, int right, System.Action<int> callback)
    {
        int i = 0;
        if (left < right)
        {
            int middle = (left + right) / 2;
            yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();

            // Recursively sort the two halves
            StartCoroutine(MergeSortCorutineHelper(array, left, middle,
                x =>
                {
                    StartCoroutine(MergeSortCorutineHelper(array, middle + 1, right,
                        y =>
                        {
                            StartCoroutine(MergeCoroutine(array, left, middle, right,
                                z => { callback(i); }));
                        }));
                }));

            // Merge the sorted halves
        }

        else
        {
            callback(i);
        }
    }

    IEnumerator MergeCoroutine(float[] array, int left, int middle, int right, System.Action<int> callback)
    {
        int len1 = middle - left + 1;
        int len2 = right - middle;

        float[] leftArray = new float[len1];
        float[] rightArray = new float[len2];


        int i = 0;
        int j = 0;
        for (i = 0; i < len1; i++) leftArray[i] = array[left + i];
        for (j = 0; j < len2; j++) rightArray[j] = array[middle + 1 + j];

        i = 0;
        j = 0;
        int k = left;

        while (i < len1 && j < len2)
        {
            if (leftArray[i] <= rightArray[j])
            {
                array[k] = leftArray[i];
                i++;
                k++;
            }
            else
            {
                array[k] = rightArray[j];
                j++;
                k++;
            }

            yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
        }

        while (i < len1)
        {
            array[k] = leftArray[i];
            i++;
            k++;
            yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
        }

        while (j < len2)
        {
            array[k] = rightArray[j];
            j++;
            k++;
            yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
        }

        yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
        callback(k);
    }

    #endregion MergeSort

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

    void QuickSortDisplay(float[] array)
    {
        StartCoroutine(QuickSortHelperCoroutine(array, 0, array.Length - 1, i => { }));
    }

    IEnumerator QuickSortHelperCoroutine(float[] array, int low, int high, System.Action<int> callback)
    {
        if (low < high)
        {
            StartCoroutine(Partition(array, low, high,
                i =>
                {
                    StartCoroutine(QuickSortHelperCoroutine(array, low, i - 1,
                        i1 =>
                        {
                            StartCoroutine(QuickSortHelperCoroutine(array, i + 1, high,
                                i2 => { callback(0); }));
                        }));
                }));
        }
        else
        {
            callback(0);
        }

        yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
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
                yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
            }

            while (i < j && array[i] <= pivot)
            {
                i++;
                yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
            }

            (array[i], array[j]) = (array[j], array[i]);
            yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
        }

        (array[low], array[i]) = (array[i], array[low]);
        yield return usingPauseInterval ? new WaitForSeconds(pauseInterval) : new WaitForEndOfFrame();
        callback(i);
    }

    #endregion
}