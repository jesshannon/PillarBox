
// More info: http://frhagn.github.io/Typewriter/




export class MessageSummaryModel {
    
    // ID
    public id: string = null;
    // FROM
    public from: string = null;
    // TO
    public to: string = null;
    // SUBJECT
    public subject: string = null;
    // INTRO
    public intro: string = null;
    // DATESENT
    public dateSent: Date = new Date(0);
    // ISNEW
    public isNew: boolean = false;
    // INBOXID
    public inboxId: string = null;
}
