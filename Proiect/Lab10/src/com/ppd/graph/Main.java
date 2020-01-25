package com.ppd.graph;

public class Main {

    public static void main(String[] args) {
        Graph g = new Graph(10);
        g.addEdge(0,2);
        g.addEdge(0,3);
        g.addEdge(0,5);
        g.addEdge(1,3);
        g.addEdge(1,4);
        g.addEdge(1,6);
        g.addEdge(2,4);
        g.addEdge(2,7);
        g.addEdge(3,8);
        g.addEdge(4,9);
        g.addEdge(5,6);
        g.addEdge(5,9);
        g.addEdge(6,7);
        g.addEdge(7,8);
        g.addEdge(8,9);
        long startTime = System.nanoTime();
        g.colorGraph();
        long endTime = System.nanoTime();

        long duration = (endTime - startTime);
        g.printColors();

        System.out.println("Duration:" + duration/1000000);
    }
}
