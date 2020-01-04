package com.lftc.lab2;

import java.util.Scanner;

public class Main {

    public static void main(final String[] args) {
        final Scanner scan = new Scanner(System.in);
        System.out.println("HamiltonianCycle Algorithm Test\n");
        final int[][] graph = {
            {0, 1, 0, 1, 1},
            {0, 0, 1, 1, 0},
            {0, 1, 0, 1, 1},
            {1, 1, 1, 0, 1},
            {0, 1, 1, 0, 0},
        };

        final HamiltoneanCycle hc = new HamiltoneanCycle();
        hc.findHamiltonianCycle(graph);
    }

}

