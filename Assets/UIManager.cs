using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private DisplaySorting _displaySorting;
    private Sorter _sorter;

    public TextMeshProUGUI toggleButtonText;

    // Start is called before the first frame update
    void Start()
    {
        _displaySorting = DisplaySorting.instance;
        _sorter = Sorter.instance;
        SetToggleMode(_sorter.displayMode);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToogleMode()
    {
        _sorter.displayMode = !_sorter.displayMode;
        SetToggleMode(_sorter.displayMode);
    }

    public void SetToggleMode(bool isDisplay)
    {
        if (!isDisplay)
        {
            toggleButtonText.text = "Measure Mode";
        }
        else
        {
            toggleButtonText.text = "Display Mode";
        }
    }

    public void OnSizeSelected(int index)
    {
        switch (index)
        {
            case 0:
                _displaySorting.arraySize = DisplaySorting.ArraySize.Ten;
                break;
            case 1:
                _displaySorting.arraySize = DisplaySorting.ArraySize.Fifty;
                break;
            case 2:
                _displaySorting.arraySize = DisplaySorting.ArraySize.Hundred;
                break;
            case 3:
                _displaySorting.arraySize = DisplaySorting.ArraySize.FiveHundred;
                break;
            case 4:
                _displaySorting.arraySize = DisplaySorting.ArraySize.Thousand;
                break;
            case 5:
                _displaySorting.arraySize = DisplaySorting.ArraySize.FiveThousand;
                break;
        }

        _displaySorting.RegenerateArray();
    }

    public void OnAlgorithmSelected(int index)
    {
        switch (index)
        {
            case 0:
                _sorter.algorithm = Sorter.Algorithm.InsertionSort;
                break;
            case 1:
                _sorter.algorithm = Sorter.Algorithm.BubbleSort;
                break;
            case 2:
                _sorter.algorithm = Sorter.Algorithm.SelectionSort;
                break;
            case 3:
                _sorter.algorithm = Sorter.Algorithm.MergeSort;
                break;
            case 4:
                _sorter.algorithm = Sorter.Algorithm.QuickSort;
                break;
        }
        _sorter.Sort();
    }
}