#SharpUtils

C# Data Structures and Algorithm Library

###Data Classes:

- DisjointSet - disjoint-set with almost constant amortized time per operation [link](http://en.wikipedia.org/wiki/Disjoint-set_data_structure)

- MultiSet<T> - C# implementation of std::multiset [link](http://www.cplusplus.com/reference/set/multiset/)

- Trie<T> - prefix tree [link](http://en.wikipedia.org/wiki/Trie)

- SuffixTree<T> - suffix tree with O(n) construction [link](http://www.cise.ufl.edu/~sahni/dsaaj/enrich/c16/suffix.htm) [link](http://stackoverflow.com/questions/9452701/ukkonens-suffix-tree-algorithm-in-plain-english)

- SegTree<T> - segment (interval) tree, which allows custom associative function [link](http://en.wikipedia.org/wiki/Segment_tree)

- Heap<T> - heap [link](http://en.wikipedia.org/wiki/Heap_(data_structure))
	
- Complex - complex number implementation with operator overloads

- Quaternion - quaternion implementation with operator overloads

###Algorithms:
	
- FFT - Fast Fourier Transform [link](http://en.wikipedia.org/wiki/Fast_Fourier_transform)
	
- Graph:
  - Dijkstra - single source shortest path [link](http://en.wikipedia.org/wiki/Dijkstra's_algorithm)

  - Dinitz - maximum flow in O(V^2 * E) [link](http://en.wikipedia.org/wiki/Dinic's_algorithm)

  - FloydWarshal - all pairs shortest paths [link](http://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm)
		
- Combinatorics: 
  - inversion count, next permutation
		
- Number Theory:
  - base conversion, Euler totient function, prime factorization, greatest common divisor, fast exponentiation
	
- Geometry:
  - Convex hull - Andrew's algorithm in O(n log n) [link](http://en.wikibooks.org/wiki/Algorithm_Implementation/Geometry/Convex_hull/Monotone_chain)
  
  - area, line intersection, linear transformation, distance from point to line

- Arrays:
  - RMQ - range minimal query (with sparse table) in O(1) query time and O(n log n) preprocessing time
  - ZFunc - Z algorithm [link](http://codeforces.com/blog/entry/3107)
  - MaxCommonSubstring - longest common substring for up to 64 words in O(n) [link](http://en.wikipedia.org/wiki/Longest_common_substring_problem)
				
