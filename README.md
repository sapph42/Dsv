# Dsv
Library for Dynamically Separated Values

## Purpose

DSV is a truely cursed format. With a Dynamically Seperated Values file, the delimiter for each line is dynamically determined by the first non-doublequote character in the row.

## Usage

DSV is a static class containing two public methods.

### Dsv.ToDataTable Method

#### Overloads

| Method | Description |
| ---- | ---- |
| ToDataTable(string[], bool) | Converts a DSV string array to a DataTable, optionally with the first element in the array marked as field names |
| ToDataTable(string) | Converts a DSV string to a DataTable, treating the string as a single row |


#### ToDataTable(string[], bool)

Converts a DSV string array to a DataTable, optionally with the first element in the array marked as field names

```cs
public static DataTable ToDataTable(string[] dsv, bool hasHeaders);
```

##### Parameters

`dsv` string[]

A string array where each element maps to a line from a DSV file

`hasHeaders` bool

Indicates if the first row should be treated as field names for purposes of DataRow construction

##### Returns

DataTable

A DataTable that contains the parsed contents of the dsv string array

##### Exceptions

InvalidDataException
If any element in `dsv` contains an odd number of doublequotes

### Dsv.ToStringArr Method

#### ToStringArr(DataTable)

Converts a DataTable to a string array representing a DSV file

```cs
public static string[] ToStringArr(DataTable table);
```

##### Parameters

`table` DataTable

A DataTable of strings. The Captions of DataColumns will be considered the first row

##### Returns

string[]

A string array where each element maps to a line from a DSV file

##### Exceptions

InvalidDataException
If the first column of any row contains only doublequotes
