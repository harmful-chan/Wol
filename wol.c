#include <stdio.h>

unsigned char atox(unsigned char a)
{
	if( a >= 'a'  && a <= 'z')
		return a - 'a' - 10;
	else if( a >= 'A' && a <= 'Z' )
		return a - 'A' - 10;
	else if( a >= '0' && a <= '9' )
		return a - '0';
}
	
int main (int argc, char * argv[]) 
{
	
	unsigned char* mac;
	unsigned char* p;
	unsigned char magicPackage[108] = {0};
	unsigned char macBytes[6] = {0};
	int i = 0;
	
	//入参数量检测
	if( argc == 2 )
		mac = argv[1];
	else
	{
		printf("入参数量错误，argc=%d\n", argc);
		return 0;
	}
	
	//mac地址检测
	for( p = mac, i = 0; *(p++) != '\0'; i++ );
	if( i != 12 )
		printf("mac格式国务");
	
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
		printf("%02x ", magicPackage[i]);
	
	return 0;
}
