package com.lftc.lab2;

import java.util.Arrays;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

/**
 * @author ciprian.mosincat on 12/3/2017.
 */
public class HamiltoneanCycle {
    private int v;
    private volatile int[][] graph;

    /**
     * Function to find cycle
     **/
    public void findHamiltonianCycle(final int[][] g) {
        v = g.length;
        final int[] path = new int[v];

        Arrays.fill(path, -1);
        graph = g;
        try {
            path[0] = 0;
            solve(0, path, 1);
            System.out.println("Solution not found");
        } catch (final Exception e) {
            if(!e.getMessage().contains("Solution found")){
                throw new RuntimeException(e);
            }
        }
    }

    /**
     * function to find paths recursively
     **/
    public void solve(final int vertex, final int[] path, int count) throws Exception {
        if (graph[vertex][0] == 1 && count == v) {
            display(path);
            throw new Exception("Solution found");
        }
        if (count == v)
            return;

        for (int i = 0; i < this.v; i++) {
            /** if connected **/
            if (graph[vertex][i] == 1) {
                /** add to path **/
                path[count++] = i;
                /** remove connection **/
                graph[vertex][i] = 0;
                graph[i][vertex] = 0;


                /** if vertex not already selected  solve recursively **/
                if (!isPresent(i, path,count)) {
                    final int current = i;
                    final int currentCount = count;
                    final ExecutorService service = Executors.newSingleThreadExecutor();
                    final Runnable task = () -> {
                        try {
                            solve(current, path, currentCount);
                        } catch (final Exception e) {
                            throw new RuntimeException(e.getMessage());
                        }
                    };
                    //service.submit(task);
                    final Future<?> future = service.submit(task);
                    future.get();
                }

                /** restore connection **/
                graph[vertex][i] = 1;
                graph[i][vertex] = 1;
                /** remove path **/
                path[--count] = -1;
            }
        }
    }

    /**
     * function to check if path is already selected
     **/
    private boolean isPresent(final int v, final int[] path, final int count) {
        for (int i = 0; i < count - 1; i++)
            if (path[i] == v)
                return true;
        return false;
    }

    /**
     * display solution
     **/
    private void display(final int[] path) {
        System.out.print("\nPath : ");
        for (int i = 0; i <= v; i++)
            System.out.print(path[i % v] + " ");
        System.out.println();
    }

}
