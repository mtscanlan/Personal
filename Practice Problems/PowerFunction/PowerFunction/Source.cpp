#include <string>

float power(float x, int y)
{
	float temp;
	if (y == 0)
		return 1;
	temp = power(x, y / 2);
	if (y % 2 == 0)
		return temp*temp;
	else
	{
		if (y > 0)
			return x*temp*temp;
		else
			return (temp*temp) / x;
	}
}

float powerTwo(float a, int b)
{
	double result = 1;
	while (b != 0) {
		if (b % 2 == 1) {
			result *= a;
		}
		b /= 2;
		a *= a;
	}
	return result;
}

/* Program to test function power */
int main()
{
	float x = 2;
	int y = 10;
	printf("%f", power(x, y)); // 1024
	printf("\n");
	printf("%f", powerTwo(x, y)); // 1024
	getchar();
	return 0;
}