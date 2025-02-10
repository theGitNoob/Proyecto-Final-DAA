/*
Problem A
Link in codefoces:
https://codeforces.com/problemset/problem/1730/F



Problem:
You are given a permutation p of length n and a positive integer k. Consider a
permutation q of length n such that for any integers i and j, where 1 ≤ i < j ≤
n, we have: pᵢ ≤ pⱼ + k Find the minimum possible number of inversions in a
permutation q. A permutation is an array consisting of n distinct integers from
1 to n in arbitrary order. For example, [2,3,1,5,4] is a permutation, but
[1,2,2] is not a permutation (2 appears twice in the array) and [1,3,4] is also
not a permutation (n = 3 but there is 4 in the array). An inversion in a
permutation a is a pair of indices i and j (1 ≤ i,j ≤ n) such that i < j, but aᵢ
> aⱼ. Input:

The first line contains two integers n and k (1 ≤ n ≤ 5000, 1 ≤ k ≤ 8).
The second line contains n distinct integers p₁, p₂, ..., pₙ (1 ≤ pᵢ ≤ n).

Output:
Print the minimum possible number of inversions in the permutation q.

*/

#include <bits/stdc++.h>
#define MAX(a, b) ((a) > (b)) ? (a) : (b)
#define MIN(a, b) ((a) < (b)) ? (a) : (b)
#define forl(i, x, n) for (int i = (x); i <= (int)(n); i++)
#define lsone(x) ((x) & -(x))

using namespace std;

const int MAXN = 5e3 + 5;

int input_array[MAXN];
int number_pos[MAXN]; // Stores the number position in the originar array: 5 is
                      // in the 3rd position in the input_array
int memo[1 << 10][MAXN];
int Fenwick_Tree[MAXN]; // The binary indexed tree(BIT) array
int n, initial_rating, t, k;

int query(int k) {
  int sum = 0;
  for (int i = k; i < MAXN; i += lsone(i))
    sum += Fenwick_Tree[i];

  return sum;
}

void update(int k, int v) {
  for (int i = k; i > 0; i -= lsone(i))
    Fenwick_Tree[i] += v;
}

int get_rightmost_zero(int mask) {

  forl(i, 0, k) {
    if (!(mask & (1 << i)))
      return i;
  }
  return k + 1;
}

int dp(int block_start, int mask) {
  if (block_start > n)
    return 0;

  int &memoized_value = memo[mask][block_start];

  if (memoized_value != -1)
    return memoized_value;

  int curr_sol =
      INT_MAX; // Set a bigger value than the possible biggest optimal solution

  for (int idx = 0; idx <= k && block_start + idx <= n; idx++) {
    if (mask & (1 << idx))
      continue; // Avoid processing twice the same number
    int original_pos = number_pos[block_start + idx];

    int new_mask = mask | (1 << idx);
    int rt_zero = get_rightmost_zero(new_mask);

    int cost = query(original_pos);
    update(original_pos, 1);
    curr_sol =
        min(curr_sol, dp(block_start + rt_zero, new_mask >> rt_zero) + cost);
    update(original_pos, -1);
  }

  return memoized_value = curr_sol;
}
void solve() {
  cin >> n >> k;

  forl(i, 1, n) {
    cin >> input_array[i];
    number_pos[input_array[i]] = i;
  }
  // Fills the memo table with -1
  forl(idx, 1, n) {
    forl(mask, 0, (1 << (k + 1)) - 1) { memo[mask][idx] = -1; }
  }
  cout << dp(1, 0);
}
int32_t main() {

  ios_base::sync_with_stdio(0);
  cin.tie(0);
  solve();

  return 0;
}
