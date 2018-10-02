
public class ScoreDataObserver
{
    protected ScoreDataSubject model;

    public ScoreDataObserver(ScoreDataSubject subject)
    {
        this.model = subject;
        model.Attach(this);
    }
    
    public virtual void OnNotify()
    {
        
    }

}
