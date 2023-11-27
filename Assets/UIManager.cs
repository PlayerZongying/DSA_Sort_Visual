using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using Unity.UI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    private DisplaySorting _displaySorting;
    private Sorter _sorter;

    public TextMeshProUGUI toggleButtonText;

    public GameObject measureModePanel;
    public GameObject displayModePanel;

    [Space]
    public TMP_Dropdown sizeDropdownInDisplay;
    public TMP_Dropdown algorithmDropdownInDisplay;
    public TMP_Dropdown algorithmDropdownInMeasure;
    public TMP_Dropdown arrayTypeDropdownInMeasure;

    [Space] 
    public SortTable insertionSortTable;
    public SortTable bubbleSortTable;
    public SortTable selectionSortTable;
    public SortTable mergeSortTable;
    public SortTable quickSortTable;

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
            displayModePanel.SetActive(false);
            measureModePanel.SetActive(true);
            SetAlgorithmDropDown(algorithmDropdownInMeasure);
        }
        else
        {
            toggleButtonText.text = "Display Mode";
            measureModePanel.SetActive(false);
            displayModePanel.SetActive(true);
            SetSizeDropDown(sizeDropdownInDisplay);
            SetAlgorithmDropDown(algorithmDropdownInDisplay);
            SetArrayTypeDropDown(arrayTypeDropdownInMeasure);
        }
    }

    public void SetArrayTypeDropDown(TMP_Dropdown arrayTypeDropDown)
    {
        if (_sorter.testOnRandomizedArray)
        {
            arrayTypeDropDown.value = 0;
        }
        else
        {
            arrayTypeDropDown.value = 1;
        }
    
    }

    public void SetSizeDropDown(TMP_Dropdown sizeDropDown)
    {
        DisplaySorting.ArraySize arraySize = _displaySorting.arraySize;
        switch (arraySize)
        {
            case DisplaySorting.ArraySize.Ten:
                sizeDropDown.value = 0;
                break;
            case DisplaySorting.ArraySize.Fifty:
                sizeDropDown.value = 1;
                break;
            case DisplaySorting.ArraySize.Hundred:
                sizeDropDown.value = 2;
                break;
            case DisplaySorting.ArraySize.FiveHundred:
                sizeDropDown.value = 3;
                break;
            case DisplaySorting.ArraySize.Thousand:
                sizeDropDown.value = 4;
                break;
            case DisplaySorting.ArraySize.FiveThousand:
                sizeDropDown.value = 5;
                break;
        }
    }
    
    public void SetAlgorithmDropDown(TMP_Dropdown algorithmDropDown)
    {
        Sorter.Algorithm algorithm = _sorter.algorithm;
        switch (algorithm)
        {
            case Sorter.Algorithm.InsertionSort:
                algorithmDropDown.value = 0;
                break;
            case Sorter.Algorithm.BubbleSort:
                algorithmDropDown.value = 1;
                break;
            case Sorter.Algorithm.SelectionSort:
                algorithmDropDown.value = 2;
                break;
            case Sorter.Algorithm.MergeSort:
                algorithmDropDown.value = 3;
                break;
            case Sorter.Algorithm.QuickSort:
                algorithmDropDown.value = 4;
                break;
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
    }

    public void OnArrayTypeSelected(int index)
    {
        switch (index)
        {
            case 0:
                _sorter.testOnRandomizedArray = true;
                break;
            case 1:
                _sorter.testOnRandomizedArray = false;
                break;
        }
    }
    
}