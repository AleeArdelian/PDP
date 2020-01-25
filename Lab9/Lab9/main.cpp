#include <iostream>
#include <vector>
#include <algorithm>

void addRemainingCoefficients(std::vector<int> A, std::vector<int> B, int minDegree, int maxDegree, std::vector<int>& coefficients)
{
	if (minDegree != maxDegree)
	{
		if (maxDegree == A.size() - 1)
		{
			for (int i = minDegree + 1; i <= maxDegree; i++)
				coefficients.push_back(A[i]);
		}
		else
		{
			for (int i = minDegree + 1; i <= maxDegree; i++)
				coefficients.push_back(B[i]);
		}
	}
}
std::vector<int> slice(std::vector<int> A, int f, int s)
{
	auto first = A.cbegin() + f;
	auto second = A.cbegin() + s;

	std::vector<int> res(first, second);
	return res;
}
void printPolyinom(int Len, std::vector<int> result)
{
	for (int i = 0; i < Len; i++)
	{
		std::cout<<result[i];
		if (i != 0)
			std::cout<<"x^"<< i;
		if (i != Len - 1)
			std::cout<<" + ";
	}
	std::cout << std::endl;
}
std::vector<int> multiplySeq(std::vector<int> A, std::vector<int> B,int Len)
{
	std::vector<int> newRes(Len);
	std::fill(newRes.begin(),newRes.end(), 0);
	for (int i = 0; i < A.size(); i++)
		for (int j = 0; j < B.size(); j++)
			newRes[i + j] = newRes[i + j] + (A[i] * B[j]);
	return newRes;
}
std::vector<int> add(std::vector<int> A, std::vector<int> B)
{
	int minDegree = std::min(A.size() - 1, B.size() - 1);
	int maxDegree = std::max(A.size() - 1, B.size() - 1);
	std::vector<int> polynom(maxDegree + 1);

	//Add the 2 polynomials
	for (int i = 0; i <= minDegree; i++)
		polynom.push_back(A[i] + B[i]);
	
	addRemainingCoefficients(A, B, minDegree, maxDegree, polynom);
	return polynom;
}
std::vector<int> shift(std::vector<int> A, int offset)
{
	std::vector<int> polynom(offset);
	std::fill(polynom.begin(), polynom.end(), 0);
	for (int i = 0; i < A.size(); i++)
		polynom.push_back(A[i]);
	return polynom;
}
std::vector<int> subtract(std::vector<int> A, std::vector<int> B)
{
	int minDegree = std::min(A.size() - 1, B.size() - 1);
	int maxDegree = std::max(A.size() - 1, B.size() - 1);
	std::vector<int> polynom(maxDegree + 1);
	//Subtract the 2 polynomials
	for (int i = 0; i <= minDegree; i++)
		polynom.push_back(A[i] - B[i]);

	addRemainingCoefficients(A, B, minDegree, maxDegree, polynom);
	//remove coefficients starting from biggest power if coefficient is 0
	int k = polynom.size() - 1;
	while (polynom[k] == 0 && k > 0)
	{
		polynom.erase(polynom.begin()+k-1);
		k--;
	}
	return polynom;
}
std::vector<int> multiplicationKaratsubaMPI(std::vector<int> A, std::vector<int> B)
{
	int Len = A.size() + B.size() - 1;
	if (A.size()-1 < 2 || B.size()-1 < 2)
	{
		return multiplySeq(A,B,Len);
	}
	int len = std::max(A.size()-1, B.size()-1) / 2;

	auto lowP1 = slice(A, 0, len);
	auto highP1 = slice(A, len, A.size() - 1);
	auto lowP2 = slice(B, 0, len);
	auto highP2 = slice(B, len, B.size() - 1);

	auto z1 = multiplicationKaratsubaMPI(lowP1, lowP2);
	auto z2 = multiplicationKaratsubaMPI(add(lowP1, highP1), add(lowP2, highP2));
	auto z3 = multiplicationKaratsubaMPI(highP1, highP2);

	auto r1 = shift(z3, 2 * len);
	auto r2 = shift(subtract(subtract(z2, z3), z1), len);
	auto result = add(add(r1, r2), z1);
	return result;

}
int main()
{
	std::vector<int> A{ 1,2,3 };
	std::vector<int> B{ 1,2,3,4 };

	std::cout << "First pol:" << std::endl;
	printPolyinom(A.size(), A);
	std::cout << "Second pol:" << std::endl;
	printPolyinom(B.size(), B);

	auto result = multiplicationKaratsubaMPI(A, B);
	std::cout << "Result pol:" << std::endl;
	printPolyinom(result.size(), result);
}