# SharpUtils

C# Data Structures and Algorithm Library

### Data Classes:

- `Complex` - complex number implementation with operator overloads

- `DisjointSet` - disjoint-set with almost constant amortized time per operation [link](http://en.wikipedia.org/wiki/Disjoint-set_data_structure)

- `Heap<T>` - heap [link](http://en.wikipedia.org/wiki/Heap_(data_structure))

- `MultiSet<T>` - C# implementation of std::multiset [link](http://www.cplusplus.com/reference/set/multiset/)

- `Quaternion` - quaternion implementation with operator overloads

- `RBTree<K, V, A>` - red-black tree with augmentation [link](https://en.wikipedia.org/wiki/Introduction_to_Algorithms)

- `SegTree<T>` - segment (interval) tree, which allows custom associative function [link](http://en.wikipedia.org/wiki/Segment_tree)

- `SuffixTree<T>` - suffix tree with O(n) construction [link](http://www.cise.ufl.edu/~sahni/dsaaj/enrich/c16/suffix.htm) [link](http://stackoverflow.com/questions/9452701/ukkonens-suffix-tree-algorithm-in-plain-english)

- `Trie<T>` - prefix tree [link](http://en.wikipedia.org/wiki/Trie)

### Algorithms:
	
- Arrays:
  - MaxCommonSubstring - longest common substring for up to 64 words in O(n) [link](http://en.wikipedia.org/wiki/Longest_common_substring_problem)	
  - RMQ - range minimal query (with sparse table) in O(1) query time and O(n log n) preprocessing time
  - ZFunc - Z algorithm [link](http://codeforces.com/blog/entry/3107)

- Combinatorics: 
  - inversion count, next permutation
  
- FFT - Fast Fourier Transform [link](http://en.wikipedia.org/wiki/Fast_Fourier_transform)

- Geometry:
  - Convex hull - Andrew's algorithm in O(n log n) [link](http://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain)
  
  - area, line intersection, linear transformation, distance from point to line
	
- Graph:
  - FloydWarshal - all pairs shortest paths [link](http://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm)

  - Dijkstra - single source shortest path [link](http://en.wikipedia.org/wiki/Dijkstra's_algorithm)

  - Dinitz - maximum flow in O(V^2 * E) [link](http://en.wikipedia.org/wiki/Dinic's_algorithm)
		
- Number Theory:
  - base conversion, Euler totient function, prime factorization, greatest common divisor, fast exponentiation