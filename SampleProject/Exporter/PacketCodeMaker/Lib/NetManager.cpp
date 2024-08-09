#include "NetManager.h"
#include "Networking.h"
#include "NetAgent.h"



NetManager::NetManager()
{
	mAgents.AddUninitialized((int)SERVER::MAX);

	mAgents[(int)SERVER::LOGIN] = new NetAgent(new Net_Login_C2S(), new Net_Login_S2C());
	mAgents[(int)SERVER::GAME] = new NetAgent(new Net_Game_C2S(), new Net_Game_S2C());
	mAgents[(int)SERVER::BATTLE] = new NetAgent(new Net_Battle_C2S(), new Net_Battle_S2C());
}

NetManager::~NetManager()
{
	for (int i = 0; i < (int)SERVER::MAX; ++i)
	{
		delete mAgents[i];
	}

	mAgents.Empty();
}

void NetManager::Connect(SERVER type, uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port)
{
	mAgents[(int)type]->Connect(ip_a, ip_b, ip_c, ip_d, port);
}

void NetManager::ConnectLogin(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port)
{
	mAgents[(int)SERVER::LOGIN]->Connect(ip_a, ip_b, ip_c, ip_d, port);
}

void NetManager::ConnectGame(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port)
{
	mAgents[(int)SERVER::GAME]->Connect(ip_a, ip_b, ip_c, ip_d, port);
}

void NetManager::ConnectBattle(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port)
{
	mAgents[(int)SERVER::BATTLE]->Connect(ip_a, ip_b, ip_c, ip_d, port);
}

void NetManager::Tick(SERVER type)
{
	mAgents[(int)type]->Tick();
}

void NetManager::Tick()
{
	for (int i = 0; i < (int)SERVER::MAX; ++i)
	{
		mAgents[i]->Tick();
	}
}

void NetManager::Close(SERVER type)
{
	mAgents[(int)type]->Close();
}

void NetManager::Close()
{
	for (int i = 0; i < (int)SERVER::MAX; ++i)
	{
		mAgents[i]->Close();
	}
}

Net_Login_C2S* NetManager::GetLogin()
{
	return (Net_Login_C2S*)mAgents[(int)SERVER::LOGIN]->GetSender();
}

Net_Game_C2S* NetManager::GetGame()
{
	return (Net_Game_C2S*)mAgents[(int)SERVER::GAME]->GetSender();
}

Net_Battle_C2S* NetManager::GetBattle()
{
	return (Net_Battle_C2S*)mAgents[(int)SERVER::BATTLE]->GetSender();
}