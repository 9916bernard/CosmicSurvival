#pragma once

#include <string>


typedef uint16 UTF16CHAR;


#define TCHAR_TO_UTF16(str) (UTF16CHAR*)(str)


class FStringArray
{
private:
	SIZE_T mNum;
public:
	__forceinline FStringArray() : mNum(0) { }
	__forceinline FStringArray(size_t num) : mNum((SIZE_T)num) { }
	__forceinline void SetNum(size_t num) { mNum = (SIZE_T)num; }
	__forceinline SIZE_T Num() const { return mNum; }
};

class FString
{
private:
	FStringArray mArray;
	wchar_t* mStr;

public:
	__forceinline FString(uint16* Src) : FString((wchar_t*)Src) { }
	__forceinline FString(wchar_t* Src)
	{
		size_t len = wcslen(Src);
		mStr = new wchar_t[len + 1];
		wcscpy_s(mStr, len + 1, Src);
		mStr[len] = 0;
		mArray.SetNum(len + 1);
	}
	__forceinline ~FString() { delete[] mStr; }
	__forceinline const wchar_t* operator*() const { return mStr; }
	__forceinline const FStringArray& GetCharArray() const { return mArray; }
};

struct FMemory
{
	static __forceinline void Memcpy(void* Dest, const void* Src, SIZE_T Count)
	{
		memcpy(Dest, Src, Count);
	}
};