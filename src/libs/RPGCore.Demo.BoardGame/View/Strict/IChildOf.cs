namespace RPGCore.Demo.BoardGame;

public interface IChildOf<T>
{
	public T Parent { get; set; }
}
