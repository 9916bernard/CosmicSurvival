#pragma once

#include "../Net_Login_S2C.h"
#include "../Net_Login_C2S.h"
#include "../Net_Game_S2C.h"
#include "../Net_Game_C2S.h"
#include "../Net_Battle_S2C.h"
#include "../Net_Battle_C2S.h"
#include "./NetAgent.h"


class FSocket;
class NetBuffer;
class NetSender;
class NetReceiver;
class NetAgent;


class NetManager
{

public:
	enum class SERVER : int
	{
		LOGIN = 0,
		GAME = 1,
		BATTLE = 2,
		MAX =3,
	};

private:
	TArray<NetAgent*> mAgents;

public:
	NetManager();
	~NetManager();

	void Connect(SERVER type, uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port);
	void ConnectLogin(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port);
	void ConnectGame(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port);
	void ConnectBattle(uint8 ip_a, uint8 ip_b, uint8 ip_c, uint8 ip_d, int port);

	void Tick(SERVER type);
	void Tick();
	void Close(SERVER type);
	void Close();

	Net_Login_C2S* GetLogin();
	Net_Game_C2S* GetGame();
	Net_Battle_C2S* GetBattle();

	template<class H, class T>
	void RegisterFunc(SERVER type, T* ptr)
	{
		((H*)mAgents[(int)type]->GetReceiver())->template RegisterFunc<T>(ptr);
	}

	template<class T1, class T2, class T3>
	void RegisterFunc(T1* ptr1, T2* ptr2, T3* ptr3)
	{
		RegisterFunc<Net_Login_S2C, T1>(SERVER::LOGIN, ptr1);
		RegisterFunc<Net_Game_S2C, T2>(SERVER::GAME, ptr2);
		RegisterFunc<Net_Battle_S2C, T3>(SERVER::BATTLE, ptr3);
	}
};

