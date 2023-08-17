using Godot;
using System;
using System.Collections.Generic;

public partial class CircularBuffer<T> : GodotObject
{
	private Queue<T> buffer;
	private int maxLength;
	
	public CircularBuffer(int MaxLength){
		if (MaxLength <= 0){
			throw new ArgumentException("MaxLength must be positive", nameof(MaxLength));
		}
		
		maxLength = MaxLength;
		buffer = new Queue<T>(MaxLength);
	}
	
	public void Add(T element){
		if (buffer.Count >= maxLength)
			buffer.Dequeue();
			
		buffer.Enqueue(element);
	}
	
	public void Grow(){
		Queue<T> BufferNew = new Queue<T>(maxLength+1);
		foreach (T element in buffer){
			BufferNew.Enqueue(element);
		}
		maxLength++;
		buffer = BufferNew;
	}
	public int Count => buffer.Count;
	public IEnumerable<T> Elements => buffer;
	
}
