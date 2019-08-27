using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sLOGGER
{
	class RingBuff
	{
		private double[] _vals;
		private int _sidx, _eidx, _vcnt, _vmax;
		public int Count() { 
			return (this._vcnt);
		}

		public int MaxCapacity { get; private set; }

		public RingBuff(int max)
		{
			this._vals = new double[max];
			this._vmax = max;
			this._vcnt = 0;
			this._sidx = 0;
			this._eidx = 0;
		}
		public void clear()
		{
			this._vcnt = 0;
			this._sidx = 0;
			this._eidx = 0;
		}
		public void add(double item)
		{
			int bidx = this._eidx;
			//---
			this._vals[this._eidx] = item;
			if (++this._eidx >= this._vmax) {
				this._eidx = 0;
			}
			this._vcnt++;
			if (this._eidx == this._sidx) {
				Console.WriteLine("ring到達:cnt=" + this._vcnt);
				this._sidx--;
				if (this._sidx < 0) {
					this._sidx = this._vmax - 1;
				}
			}
		}
		public int GetiMin()
		{
			return (this._vcnt - this._vmax);
		}
		public int GetiMax()
		{
			return (this._vcnt - 1);
		}
		public void GetMaxMin(int imin, int imax, out double fmax, out double fmin)
		{
			fmax = double.MinValue;
			fmin = double.MaxValue;
			if (imin < (this._vcnt - this._vmax)) {
				imin = (this._vcnt - this._vmax);
			}
			if (imax > (this._vcnt - 1)) {
				imax = (this._vcnt - 1);
			}
			for (int i = imin; i <= imax; i++) {
				double f = this[i];
				if (fmin > f) {
					fmin = f;
				}
				if (fmax < f) {
					fmax = f;
				}
			}
		}

		public double this[int i]
		{
			set {
				if (this._vcnt < this._vmax) {
					this._vals[i] = value;
				}
				else {
					// vmax:10, vcnt:16
					// eidx: 6(次書き込む位置)
					//[0-9][0,1,2,3,4,5,..]
					// 有効範囲: [6] - [15]
					if (i > (this._vcnt - 1) || i < (this._vcnt - this._vmax)) {
						//範囲外
					}
					else {
						int dif = this._vcnt-i;
						int idx = this._eidx - dif;
						if (idx < 0) {
							idx += this._vmax;
						}
						this._vals[idx] = value;
					}
				}
			}
			get {
				int idx;
				if (this._vcnt < this._vmax) {
					idx = i;
				}
				else {
					// vmax:10, vcnt:16
					// eidx: 6(次書き込む位置)
					//[0-9][0,1,2,3,4,5,..]
					// 有効範囲: [6] - [15]
					if (i > (this._vcnt - 1) || i < (this._vcnt - this._vmax)) {
						idx = -1;//範囲外
					}
					else {
						int dif = this._vcnt - i;
						idx = this._eidx - dif;
						if (idx < 0) {
							idx += this._vmax;
						}
					}
				}
				return(this._vals[idx]);
			}
		}
	//public T Dequeue() => this._queue.Dequeue();

	//public T Peek() => this._queue.Peek();

	//public IEnumerator<T> GetEnumerator() => this._queue.GetEnumerator();

	//IEnumerator IEnumerable.GetEnumerator() => this._queue.GetEnumerator();
	}
}
