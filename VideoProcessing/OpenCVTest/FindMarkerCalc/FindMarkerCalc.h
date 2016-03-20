// Based on the examples
// https://msdn.microsoft.com/en-us/library/ms235636.aspx
// http://www.codeproject.com/Articles/18032/How-to-Marshal-a-C-Class

#pragma once

#ifdef FINDMARKERCALC_EXPORTS
#define FINDMARKERCALC_API extern "C" __declspec(dllexport) 
#else
#define FINDMARKERCALC_API __declspec(dllimport) 
#endif

// This functions are exported from the FindMarkerCalc.dll
FINDMARKERCALC_API double Add(double a, double b);