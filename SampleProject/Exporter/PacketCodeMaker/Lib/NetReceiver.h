#pragma once

#include "./Delegate.h"


class NetReceiver
{
public:
	NetReceiver() {}
	virtual ~NetReceiver() {}

protected:
	SA::delegate<void(bool)> mOnConnect;
	SA::delegate<void()> mOnDisconnect;

public:
	virtual void OnConnect(bool success)
	{
		if (mOnConnect.isNull() == false) mOnConnect(success);
	};

	virtual void OnDisconnect()
	{
		if (mOnDisconnect.isNull() == false) mOnDisconnect();
	}

	virtual void OnReceive(uint8* data) = 0;

	void RegistOnConnect(SA::delegate<void(bool)> func)
	{
		mOnConnect = func;
	}

	void RegistOnDisconnect(SA::delegate<void()> func)
	{
		mOnDisconnect = func;
	}
};

