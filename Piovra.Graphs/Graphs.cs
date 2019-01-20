﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra.Graphs {
    public class Node<V> : IEquatable<Node<V>> where V : IEquatable<V> {
        public static Node<V> Of(V vertex) => new Node<V>(vertex);
        Node(V vertex) => Vertex = vertex;
        public V Vertex { get; }
        public int Id { get; set; }

        public bool Equals(Node<V> other) {
            if (other == null) {
                return false;
            }
            return Vertex.Equals(other.Vertex);
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals(obj as Node<V>);
        }

        public override int GetHashCode() {
            return Vertex.GetHashCode();
        }
    }

    public interface IGraph<V> where V : IEquatable<V> {
        int NV { get; }
        int NE { get; }
        Node<V> NodeOf(V vertex);
        IEnumerable<Node<V>> AllNodes();
        IEnumerable<Node<V>> Neighbors(Node<V> node);        
    }

    public abstract class Graph<V, E> : IGraph<V>
        where E : IEdge<V> where V : IEquatable<V> {
        readonly Dictionary<Node<V>, HashSet<E>> _adj = new Dictionary<Node<V>, HashSet<E>>();
        readonly Dictionary<V, Node<V>> _map = new Dictionary<V, Node<V>>();
        int _nodeId;

        public int NV { get; private set; }
        public int NE { get; private set; }

        public void AddVertex(V vertex) {
            InsertVertex(vertex);
        }

        public abstract void AddEdge(E edge);

        public IEnumerable<Node<V>> AllNodes() {
            return _adj.Keys;
        }

        public IEnumerable<Node<V>> Neighbors(Node<V> node) {
            return _adj[node].Select(x => x.Tail);
        }        

        public Node<V> NodeOf(V vertex) {
            if (!_map.ContainsKey(vertex)) {
                return null;
            }
            return _map[vertex];
        }

        public IEnumerable<E> IncidentEdges(Node<V> node) {
            return _adj[node];
        }

        public IEnumerable<E> AllEdges() {
            var res = new HashSet<E>();
            foreach (var node in AllNodes()) {
                var incidentEdges = IncidentEdges(node);
                res.Extend(incidentEdges);
            }            
            return res;
        }

        void InsertEdge(Node<V> head, E edge) {
            var headNode = InsertVertex(edge.Head.Vertex);
            var tailNode = InsertVertex(edge.Tail.Vertex);

            edge.Head.Id = headNode.Id;
            edge.Tail.Id = tailNode.Id;

            var set = _adj[head];
            if (!set.Contains(edge)) {
                set.Add(edge);
                ++NE;
            }
        }

        Node<V> InsertVertex(V vertex) {
            var node = Node<V>.Of(vertex);
            if (!_adj.ContainsKey(node)) {
                node.Id = _nodeId++;
                _adj.Add(node, new HashSet<E>());
                _map.Add(vertex, node);
                ++NV;
            } else {
                node = _map[vertex];
            }
            return node;
        }

        protected void AddEdgeForUndirected(E edge) {
            InsertEdge(edge.Head, edge);
            InsertEdge(edge.Tail, edge);
        }

        protected void AddEdgeForDirected(E edge) {
            InsertEdge(edge.Head, edge);
        }
    }

    public interface IUnweightedGraph<V> where V : IEquatable<V> { }
    public interface IWeightedGraph<V> where V : IEquatable<V> { }
    public interface IDirectedGraph<V> where V : IEquatable<V> { }
    public interface IUndirectedGraph<V> where V : IEquatable<V> {}

    public class UnweightedUndirectedGraph<V> : Graph<V, Edge<V>>, IUnweightedGraph<V>, IUndirectedGraph<V>
        where V : IEquatable<V> {
        public override void AddEdge(Edge<V> edge) {
            AddEdgeForUndirected(edge);
        }
    }

    public class UnweightedDirectedGraph<V> : Graph<V, Edge<V>>, IUnweightedGraph<V>, IDirectedGraph<V>
        where V : IEquatable<V> {
        public override void AddEdge(Edge<V> edge) {
            AddEdgeForDirected(edge);
        }
    }

    public class WeightedUndirectedGraph<V> : Graph<V, WeightedEdge<V>>, IWeightedGraph<V>, IUndirectedGraph<V>
        where V : IEquatable<V> {
        public override void AddEdge(WeightedEdge<V> edge) {
            AddEdgeForUndirected(edge);
        }
    }

    public class WeightedDirectedGraph<V> : Graph<V, WeightedEdge<V>>, IWeightedGraph<V>, IDirectedGraph<V>
        where V : IEquatable<V> {
        public override void AddEdge(WeightedEdge<V> edge) {
            AddEdgeForDirected(edge);
        }
    }
}