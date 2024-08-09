using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class RandomTable
{
    private short mCount = 0;

    private ushort[] mBuffer = null;

    private short mIndex = 0;


    public int ParseData(byte[] data, int start)
    {
        int dataPos = start;

        // Count
        mCount = BitConverter.ToInt16(data, dataPos);
        dataPos += sizeof(short);

        if (mBuffer == null || mBuffer.Length < mCount)
        {
            mBuffer = new ushort[mCount];
        }

        int bufferLen = mCount * sizeof(ushort);

        Buffer.BlockCopy(data, dataPos, mBuffer, 0, bufferLen);

        dataPos += bufferLen;

        return dataPos;
    }

    public void Make(short InCount)
    {
        mCount = InCount;

        if (mBuffer == null || mBuffer.Length < mCount)
        {
            mBuffer = new ushort[mCount];
        }

        for (int i = 0; i < mBuffer.Length; ++i)
        {
            mBuffer[i] = (ushort)UnityEngine.Random.Range(0, 100000);
        }
    }

    public ushort Get()
    {
        ushort num = mBuffer[mIndex];

        mIndex++;

        if (mIndex >= mCount)
        {
            mIndex = 0;
        }

        return num;
    }

    public ushort Dis(ushort InMin, ushort InMax)
    {
        ushort num = Get();

        ushort gap = InMax;

        gap -= InMin;

        gap += 1;

        if (gap <= 0)
        {
            return 0;
        }

        num %= gap;

        num += InMin;

        return num;
    }

    public ushort Current()
    {
        return mBuffer[mIndex];
    }
}