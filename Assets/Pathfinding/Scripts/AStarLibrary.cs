using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  A* Library
	
	Author -  Cole Barach
	Created - 2022.02.07
	Updated - 2022.09.16
	
	Function
		- Collection of A* Pathfinding Functions
        - Call FindPath() for generic usage
        - Creation of a path between a starting and ending point based on the A* algorithm
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class AStarLibrary : MonoBehaviour {
    // Frontend function for library use
    public static Vector3[] FindPath(Texture2D obstruction, Vector3 start, Vector3 end, Transform transform) {
        Vector3 localStart = transform.InverseTransformPoint(start);
        Vector3 localEnd   = transform.InverseTransformPoint(end);
        Vector2Int start2D = new Vector2Int((int)Mathf.Round(localStart.x),(int)Mathf.Round(localStart.z));
        Vector2Int end2D   = new Vector2Int((int)Mathf.Round(localEnd.x),  (int)Mathf.Round(localEnd.z));
        Vector2Int[] path2D = GetPath2D(obstruction,start2D,end2D);
        if(path2D == null) print("Error Finding Path");
        return TransformPath2D(path2D, transform);
    }

    // Find a path using local texture coordinates
    public static Vector2Int[] GetPath2D(Texture2D obstruction, Vector2Int start, Vector2Int end) {
        List<Node> open   = new List<Node>();
        List<Node> closed = new List<Node>();
        
        Node parent = new Node(start);
        parent.UpdateCosts(start,end);
        open.Add(parent);
        
        int step = 0;
        while(open.Count != 0 && step < 512) {
            Node cheapest = GetCheapestNode(open);
            if(cheapest.hCost <= 0) {
               Vector2Int[] path = GetNodePath(cheapest);
               return path;
            }
            open.Remove(cheapest);
            closed.Add(cheapest);
            AppendNeighbors(cheapest,start,end,open,closed,obstruction);
            step++;
        }
        return GetNodePath(GetClosestNode(closed));
    }
    public static Vector2Int[] GetNodePath(Node node) {
        List<Vector2Int> path = new List<Vector2Int>();
        while(node.parent != null) {
            path.Add(new Vector2Int(node.x,node.y));
            node = node.parent;
        }
        path.Add(new Vector2Int(node.x,node.y));
        return path.ToArray();
    }
    public static Vector3[] TransformPath2D(Vector2Int[] path, Transform transform) {
        Vector3[] transformedPath = new Vector3[path.Length];
        for(int n = 0; n < path.Length; n++) {
            transformedPath[n] = transform.TransformPoint(new Vector3(path[n].x, 0, path[n].y));
        }
        return transformedPath;
    } 
    
    public static Node GetCheapestNode(List<Node> nodes) {
        Node cheapest = nodes[0];
        for(int x = 1; x < nodes.Count; x++) {
            if(cheapest.fCost > nodes[x].fCost) cheapest = nodes[x];
        }
        return cheapest;
    }
    public static Node GetClosestNode(List<Node> nodes) {
        Node closest = nodes[0];
        for(int x = 1; x < nodes.Count; x++) {
            if(closest.hCost > nodes[x].hCost) closest = nodes[x];
        }
        return closest;
    }

    public static int GetHeuristicDistance(Vector2Int a, Vector2Int b) {
        return GetHeuristicDistance(a.x,a.y,b.x,b.y);
    }
    public static int GetHeuristicDistance(int ax, int ay, int bx, int by) {
        int dx = (ax-bx)*10;
        int dy = (ay-by)*10;
        return (int)Mathf.Sqrt(dx*dx + dy*dy);
    }

    public static void AppendNeighbors(Node node, Vector2Int start, Vector2Int end, List<Node> open, List<Node> closed, Texture2D obstruction) {
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                if(x == 0 && y == 0) continue;
                Node neighbor = new Node(node.x+x, node.y+y);
                neighbor.parent = node;
                neighbor.UpdateCosts(start, end);
                if(neighbor.CheckAppension(open,closed,obstruction)) open.Add(neighbor);
            }
        }
    }

    public class Node {
        public Node parent;
        public int x;
        public int y;
        public int gCost;
        public int hCost;
        public int fCost;

        public Node(int positionX, int positionY) {
            x = positionX;
            y = positionY;
        }
        public Node(Vector2Int position) {
            x = position.x;
            y = position.y;
        }

        public bool CheckObstruction(Texture2D obstruction) {
            if(x < 0 || x >= obstruction.width)  return false;
            if(y < 0 || y >= obstruction.height) return false;
            return obstruction.GetPixel(x,y) == Color.white;
        }
        public bool CheckDuplicate(Node node) {
            return (x == node.x) && (y == node.y);
        }
        public bool CheckAppension(List<Node> open, List<Node> closed, Texture2D obstruction) {
            return !CheckListDuplicates(open,closed) && CheckObstruction(obstruction);
        }
        public bool CheckListDuplicates(List<Node> open, List<Node> closed) {
            for(int n = 0; n < Mathf.Max(open.Count,closed.Count); n++) {
                if(n < open.Count) {
                    if(CheckDuplicate(open[n])) {
                        if(open[n].fCost > fCost) {
                            open[n].SetCosts(this);
                        }
                        return true;
                    }
                }
                if(n < closed.Count) {
                    if(CheckDuplicate(closed[n]) && closed[n].fCost < fCost) {
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateCosts(Vector2Int start, Vector2Int end) {
            if(parent == null) {
                gCost = GetHeuristicDistance(x,y,start.x,start.y);
            } else {
                gCost = GetHeuristicDistance(x,y,parent.x,parent.y) + parent.gCost;
            }
            hCost = GetHeuristicDistance(x,y,end.x,end.y);
            fCost = gCost + hCost;
        }
        public void SetCosts(Node node) {
            parent = node.parent;
            gCost = node.gCost;
            hCost = node.hCost;
            fCost = node.fCost;
        }
    }
}
