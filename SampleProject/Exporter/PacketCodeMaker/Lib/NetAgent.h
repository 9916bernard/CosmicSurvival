#pragma once


class FSocket;
class NetBuffer;
class NetSender;
class NetReceiver;

class NetAgent
{

public:
	enum class STATE : int
	{
		DISCONNECTED,
		TRY_CONNECT,
		CONNECTED,
	};


public:
	NetAgent();
	NetAgent(NetSender* sender, NetReceiver* receiver);
	~NetAgent();

	void Connect(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port);
	void Tick();
	void Close();


private:
	FSocket* mNetSocket;
	NetBuffer* mRecvBuffer;
	NetSender* mSender;
	NetReceiver* mReceiver;
	STATE mState;
	bool mDisconnetReserve;

public:
	FSocket* GetSocket() { return mNetSocket; }
	void SetDisconnectReserve() { mDisconnetReserve = true; }
	bool IsConnected() { return mState == STATE::CONNECTED; }
	NetSender* GetSender() { return mSender; }
	NetReceiver* GetReceiver() { return mReceiver; }
};

