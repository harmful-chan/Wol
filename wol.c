#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <sys/types.h>
#include <arpa/inet.h>

#define LOG_D(s, ...) printf(s, __VA_ARGS__)
unsigned char atox(unsigned char a)
{
	if( a >= 'a'  && a <= 'z')
		return a - 'a' + 10;
	else if( a >= 'A' && a <= 'Z' )
		return a - 'A' + 10;
	else if( a >= '0' && a <= '9' )
		return a - '0';
}
	
int main (int argc, char * argv[]) 
{
	
	unsigned char* mac;
	unsigned char* ip;
	unsigned char* p;
	unsigned char magicPackage[108] = {0};
	unsigned char macBytes[6] = {0};
	int i = 0;
	int sockfd;
	struct sockaddr_in addr;

	//入参数量检测
	if( argc == 3 )
	{
		mac = argv[2];	
		ip = argv[1];
	}
	else
	{
		LOG_D("入参数量错误，argc=%d\n", argc);
		return 0;
	}
	
	//mac地址检测
	for( p = mac, i = 0; *p != '\0'; p++, i++ );
	if( i != 12 )
	{	
		LOG_D("mac格式错误, i=%d\n", i);
		return 0;
	}
	
	//mac转换
	for( p = mac, i = 0; *p != '\0'; i++, p += 2)
	{
		macBytes[i] |= atox(*p) << 4;
		macBytes[i] |= atox(*(p+1));
	}
	
	//魔术包封装
	for( i = 0; i < 6; i++)
		magicPackage[i] = 0xFF;
	for(i = 6; i < 108; i += 6)
	{
		int j=0;
		for( j = 0; j < 6; j++ )
			magicPackage[i+j] = macBytes[j];
	}
	
	for(i = 0; i < 108; i++)
		LOG_D("%02x ", magicPackage[i]);
	

	//建立UDP包
	sockfd = socket(AF_INET, SOCK_DGRAM, 0);
	if( sockfd < 0 )
	{
		LOG_D("建立socket错误，sockfd=%d\n", sockfd);
		return 0;
	}

	bzero(&addr, sizeof(struct sockaddr_in));
	addr.sin_family = AF_INET;
	addr.sin_port = htons(9);
	if( inet_aton(ip, &addr.sin_addr) < 0 )
	{
		LOG_D("ip地址转化错误，ip=%s\n", ip);
		return 0;
	}
	
	sendto(sockfd, magicPackage, 108, 0, &addr, 108);

	close(sockfd);
	return 0;
}
