public interface IClickable
{
    void SetListener(IClickListener clickListener);
    void OnClicked();
}

public interface IClickListener 
{
    void OnClicked(IClickable clickable);
}
