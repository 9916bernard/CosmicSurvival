#pragma once

#include "./NetPacket.h"

//#define LOGC(Format, ...) UE_LOG(LogTemp, Log, TEXT(Format), ##__VA_ARGS__)

class NetBuffer
{
public:
	NetBuffer();
	NetBuffer(uint32 bufferSize);
	~NetBuffer();

private:
	uint8* mRecvBuffer;
	uint8* mRecvBufferReserve;

	uint32 mRecvBufferSizeMax;
	uint32 mRecvBufferStart;
	uint32 mRecvBufferEnd;

public:
	uint32 GetUsableSize()
	{
		return mRecvBufferSizeMax - mRecvBufferEnd;
	}

	uint32 GetDataSize()
	{
		return mRecvBufferEnd - mRecvBufferStart;
	}

	uint8* GetDataPtr()
	{
		return mRecvBuffer + mRecvBufferStart;
	}

	uint8* GetWritePtr()
	{
		return mRecvBuffer + mRecvBufferEnd;
	}

	uint32 GetPacketSize()
	{
		NetPacketHeader* header = (NetPacketHeader*)GetDataPtr();

		return header->GetLen();
	}

	bool HasOnePacket(uint32& packetSize)
	{
		uint32 dataSize = GetDataSize();

		if (dataSize < NetPacketHeader::GetLenByteSize())
		{
			return false;
		}

		packetSize = GetPacketSize();
		if (dataSize < packetSize)
		{
			return false;
		}

		return true;
	}

	void SwapBuffer()
	{
		uint32 dataSize = GetDataSize();

		if (dataSize > 0)
		{
			FMemory::Memcpy(mRecvBufferReserve, GetDataPtr(), dataSize);
		}

		mRecvBufferStart = 0;
		mRecvBufferEnd = dataSize;

		uint8* temp = mRecvBuffer;
		mRecvBuffer = mRecvBufferReserve;
		mRecvBufferReserve = temp;
	}

	void SetUseBuffer(uint32 useByte)
	{
		mRecvBufferEnd += useByte;

		//LOGC("===== SetUseBuffer  Start:%d End:%d UseByte:%d", mRecvBufferStart, mRecvBufferEnd, useByte);

		if (mRecvBufferEnd < mRecvBufferSizeMax)
		{
			// success
		}
		else if (mRecvBufferEnd == mRecvBufferSizeMax)
		{
			uint32 packetSize = 0;
			if (HasOnePacket(packetSize) == false)
			{
				//LOGC("===== SetUseBuffer  Buffer Swap");
				SwapBuffer();
			}
		}
		else
		{
			// critical error
		}
	}

	void PostExecutePacket(uint32 packetSize)
	{
		mRecvBufferStart += packetSize;

		//LOGC("===== PostExecutePacket  Start:%d End:%d PacketSize:%d", mRecvBufferStart, mRecvBufferEnd, packetSize);

		if (mRecvBufferStart == mRecvBufferSizeMax && mRecvBufferEnd == mRecvBufferSizeMax)
		{
			mRecvBufferStart = 0;
			mRecvBufferEnd = 0;
		}
	}
};