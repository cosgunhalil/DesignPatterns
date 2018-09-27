
public class Observer
{
    protected ScoreDataSubject model;

    public Observer(ScoreDataSubject subject)
    {
        this.model = subject;
        model.Attach(this);
    }
    
    public virtual void OnNotify()
    {
        
    }

}
