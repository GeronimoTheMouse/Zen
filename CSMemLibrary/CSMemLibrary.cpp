// CSMemLibrary.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <Windows.h>
#include <string>

//ReadAddInt
extern "C" __declspec(dllexport) int ReadAddInt(int* address, HANDLE processHandle)
{
	int data = 0;

	ReadProcessMemory(processHandle, address, &data, sizeof(int), 0);

	return data;
}

//ReadAddFloat
extern "C" __declspec(dllexport) float ReadAddFloat(int* address, HANDLE processHandle)
{
	float data = 0.0f;

	ReadProcessMemory(processHandle, address, &data, sizeof(float), 0);

	return data;
}

//WriteFloat
extern "C" __declspec(dllexport) void WriteFloat(int* address, HANDLE processHandle, float value)
{
	WriteProcessMemory(processHandle, address, &value, sizeof(value), nullptr);
}


//WriteBool
extern "C" __declspec(dllexport) void WriteBool(int* address, HANDLE processHandle, bool value)
{
	WriteProcessMemory(processHandle, address, &value, sizeof(value), nullptr);
}

//GetTheKeyState
extern "C" __declspec(dllexport) int GetTheKeyState(int key)
{
	return GetAsyncKeyState(key);
}

//GetWindowLng
extern "C" __declspec(dllexport) int GetWindowLng(HWND hWnd, int nIndex)
{
	return GetWindowLongA(hWnd, nIndex);
}

//SetWindowLng
extern "C" __declspec(dllexport) int SetWindowLng(HWND hWnd, int nIndex, int dwNewLong)
{
	return SetWindowLongA(hWnd, nIndex, dwNewLong);
}

//DoubleClick
extern "C" __declspec(dllexport) void DoubleClick()
{
	mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
	mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
}

//MoveMouseTo
extern "C" __declspec(dllexport) void MoveMouseTo(int x, int y)
{
	mouse_event(MOUSEEVENTF_MOVE, x, y, 0, 0);
}