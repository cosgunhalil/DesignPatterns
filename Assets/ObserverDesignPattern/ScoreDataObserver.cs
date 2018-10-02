
public class ScoreDataObserver
{
    protected ScoreDataSubject subject;

    public ScoreDataObserver(ScoreDataSubject subject)
    {
        this.subject = subject;
        subject.Attach(this);
    }
    
    public virtual void OnNotify()
    {
        
    }

}
