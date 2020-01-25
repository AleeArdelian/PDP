package com.ppd.graph;

import java.util.*;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.stream.Collectors;


class Graph {

    private int E;
    private int V;

    private final Set<Integer> independentSet;

    private final Map<Integer, NodeInfo> vInfo;

    private Set<Integer> colors;

    private Map<Integer, Set<Integer>> graph;

    private ExecutorService exec;

    public Graph(int nrOfVertices) {
        this.V = nrOfVertices;
        graph = new HashMap<>();
        vInfo = new HashMap<>();
        for (int i = 0; i < nrOfVertices; i++) {
            graph.put(i, new HashSet<>());
            vInfo.put(i, new NodeInfo(-1, getRandomNumberInRange(0, 100)));
        }
        colors = new TreeSet<>();
        independentSet = new HashSet<>(graph.keySet());
        for (int i = 0; i < 100; i++) {
            colors.add(i);
        }
        exec = Executors.newFixedThreadPool(8);
    }

    private static int getRandomNumberInRange(int min, int max) {

        if (min >= max) {
            throw new IllegalArgumentException("max must be greater than min");
        }

        Random r = new Random();
        return r.nextInt((max - min) + 1) + min;
    }

    public void addEdge(int src, int dest) {
        E++;
        graph.get(src).add(dest);
        graph.get(dest).add(src);
    }

    public List<Integer> getNeigh(int v) {
        return graph.get(v).stream().filter((node) -> {
            return getColor(node) == -1;
        }).collect(Collectors.toList());
    }

    public void colorGraph() {
        while (!independentSet.isEmpty()) {
            Set<Integer> set = getIndependentSet();
            for (Integer v : set) {
                exec.submit(() -> {
                    setColor(v);
                });
            }
            independentSet.removeAll(set);
        }
        exec.shutdown();
    }

    public void printColors() {
        for (Integer v : vInfo.keySet())
            System.out.println("V:" + v + " color:" + getColor(v) + " random:" + getValue(v));
    }

    public Integer getValue(int v) {
        return vInfo.get(v).random;
    }

    public Integer getColor(int v) {
        return vInfo.get(v).color;
    }

    private boolean checkVertext(int v) {
        for (Integer neigh : getNeigh(v)) {
            if (getValue(neigh) > getValue(v))
                return false;
            else if (getValue(neigh) == getValue(v) && v > neigh)
                return false;
        }
        return true;
    }

    public Set<Integer> getNeighColors(int v) {
        return getAllNeighs(v).stream().filter((node) -> {
            return getColor(node) != -1;
        }).map(node -> getColor(node)).distinct().collect(Collectors.toSet());
    }

    public Set<Integer> getAllNeighs(int v) {
        return graph.get(v);
    }

    public Integer getSmallestColor(int v) {
        Set<Integer> neighColors = getNeighColors(v);
        for (Integer c : colors) {
            if (!neighColors.contains(c))
                return c;
        }
        return 0;
    }

    public void setColor(int v) {
        vInfo.get(v).color = getSmallestColor(v);
    }

    public Set<Integer> getIndependentSet() {
        ArrayList<Future<Boolean>> list = new ArrayList<>();
        Set<Integer> res = new HashSet<>();
        List<Integer> l = new ArrayList<>(independentSet);
        for (Integer v : l) {
            Future<Boolean> f = exec.submit(() -> checkVertext(v));
            list.add(f);
        }

        for (int i = 0; i < independentSet.size(); i++) {
            try {
                if (list.get(i).get() == true)
                    res.add(l.get(i));
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            }
        }
        return res;
    }
}
