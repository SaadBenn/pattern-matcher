using System;
using System.Collections.Generic;

/*
Two solutions are given below
1. Uses the substring and dictionary - Runtime O(n*n) where n is the length of the string and O(n) space for the dictionary + new string
2. Trie Data Structure - Here is my github repo (from 2018) where the data structure is implemented in python -> O(nlogk) where k is pattern length

https://github.com/SaadBenn/Trie-for-auto-suggestion

Inspiration from here: https://www.cs.upc.edu/~ps/downloads/tst/tst.html
3 . Another solution that might work is KMP algorithm - (not implemented here)

Assumptions
 1. pattern is assumed to be a contiguous overlapping substring i.e (ABCD, 2) -> (AB, BC, CD)
 2. pattern length k is assumed positive i.e negative length k is invalid
 3. pattern length k is upto max size of int32
 4. string consists of only ascii characters
*/

public class SubStringDict
{
	public static void processPatterns(String s, int k) {
		if (k > s.Length || k < 0) {
			Console.WriteLine("Input string must be >= in length than the value k");
			return;
		}
		
		if (string.IsNullOrEmpty(s)) {
			Console.WriteLine("Input String is empty");
			return;
		}
		
		Dictionary<string, int> dict = new Dictionary<string, int>();
		for (int i = 0; i <= s.Length-k; i++) {
			// get a substring of pattern length k  - makes a new copy worst case: O(n) time
			String sub = s.Substring(i, k);
			
			if (dict.ContainsKey(sub)) {
				dict[sub]+=1;
			} else {
				dict[sub] = 1;
			}
		}
		
		foreach(KeyValuePair<string, int> entry in dict) {
			// TODO: can be expanded to an m count instead of > 1
			if (entry.Value > 1) {
				Console.WriteLine(entry.Key + "->" + entry.Value);
			}
		}
	}
}


public class TrieSolution {
	String s;
	int k;
	
	public TrieSolution(string s, int k) {
		this.s = s;
		this.k = k;
	}
	
	// keeping the scope private as only this class should be able to have access to the method.
	private Node root {get; set;}
	
	public void addPatterns(char[] curr_pattern, Node node, int pos, int curr_length) {
		char letter = s[pos];
		if (node == null) {
			if (root == null) {
				root = new Node(letter);
			}
			node = root;
		}
		
		if (letter > node.letter) {
			if (node.right == null) {
				node.right = new Node(letter);
			}
			addPatterns(curr_pattern, node.right, pos, curr_length);	
		} else if (letter < node.letter) {
			if (node.left == null) {
				node.left = new Node(letter);
			}
			addPatterns(curr_pattern, node.left, pos, curr_length);
		} else {
			if (curr_length == (k-1)) {
				curr_pattern[k-1] = letter;
				node.isEndOfWord = true;
				node.freq+=1;
				node.word = curr_pattern;
			} else {
				if (pos+1 < s.Length) {
					curr_pattern[curr_length] = letter;
					pos+=1;
					letter = s[pos];
					curr_length+=1;
					
					if (node.center == null) {
						node.center = new Node(letter);
						addPatterns(curr_pattern, node.center, pos, curr_length);
					} else if (letter < node.center.letter) {
						if (node.left == null) {
							node.left = new Node(letter);
						}
						addPatterns(curr_pattern, node.left, pos, curr_length);
					} else if (letter > node.center.letter) {
						if (node.right == null) {
							node.right = new Node(letter);
						}
						addPatterns(curr_pattern, node.right, pos, curr_length);
					} else {
						addPatterns(curr_pattern, node.center, pos, curr_length);
					}
				}
			}
		}
	}
	
	public void print(Node node) {		
		if (node.left != null) {
			print(node.left);
		}
		
		if (node.center != null) {
			print(node.center);
		}
		
		if (node.right != null) {
			print(node.right);
		}
		
		if (node.isEndOfWord) {
			String word = new String(node.word);
			if (node.freq > 1) {
				Console.WriteLine(word + " with freq " + node.freq);
			}
		}
	}
	
	public void helperPrint() {
		print(root);
	}
}


public class Node {
	/*
	Node class is based on 2-3 search tree
	// https://en.wikipedia.org/wiki/2%E2%80%933_tree
	*/
    public Node (char letter) {
      	this.letter = letter;
		this.freq = 0;
      	this.isEndOfWord = false;
    }
	 // when letter == parent node
	  public Node center { get; set; }
    public char letter { get; set; }
    public bool isEndOfWord { get; set; }
    public char[] word { get; set; }
    // left node contains > than the parent node letters
    public Node left { get; set; }
    // right node contains > than the parent node letters
    public Node right { get; set; }
	// freq of the pattern
	public int freq { get; set; }
 }
