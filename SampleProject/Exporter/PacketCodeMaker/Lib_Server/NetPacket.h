#pragma once


#ifndef _NET_PACKET_HEADER_
#define _NET_PACKET_HEADER_

#pragma pack(1)

struct NetPacketHeader
{
private:
	///���� unsigned short�� �Ѿ�� �ʴ´ٰ� �����Ѵ�.
	unsigned short m_nSize;

public:
	/**
	@brief ������.
	@param nSize ��Ŷ ����.
	*/
	NetPacketHeader(size_t nSize) : m_nSize((unsigned short)nSize)
	{
	}

	//int GetLenByteSize() { return sizeof(unsigned short); }
	static unsigned int GetLenByteSize() { return 2; }

	/**
	@brief ��Ŷ ���� ������ �޼ҵ�
	@return ��Ŷ ����
	*/
	unsigned short GetLen() { return m_nSize; }

	/**
	@brief ��Ŷ ���̸� �����Ѵ�.
	@param nSize ��Ŷ ����
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