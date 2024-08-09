#include "NetBuffer.h"



NetBuffer::NetBuffer()
    : mRecvBuffer(nullptr)
    , mRecvBufferReserve(nullptr)
    , mRecvBufferSizeMax(512)
    , mRecvBufferStart(0)
    , mRecvBufferEnd(0)
{
    mRecvBuffer = new uint8[mRecvBufferSizeMax];
    mRecvBufferReserve = new uint8[mRecvBufferSizeMax];
}

NetBuffer::NetBuffer(uint32 bufferSize)
    : mRecvBuffer(nullptr)
    , mRecvBufferReserve(nullptr)
    , mRecvBufferSizeMax(bufferSize)
    , mRecvBufferStart(0)
    , mRecvBufferEnd(0)
{
    mRecvBuffer = new uint8[mRecvBufferSizeMax];
    mRecvBufferReserve = new uint8[mRecvBufferSizeMax];
}

NetBuffer::~NetBuffer()
{
    if (mRecvBuffer != nullptr)
        delete[] mRecvBuffer;

    if (mRecvBufferReserve != nullptr)
        delete[] mRecvBufferReserve;

    mRecvBufferStart = 0;
    mRecvBufferEnd = 0;
}