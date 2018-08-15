
// More info: http://frhagn.github.io/Typewriter/




export class MessageDetailsModel {
    
    // ID
    public id: string = null;
    // INBOXID
    public inboxId: string = null;
    // DATESENT
    public dateSent: Date = new Date(0);
    // DATECREATED
    public dateCreated: Date = new Date(0);
    // DATEMODIFIED
    public dateModified: Date = new Date(0);
    // SUBJECT
    public subject: string = null;
    // FROM
    public from: string = null;
    // TO
    public to: string = null;
    // CC
    public cc: string = null;
    // BCC
    public bcc: string = null;
    // SIZEBYTES
    public sizeBytes: number = 0;
}
