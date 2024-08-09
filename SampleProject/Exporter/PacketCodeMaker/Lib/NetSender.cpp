#include "NetSender.h"
#include "NetAgent.h"
#include "Sockets.h"


NetSender::NetSender()
    : mNetAgent(nullptr)
{
}

void NetSender::Init(NetAgent* agent)
{
    mNetAgent = agent;
};

void NetSender::Send(const uint8* packet, int32 len)
{
    if (mNetAgent->IsConnected() == false)
    {
        return;
    }

    int32 sendByte = 0;
    bool result = mNetAgent->GetSocket()->Send(packet, len, sendByte);

    if (result == false || sendByte != len)
    {
        mNetAgent->SetDisconnectReserve();
    }
}