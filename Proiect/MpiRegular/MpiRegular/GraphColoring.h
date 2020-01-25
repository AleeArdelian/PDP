#pragma once
#include<set>
#include<vector>

using namespace std;


class GraphColoring
{
public:
	GraphColoring();
	~GraphColoring();

	set<int> getIndependentSet(set<int>& independentSet) {
		set<int> res;
		vector<int> list;
		list.reserve(independentSet.size());
		list.insert(list.end(), independentSet.begin(), independentSet.end());

		for (int i = 0; i < list.size(); i++) {
			if (checkVertex(list.at(i)) == true) {
				res.insert(list.at(i));
			}
		}
		return res;
	}

	bool checkVertex(int v) {
		vector<int> neigh = getNeight(v)
	}

	vector<int> getNeigh(int v) {
		for (int i = 0; i<)
		return graph.get(v).stream().filter((node)->{
			return getColor(node) == -1;
		}).collect(Collectors.toList());
	}

private:
	int E;
	int V;
	set<int> colors;
};

GraphColoring::GraphColoring()
{

}

GraphColoring::~GraphColoring()
{
}



