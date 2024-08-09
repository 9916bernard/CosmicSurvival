#pragma once


#ifndef _NET_PACKET_HEADER_
#define _NET_PACKET_HEADER_

#pragma pack(1)

struct NetPacketHeader
{
private:
	///길이 unsigned short을 넘어서지 않는다고 가정한다.
	unsigned short m_nSize;

public:
	/**
	@brief 생성자.
	@param nSize 패킷 길이.
	*/
	NetPacketHeader(size_t nSize) : m_nSize((unsigned short)nSize)
	{
	}

	//int GetLenByteSize() { return sizeof(unsigned short); }
	static unsigned int GetLenByteSize() { return 2; }

	/**
	@brief 패킷 길이 얻어오는 메소드
	@return 패킷 길이
	*/
	unsigned short GetLen() { return m_nSize; }

	/**
	@brief 패킷 길이를 수정한다.
	@param nSize 패킷 길이
	*/
	void ResetLen(size_t nSize) { m_nSize = (unsigned short)nSize; }
};

struct CZLibHeader : NetPacketHeader {
	bool m_bCompress;
	unsigned short m_nUnCompSize;
	CZLibHeader(size_t nSize) : NetPacketHeader(nSize), m_bCompress(false), m_nUnCompSize(0) {
	}
public:

	bool isCompress() { return m_bCompress; }
	void SetCompress() {
		m_bCompress = true;
	}
	void SetCompressSize(unsigned short nUnCompressSize, unsigned short nCompressSize) {
		ResetLen(nCompressSize);
		m_nUnCompSize = nUnCompressSize;
	}
};


#pragma pack()

#endif