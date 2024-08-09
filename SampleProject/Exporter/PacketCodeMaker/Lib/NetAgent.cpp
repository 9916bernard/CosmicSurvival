#include "NetAgent.h"
#include "Networking.h"
#include "Sockets.h"
#include "SocketSubsystem.h"
#include "./NetPacket.h"
#include "NetBuffer.h"
#include "NetSender.h"
#include "NetReceiver.h"



NetAgent::NetAgent()
	: mNetSocket(nullptr)
	, mRecvBuffer(nullptr)
	, mSender(nullptr)
	, mReceiver(nullptr)
	, mState(STATE::DISCONNECTED)
	, mDisconnetReserve(false)
{
}

NetAgent::NetAgent(NetSender* sender, NetReceiver* receiver)
	: mNetSocket(nullptr)
	, mRecvBuffer(nullptr)
	, mSender(sender)
	, mReceiver(receiver)
	, mState(STATE::DISCONNECTED)
	, mDisconnetReserve(false)
{
}


NetAgent::~NetAgent()
{
	Close();
}


void NetAgent::Connect(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port)
{
	if (mState != STATE::DISCONNECTED)
	{
		return;
	}

	if (mNetSocket == nullptr)
	{
		mNetSocket = ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->CreateSocket(NAME_Stream, TEXT("default"), false);
	}

	if (mRecvBuffer == nullptr)
	{
		mRecvBuffer = new NetBuffer(1024);
	}

	mSender->Init(this);

	FIPv4Address ip(ip_a, ip_b, ip_c, ip_d);

	TSharedRef<FInternetAddr> addr = ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->CreateInternetAddr();
	addr->SetIp(ip.Value);
	addr->SetPort(port);

	mNetSocket->SetNoDelay();
	mNetSocket->SetNonBlocking();

	mNetSocket->Connect(*addr);

	mState = STATE::TRY_CONNECT;
}


void NetAgent::Tick()
{
	if (mNetSocket == nullptr || mRecvBuffer == nullptr)
	{
		return;
	}


	if (mDisconnetReserve == true)
	{
		mDisconnetReserve = false;
		mReceiver->OnDisconnect();
		return;
	}


	if (mState == STATE::TRY_CONNECT)
	{
		if (mNetSocket->GetConnectionState() == SCS_Connected)
		{
			mState = STATE::CONNECTED;

			mReceiver->OnConnect(true);
		}
		else if (mNetSocket->GetConnectionState() == SCS_ConnectionError)
		{
			mState = STATE::DISCONNECTED;

			mReceiver->OnConnect(false);
		}
	}


	uint32 pendingByte = 0;
	while (mNetSocket->HasPendingData(pendingByte) == true)
	{
		//LOGC(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> PendingData");

		int32 readByte = 0;
		mNetSocket->Recv(mRecvBuffer->GetWritePtr(), mRecvBuffer->GetUsableSize(), readByte);

		mRecvBuffer->SetUseBuffer(readByte);

		uint32 packetSize = 0;
		while (mRecvBuffer->HasOnePacket(packetSize) == true)
		{
			mReceiver->OnReceive(mRecvBuffer->GetDataPtr());

			mRecvBuffer->PostExecutePacket(packetSize);
		}
	}
}


void NetAgent::Close()
{
	if (mRecvBuffer != nullptr)
	{
		delete mRecvBuffer;
		mRecvBuffer = nullptr;
	}

	if (mNetSocket != nullptr)
	{
		mNetSocket->Close();
		ISocketSubsystem::Get(PLATFORM_SOCKETSUBSYSTEM)->DestroySocket(mNetSocket);
		mNetSocket = nullptr;
	}

	if (mSender != nullptr)
	{
		delete mSender;
		mSender = nullptr;
	}

	if (mReceiver != nullptr)
	{
		delete mReceiver;
		mReceiver = nullptr;
	}

	mState = STATE::DISCONNECTED;
	mDisconnetReserve = false;
}