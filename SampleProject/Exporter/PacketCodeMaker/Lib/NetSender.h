#pragma once


class NetAgent;

class NetSender
{
public:
	NetSender();

	void Init(NetAgent* agent);

protected:
	NetAgent* mNetAgent;

	void Send(const uint8* packet, int32 len);
};

